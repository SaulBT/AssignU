using ServicioTareas.Data.DAOs.Interfaces;
using ServicioTareas.Data.DTOs;
using ServicioTareas.Models;
using ServicioTareas.Config;
using ServicioTareas.Services.Interfaces;
using ServicioTareas.Validations;
using ServicioTareas.Data.DTOs.RPC;

namespace ServicioTareas.Services.Implementations;

public class ServicioTarea : IServicioTarea
{
    private readonly ITareaDAO _tareaDAO;
    private readonly RpcClientRabbitMQ _rpcClient;
    private readonly TareasValidaciones _validaciones;
    private readonly ILogger<ServicioTarea> _logger;

    public ServicioTarea(ITareaDAO tareaDAO, RpcClientRabbitMQ rpcClient, TareasValidaciones validaciones, ILogger<ServicioTarea> logger)
    {
        _tareaDAO = tareaDAO;
        _rpcClient = rpcClient;
        _validaciones = validaciones;
        _logger = logger;
    }

    public async Task<TareaDTO> CrearTareaAsync(CrearTareaDTO crearTareaDto, HttpContext httpContext)
    {
        _logger.LogInformation("Crear Tarea");
        await _validaciones.VerificarCrearTareaAsync(crearTareaDto, httpContext);

        var tarea = await _tareaDAO.CrearTareaAsync(crearTareaDto);

        var cuestionario = crearTareaDto.Cuestionario;
        string mensajeJson = crearMensajeRPCConCuestionario("crearCuestionario", tarea.IdTarea, cuestionario);
        await enviarMensajeRPCAsync(mensajeJson, "cola_cuestionarios");

        _logger.LogInformation($"Tarea creada con la id {tarea.IdTarea}");
        return new TareaDTO
        {
            IdTarea = tarea.IdTarea,
            IdClase = tarea.IdClase,
            Nombre = tarea.Nombre,
            FechaLimite = tarea.FechaLimite
        };
    }

    public async Task<TareaDTO> EditarTareaAsync(EditarTareaDTO editarTareaDTO, int idTarea, HttpContext httpContext)
    {
        _logger.LogInformation("Editando una Tarea");
        await _validaciones.VerificarEdicionDeTareaAsync(editarTareaDTO, idTarea, httpContext);

        var tarea = await _tareaDAO.EditarTareaAsync(editarTareaDTO);

        var cuestionario = editarTareaDTO.Cuestionario;
        string mensajeJson = crearMensajeRPCConCuestionario("editarCuestionario", tarea.IdTarea, cuestionario);
        await enviarMensajeRPCAsync(mensajeJson, "cola_cuestionarios");

        _logger.LogInformation($"Tarea con id {tarea.IdTarea} editada");
        return new TareaDTO
        {
            IdTarea = tarea.IdTarea,
            IdClase = tarea.IdClase,
            Nombre = tarea.Nombre,
            FechaLimite = tarea.FechaLimite
        };
    }

    public async Task EliminarTareaAsync(int idTarea, HttpContext httpContext)
    {
        _logger.LogInformation("Eliminando una Tarea");
        await _validaciones.VerificarEliminacionDeTareaAsync(idTarea, httpContext);

        var tarea = await _tareaDAO.ObtenerTareaPorIdAsync(idTarea);
        await _tareaDAO.EliminarTareaAsync(tarea);

        string mensajeJson = crearMensajeRPCConIdTarea("eliminarCuestionario", idTarea);
        await enviarMensajeRPCAsync(mensajeJson, "cola_cuestionarios");

        _logger.LogInformation($"Se eliminó la Tarea con la id {idTarea}");
    }

    public async Task<List<Tarea>?> ObtenerTareasDeClaseAsync(int idClase)
    {
        _logger.LogInformation("Buscando una Tarea");
        _validaciones.ValidarIdClase(idClase);

        var tareas = await _tareaDAO.ObtenerTareasPorIdClaseAsync(idClase);
        _logger.LogInformation($"Tareas recuperadas de la Clase con id {idClase}");
        
        return tareas;
    }

    public async Task<RespuestaRPCDTO> ObtenerTareasYRespuestasAsync(int idClase)
    {
        try
        {
            _logger.LogInformation("Recuperando Tareas y Respuestas de una Clase");
            var tareas = await ObtenerTareasDeClaseAsync(idClase);
            var listaIdTareas = generarListaIdTareas(tareas);
            var listaTareas = generarListaTareas(tareas);

            string mensajeJson = crearMensajeRPCConIdTareas("obtenerRespuestasDeListaTareas", listaIdTareas);
            var respuestas = await enviarMensajeRPCConRespuestaAsync(mensajeJson, "cola_cuestionarios");

            RespuestaRPCDTO respuestaRpc = new RespuestaRPCDTO
            {
                Success = respuestas.Success,
                Tareas = listaTareas,
                Respuestas = respuestas.Respuestas,
            };

            return respuestaRpc;
        }
        catch (Exception ex)
        {
            return DetectorExcepciones.detectarExcepcion(ex);
        }
    }

    public async Task<EstadisticasTareaDTO> ObtenerEstadisticasTareaAsync(int idTarea)
    {
        _validaciones.ValidarIdTarea(idTarea);

        string mensajePreguntas = crearMensajeRPCConIdTarea("obtenerPreguntasDeTarea", idTarea);
        var preguntas = (await enviarMensajeRPCConRespuestaAsync(mensajePreguntas, "cola_cuestionarios")).Preguntas;
        
        string mensajeJsonRespuestas = crearMensajeRPCConIdTarea("obtenerRespuestasDeClase", idTarea);
        var respuestas = (await enviarMensajeRPCConRespuestaAsync(mensajeJsonRespuestas, "cola_cuestionarios")).RespuestasDeTarea;
        
        var listaIdAlumnos = generarListaIdAlumnos(respuestas);
        string mensajeAlumnos = crearMensajeRPCConIdAlumnos("obtenerListaAlumnos", listaIdAlumnos);
        var alumnos = (await enviarMensajeRPCConRespuestaAsync(mensajeAlumnos, "cola_usuarios")).Alumnos;

        var listaAlumnos = generarListaAlumnos(alumnos);
        var listaPreguntas = generarListaPreguntas(preguntas);
        
        var estadistica = new EstadisticasTareaDTO
        {
            TextoPreguntas = listaPreguntas,
            Respuestas = respuestas,
            Alumnos = listaAlumnos
        };

        return estadistica;
    }

    public async Task<RespuestaRPCDTO> EliminarTareasDeClaseAsync(int idClase)
    {
        _validaciones.ValidarIdClase(idClase);

        var tareas = await _tareaDAO.ObtenerTareasPorIdClaseAsync(idClase);
        foreach (var tarea in tareas)
        {
            await _tareaDAO.EliminarTareaAsync(tarea);
            string mensajeCuestionario = crearMensajeRPCConIdTarea("eliminarCuestionario", tarea.IdTarea);
            await enviarMensajeRPCAsync(mensajeCuestionario, "cola_cuestionarios");
            
            string mensajeArchivo = crearMensajeRPCConIdTarea("eliminarArchivo", tarea.IdTarea);
            await enviarMensajeRPCAsync(mensajeArchivo, "cola_archivos");
        }

        return new RespuestaRPCDTO
        {
            Success = true
        };
    }

    private List<int> generarListaIdTareas(List<Tarea> tareas)
    {
        List<int> listaIdTareas = new List<int>();
        foreach (Tarea tarea in tareas)
        {
            int idTarea = tarea.IdTarea;
            listaIdTareas.Add(idTarea);
        }

        return listaIdTareas;
    }

    private List<TareaEstadisticasClaseDTO> generarListaTareas(List<Tarea> tareas)
    {
        List<TareaEstadisticasClaseDTO> listaTareas = new List<TareaEstadisticasClaseDTO>();
        foreach (Tarea tarea in tareas)
        {
            var tareaDto = new TareaEstadisticasClaseDTO
            {
                IdTarea = tarea.IdTarea,
                Nombre = tarea.Nombre
            };
            listaTareas.Add(tareaDto);
        }

        return listaTareas;
    }

    private List<int> generarListaIdAlumnos(List<RespuestaDTO> respuestas)
    {
        List<int> listaIdAlumnos = [];
        foreach (var respuesta in respuestas)
        {
            listaIdAlumnos.Add(respuesta.IdAlumno);
        }

        return listaIdAlumnos;
    }

    private List<AlumnoEstadisticaTareaDTO> generarListaAlumnos(List<AlumnoEstadisticasDTO> alumnos)
    {
        List<AlumnoEstadisticaTareaDTO> listaAlumnos = [];
        foreach (var alumno in alumnos)
        {
            var alumnoEnLista = new AlumnoEstadisticaTareaDTO
            {
                IdAlumno = alumno.IdAlumno,
                NombreCompleto = alumno.NombreCompleto
            };
            listaAlumnos.Add(alumnoEnLista);
        }

        return listaAlumnos;
    }

    private List<string> generarListaPreguntas(List<PreguntaDTO> preguntas)
    {
        List<string> listaPreguntas = [];
        foreach (var pregunta in preguntas)
        {
            string texto = pregunta.Texto;
            listaPreguntas.Add(texto);
        }

        return listaPreguntas;
    }

    private string crearMensajeRPCConCuestionario(string accion, int idTarea, CuestionarioDTO cuestionario)
    {
        var mensaje = new
        {
            Accion = accion,
            data = new
            {
                IdTarea = idTarea,
                Cuestionario = cuestionario
            }
        };
        return System.Text.Json.JsonSerializer.Serialize(mensaje);
    }

    private string crearMensajeRPCConIdTarea(string accion, int idTarea)
    {
        var mensaje = new
        {
            Accion = accion,
            data = new
            {
                IdTarea = idTarea
            }
        };
        return System.Text.Json.JsonSerializer.Serialize(mensaje);
    }

    private string crearMensajeRPCConIdTareas(string accion, List<int> idTareas)
    {
        var mensaje = new
        {
            Accion = accion,
            data = new
            {
                IdTareas = idTareas
            }
        };
        return System.Text.Json.JsonSerializer.Serialize(mensaje);
    }

    private string crearMensajeRPCConIdAlumnos(string accion, List<int> idAlumnos)
    {
        var mensaje = new
        {
            Accion = accion,
            IdAlumnos = idAlumnos
        };
        return System.Text.Json.JsonSerializer.Serialize(mensaje);
    }

    private async Task enviarMensajeRPCAsync(string mensajeJson, string cola)
    {
        string respuestaJson = await _rpcClient.CallAsync(cola, mensajeJson);
        var respuesta = System.Text.Json.JsonSerializer.Deserialize<RespuestaCuestionarioDTO>(respuestaJson);
        if (respuesta == null)
        {
            throw new Exception("No hay respuesta");
        }
        else if (!respuesta.Success)
        {
            if (respuesta.Error == null)
            {
                throw new InvalidOperationException("No hay error");
            }
            if (respuesta.Error.Tipo != "ArchivoNoEncontrado")
            {
                throw LanzarExcepciones.lanzarExcepcion(respuesta.Error.Tipo, respuesta.Error.Mensaje);
            }
        }
    }

    private async Task<RespuestaRPCDTO> enviarMensajeRPCConRespuestaAsync(string mensajeJson, string cola)
    {
        string respuestaJson = await _rpcClient.CallAsync(cola, mensajeJson);
        var respuesta = System.Text.Json.JsonSerializer.Deserialize<RespuestaRPCDTO>(respuestaJson);
        if (respuesta == null)
        {
            throw new Exception("No hay respuesta");
        }
        else if (!respuesta.Success)
        {
            if (respuesta.Error == null)
            {
                throw new InvalidOperationException("No hay error");
            }
            throw LanzarExcepciones.lanzarExcepcion(respuesta.Error.Tipo, respuesta.Error.Mensaje);
        }

        return respuesta;
    }
}