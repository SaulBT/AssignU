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
        await enviarMensajeRPC(mensajeJson);

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
        await enviarMensajeRPC(mensajeJson);

        return tarea;
    }

    public async Task EliminarTareaAsync(int idTarea, HttpContext httpContext)
    {
        validarAutorizacion(httpContext);
        validarIdTarea(idTarea);
        await verificarTareaExisteAsync(idTarea);

        var tarea = await _tareaDAO.ObtenerTareaPorIdAsync(idTarea);
        await _tareaDAO.EliminarTareaAsync(tarea);
        string mensajeJson = crearMensajeRPC("eliminarCuestionario", idTarea);
        await enviarMensajeRPC(mensajeJson);
    }

    public async Task<List<Tarea>?> ObtenerTareasDeClaseAsync(int idClase)
    {
        validarIdClase(idClase);

        var tareas = await _tareaDAO.ObtenerTareasPorIdClaseAsync(idClase);
        return tareas;
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
            accion = accion,
            data = new
            {
                idTarea = idTarea,
                cuestionario = cuestionario
            }
        };
        return System.Text.Json.JsonSerializer.Serialize(mensaje);
    }

    private string crearMensajeRPC(string accion, int idTarea)
    {
        var mensaje = new
        {
            accion = accion,
            data = new
            {
                idTarea = idTarea
            }
        };
        return System.Text.Json.JsonSerializer.Serialize(mensaje);
    }

    private async Task enviarMensajeRPC(string mensajeJson)
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