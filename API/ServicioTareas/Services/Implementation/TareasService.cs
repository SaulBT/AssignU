using ServicioTareas.Data.DAOs.Interfaces;
using ServicioTareas.Data.DTOs;
using ServicioTareas.Exceptions;
using ServicioTareas.Models;
using ServicioTareas.Services.Interfaces;

namespace ServicioTareas.Services.Implementations;

public class TareasService : ITareasServices
{
    private readonly ITareaDAO _tareaDAO;
    private readonly RabbitMQPublisher _rabbitMQPublisher;

    public TareasService(ITareaDAO tareaDAO)
    {
        _tareaDAO = tareaDAO;
    }

    public async Task<Tarea> crearTareaAsync(CrearTareaDTO crearTareaDTO, HttpContext httpContext)
    {
        validarAutorizacion(httpContext);
        validarCampos(crearTareaDTO.idClase, crearTareaDTO.nombre, crearTareaDTO.idArchivo);
        await verificarNombreTareaCreacionAsync(crearTareaDTO.idClase, crearTareaDTO.nombre);

        var tarea = await _tareaDAO.crearTareaAsync(crearTareaDTO);
        var cuestionario = crearTareaDTO.cuestionario;
        cuestionario.idTarea = tarea.IdTarea;
        await _rabbitMQPublisher.PublicarCuestionarioAsync(cuestionario);

        return tarea;
    }

    public async Task<Tarea> editarTareaAsync(EditarTareaDTO editarTareaDTO, HttpContext httpContext)
    {
        validarAutorizacion(httpContext);
        validarCampos(editarTareaDTO.idTarea, editarTareaDTO.nombre, editarTareaDTO.idArchivo);
        await verificarNombreTareaEdicionAsync(editarTareaDTO.idTarea, editarTareaDTO.nombre);
        await verificarTareaExisteAsync(editarTareaDTO.idTarea);

        var tarea = await _tareaDAO.editarTareaAsync(editarTareaDTO);
        return tarea;
    }

    public async Task eliminarTareaAsync(int idTarea, HttpContext httpContext)
    {
        validarAutorizacion(httpContext);
        validarIdTarea(idTarea);
        await verificarTareaExisteAsync(idTarea);

        var tarea = await _tareaDAO.obtenerTareaPorIdAsync(idTarea);
        await _tareaDAO.eliminarTareaAsync(tarea);
    }

    public async Task<List<Tarea>?> obtenerTareasDeClaseAsync(int idClase)
    {
        validarIdClase(idClase);

        var tareas = await _tareaDAO.obtenerTareasPorIdClaseAsync(idClase);
        return tareas;
    }

    private void validarAutorizacion(HttpContext httpContext)
    {
        var tieneAutorizacion = httpContext.User.Identity?.IsAuthenticated ?? true;
        //Console.WriteLine("¿Tiene autorización? " + tieneAutorizacion);
        var claim = httpContext.User.FindFirst("idUsuario");
        var idUsuario = claim != null ? int.Parse(claim.Value) : 0;
        //Console.WriteLine("IdUsuario: " + idUsuario);
        var tieneRolCorrecto = httpContext.User.IsInRole("docente");
        //Console.WriteLine("¿Tiene rol correcto? " + tieneRolCorrecto);

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

    private async Task verificarNombreTareaCreacionAsync(int idClase, string nombre)
    {
        var tarea = await _tareaDAO.obtenerTareaPorIdClaseYNombreAsync(idClase, nombre);
        if (tarea != null)
        {
            throw new RecursoYaExistenteException("Nombre repetido");
        }
    }

    private async Task verificarNombreTareaEdicionAsync(int idTarea, string nombre)
    {
        var tarea = await _tareaDAO.obtenerTareaPorIdTareaYNombreAsync(idTarea, nombre);
        if (tarea != null)
        {
            throw new RecursoYaExistenteException("Nombre repetido");
        }
    }

    private async Task verificarTareaExisteAsync(int idTarea)
    {
        var tarea = await _tareaDAO.obtenerTareaPorIdAsync(idTarea);
        if (tarea == null)
        {
            throw new RecursoNoEncontradoException($"No se encontró una tarea con la id {idTarea}");
        }
    }
}