using ServicioTareas.Data.DAOs.Interfaces;
using ServicioTareas.Data.DTOs;
using ServicioTareas.Exceptions;
using ServicioTareas.Models;
using ServicioTareas.Config;
using ServicioTareas.Services.Interfaces;
using ServicioTareas.Validations;

namespace ServicioTareas.Services.Implementations;

public class ServicioTarea : IServicioTarea
{
    private readonly ITareaDAO _tareaDAO;
    private readonly RpcClientRabbitMQ _rpcClient;

    public ServicioTarea(ITareaDAO tareaDAO, RpcClientRabbitMQ rpcClient)
    {
        _tareaDAO = tareaDAO;
        _rpcClient = rpcClient;
    }

    public async Task<Tarea> CrearTareaAsync(CrearTareaDTO crearTareaDTO, HttpContext httpContext)
    {
        validarAutorizacion(httpContext);
        validarCampos(crearTareaDTO.IdClase, crearTareaDTO.Nombre, crearTareaDTO.IdArchivo);
        await verificarNombreTareaCreacionAsync(crearTareaDTO.IdClase, crearTareaDTO.Nombre);

        var tarea = await _tareaDAO.CrearTareaAsync(crearTareaDTO);
        var cuestionario = crearTareaDTO.Cuestionario;
        string mensajeJson = crearMensajeRPCConCuestionario("crearCuestionario", tarea.IdTarea, cuestionario);
        await enviarMensajeRPCAsync(mensajeJson, "cola_cuestionario");

        return tarea;
    }

    public async Task<Tarea> EditarTareaAsync(EditarTareaDTO editarTareaDTO, HttpContext httpContext)
    {
        validarAutorizacion(httpContext);
        validarCampos(editarTareaDTO.IdTarea, editarTareaDTO.Nombre, editarTareaDTO.IdArchivo);
        await verificarNombreTareaEdicionAsync(editarTareaDTO.IdTarea, editarTareaDTO.Nombre);
        await verificarTareaExisteAsync(editarTareaDTO.IdTarea);

        var tarea = await _tareaDAO.EditarTareaAsync(editarTareaDTO);
        var cuestionario = editarTareaDTO.Cuestionario;
        string mensajeJson = crearMensajeRPCConCuestionario("editarCuestionario", tarea.IdTarea, cuestionario);
        await enviarMensajeRPCAsync(mensajeJson, "cola_cuestionario");

        return tarea;
    }

    public async Task EliminarTareaAsync(int idTarea, HttpContext httpContext)
    {
        validarAutorizacion(httpContext);
        validarIdTarea(idTarea);
        await verificarTareaExisteAsync(idTarea);

        var tarea = await _tareaDAO.ObtenerTareaPorIdAsync(idTarea);
        await _tareaDAO.EliminarTareaAsync(tarea);
        string mensajeJson = crearMensajeRPCConIdTarea("eliminarCuestionario", idTarea);
        await enviarMensajeRPCAsync(mensajeJson, "cola_cuestionario");
    }

    public async Task<List<Tarea>?> ObtenerTareasDeClaseAsync(int idClase)
    {
        validarIdClase(idClase);

        var tareas = await _tareaDAO.ObtenerTareasPorIdClaseAsync(idClase);
        return tareas;
    }

    public async Task<RespuestaRPCDTO> ObtenerTareasYRespuestasAsync(int idClase)
    {
        var tareas = await ObtenerTareasDeClaseAsync(idClase);
        List<int> idTareas = new List<int>();
        List<TareaEstadisticasClaseDTO> tareasDto = new List<TareaEstadisticasClaseDTO>();
        foreach (Tarea tarea in tareas)
        {
            var tareaDto = new TareaEstadisticasClaseDTO
            {
                IdTarea = tarea.IdTarea,
                Nombre = tarea.Nombre
            };
            int idTarea = tarea.IdTarea;
            idTareas.Add(idTarea);
            tareasDto.Add(tareaDto);
        }
        string mensajeJson = crearMensajeRPCConIdTareas("obtenerRespuestas", idTareas);
        var respuestas = await enviarMensajeRPCConRespuestaAsync(mensajeJson, "cola_cuestionarios");

        RespuestaRPCDTO respuestaRpc = new RespuestaRPCDTO
        {
            Success = respuestas.Success,
            Tareas = tareasDto,
            Respuestas = respuestas.Respuestas,
            Error = respuestas.Error
        };
        
        return respuestaRpc;
    }

    public async Task<EstadisticasTareaDTO> ObtenerEstadisticasTareaAsync(int idTarea)
    {
        validarIdTarea(idTarea);
        string mensajePreguntas = crearMensajeRPCConIdTarea("obtenerPreguntasDeTarea", idTarea);
        var preguntas = (await enviarMensajeRPCConRespuestaAsync(mensajePreguntas, "cola_cuestionarios")).Preguntas;
        Console.WriteLine("Número preguntas: " + preguntas.Count);
        Console.WriteLine("Texto de la pregunta: " + preguntas[0].Texto);
        //Obtener las respuestas relacionadas a la tarea
        string mensajeJsonRespuestas = crearMensajeRPCConIdTarea("obtenerRespuestasDeClase", idTarea);
        var respuestas = (await enviarMensajeRPCConRespuestaAsync(mensajeJsonRespuestas, "cola_cuestionarios")).RespuestasDeTarea;
        Console.WriteLine("Número respuestas: " + respuestas.Count);
        //Obtener los datos de los alumnos
        List<int> listaIdAlumnos = [];
        foreach (var respuesta in respuestas)
        {
            listaIdAlumnos.Add(respuesta.idAlumno);
        }
        string mensajeAlumnos = crearMensajeRPCConIdAlumnos("obtenerListaAlumnos", listaIdAlumnos);
        var alumnos = (await enviarMensajeRPCConRespuestaAsync(mensajeAlumnos, "cola_usuarios")).Alumnos;
        Console.WriteLine("Número alumnos: " + alumnos.Count);
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

        List<string> ListaPreguntas = [];
        foreach (var pregunta in preguntas)
        {
            string texto = pregunta.Texto;
            Console.WriteLine(pregunta.Texto);
            ListaPreguntas.Add(texto);
        }
        Console.WriteLine("Preguntas: " + ListaPreguntas.Count + ", Respuestas: " + respuestas.Count + ", Alumnos: " + alumnos.Count);
        var estadistica = new EstadisticasTareaDTO
        {
            TextoPreguntas = ListaPreguntas,
            Respuestas = respuestas,
            Alumnos = listaAlumnos
        };

        return estadistica;
    }

    public async Task<RespuestaRPCDTO> EliminarTareasDeClaseAsync(int idClase)
    {
        validarIdClase(idClase);

        var tareas = await _tareaDAO.ObtenerTareasPorIdClaseAsync(idClase);
        foreach (var tarea in tareas)
        {
            await _tareaDAO.EliminarTareaAsync(tarea);
            string mensajeCuestionario = crearMensajeRPCConIdTarea("eliminarCuestionario", tarea.IdTarea);
            await enviarMensajeRPCAsync(mensajeCuestionario, "cola_cuestionarios");
            Console.WriteLine("Se crea el mensaje para archivos");
            string mensajeArchivo = crearMensajeRPCConIdTarea("eliminarArchivo", tarea.IdTarea);
            Console.WriteLine("Se envía la petición");
            await enviarMensajeRPCAsync(mensajeArchivo, "cola_archivos");
            Console.WriteLine("Se recibe exitosamente");
        }

        return new RespuestaRPCDTO
        {
            Success = true
        };
    }

    private void validarAutorizacion(HttpContext httpContext)
    {
        var tieneAutorizacion = httpContext.User.Identity?.IsAuthenticated ?? true;
        var claim = httpContext.User.FindFirst("idUsuario");
        var idUsuario = claim != null ? int.Parse(claim.Value) : 0;
        var tieneRolCorrecto = httpContext.User.IsInRole("docente");

        if (!tieneAutorizacion || idUsuario <= 0 || !tieneRolCorrecto)
        {
            throw new UnauthorizedAccessException("Sin autorización para continuar");
        }
    }

    private void validarCampos(int id, string nombre, int idArchivo)
    {
        if
        (
            id <= 0 ||
            String.IsNullOrEmpty(nombre) ||
            idArchivo < 0
        )
        {
            throw new ArgumentException("Hay campos inválidos");
        }
    }

    private void validarIdTarea(int idTarea)
    {
        if (idTarea <= 0)
        {
            throw new ArgumentException("La id de la tarea es inválida");
        }
    }

    private void validarIdClase(int idClase)
    {
        if (idClase <= 0)
        {
            throw new ArgumentException("La id de la clase es inválida");
        }
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
            throw LanzarExcepciones.lanzarExcepcion(respuesta.Error.Tipo, respuesta.Error.Mensaje);
        }
    }

    private async Task<RespuestaRPCDTO> enviarMensajeRPCConRespuestaAsync(string mensajeJson, string cola)
    {
        string respuestaJson = await _rpcClient.CallAsync(cola, mensajeJson);
        var respuesta = System.Text.Json.JsonSerializer.Deserialize<RespuestaRPCDTO>(respuestaJson);

        return respuesta;
    }

    private async Task verificarNombreTareaCreacionAsync(int idClase, string nombre)
    {
        var tarea = await _tareaDAO.ObtenerTareaPorIdClaseYNombreAsync(idClase, nombre);
        if (tarea != null)
        {
            throw new RecursoYaExistenteException("Nombre repetido");
        }
    }

    private async Task verificarNombreTareaEdicionAsync(int idTarea, string nombre)
    {
        var tarea = await _tareaDAO.ObtenerTareaPorIdTareaYNombreAsync(idTarea, nombre);
        if (tarea != null)
        {
            throw new RecursoYaExistenteException("Nombre repetido");
        }
    }

    private async Task verificarTareaExisteAsync(int idTarea)
    {
        var tarea = await _tareaDAO.ObtenerTareaPorIdAsync(idTarea);
        if (tarea == null)
        {
            throw new RecursoNoEncontradoException($"No se encontró una tarea con la id {idTarea}");
        }
    }
}