using ServicioTareas.Data.DAOs.Interfaces;
using ServicioTareas.Data.DTOs;
using ServicioTareas.Exceptions;

namespace ServicioTareas.Validations;

public class TareasValidaciones
{
    private readonly ITareaDAO _tareaDAO;

    public TareasValidaciones(ITareaDAO tareaDAO)
    {
        _tareaDAO = tareaDAO;
    }

    public async Task VerificarCrearTareaAsync(CrearTareaDTO crearTareaDto, HttpContext httpContext)
    {
        validarAutorizacion(httpContext);
        ValidarIdClase(crearTareaDto.IdClase);
        validarNombreClase(crearTareaDto.Nombre);
        await verificarNombreTareaCreacionAsync(crearTareaDto.IdClase, crearTareaDto.Nombre);
    }

    public async Task VerificarEdicionDeTareaAsync(EditarTareaDTO editarTareaDTO, int idTarea, HttpContext httpContext)
    {
        validarAutorizacion(httpContext);
        validarNombreClase(editarTareaDTO.Nombre);
        await verificarNombreTareaEdicionAsync(idTarea, editarTareaDTO.Nombre);
        await verificarTareaExisteAsync(idTarea);
    }

    public async Task VerificarEliminacionDeTareaAsync(int idTarea, HttpContext httpContext)
    {
        validarAutorizacion(httpContext);
        ValidarIdTarea(idTarea);
        await verificarTareaExisteAsync(idTarea);
    }

    public void ValidarIdClase(int idClase)
    {
        if (idClase <= 0)
        {
            throw new ArgumentException("La id de la clase es inválida");
        }
    }

    public void ValidarIdTarea(int idTarea)
    {
        if (idTarea <= 0)
        {
            throw new IdInvalidaException("La id de la tarea es inválida");
        }
    }

    private void validarAutorizacion(HttpContext httpContext)
    {
        var tieneAutorizacion = httpContext.User.Identity?.IsAuthenticated ?? true;
        var claim = httpContext.User.FindFirst("idUsuario");
        var idUsuario = claim != null ? int.Parse(claim.Value) : 0;
        var tieneRolCorrecto = httpContext.User.IsInRole("docente");

        if (!tieneAutorizacion)
        {
            throw new UnauthorizedAccessException("Sin autorización para continuar");
        }
        else if (idUsuario <= 0)
        {
            throw new IdInvalidaException("La id del usuario es inválida");
        }
        else if (!tieneRolCorrecto)
        {
            throw new UnauthorizedAccessException("Sin autorización para continuar: rol incorrecto");
        }
    }

    private void validarNombreClase(string nombre)
    {
        if
        (String.IsNullOrEmpty(nombre))
        {
            throw new CampoObligatorioException("El nombre de la tarea es nula");
        }
    }

    private async Task verificarNombreTareaCreacionAsync(int idClase, string nombre)
    {
        var tarea = await _tareaDAO.ObtenerTareaPorIdClaseYNombreAsync(idClase, nombre);
        if (tarea != null)
        {
            throw new RecursoYaExistenteException("Nombre de la tarea repetido");
        }
    }

    private async Task verificarNombreTareaEdicionAsync(int idTarea, string nombre)
    {
        var tarea = await _tareaDAO.ObtenerTareaPorIdTareaYNombreAsync(idTarea, nombre);
        if (tarea != null)
        {
            throw new RecursoYaExistenteException("Nombre de la tarea repetido");
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