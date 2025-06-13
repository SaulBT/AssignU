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
        await enviarMensajeRPCAsync(mensajeJson);

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
        await enviarMensajeRPCAsync(mensajeJson);

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
        await enviarMensajeRPCAsync(mensajeJson);
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
        //Obtener las respuestas relacionadas a la tarea
        string mensajeJsonRespuestas = crearMensajeRPCConIdTarea("obtenerRespuestasDeClase", idTarea);
        var respuestas = (await enviarMensajeRPCConRespuestaAsync(mensajeJsonRespuestas, "cola_cuestionarios")).RespuestasDeTarea;
        //Obtener los datos de los alumnos
        List<int> listaIdAlumnos = [];
        foreach (var respuesta in respuestas)
        {
            listaIdAlumnos.Add(respuesta.idAlumno);
        }
        string mensajeAlumnos = crearMensajeRPCConIdAlumnos("obtenerListaAlumnos", listaIdAlumnos);
        var alumnos = enviarMensajeRPCConRespuestaAsync(mensajeAlumnos, "cola_clases_usuario")
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
                idTarea = idTarea,
                cuestionario = cuestionario
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
                idTarea = idTarea
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
            data = new
            {
                IdAlumnos = idAlumnos
            }
        };
        return System.Text.Json.JsonSerializer.Serialize(mensaje);
    }

    private async Task enviarMensajeRPCAsync(string mensajeJson)
    {
        string respuestaJson = await _rpcClient.CallAsync("cola_cuestionarios", mensajeJson);
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