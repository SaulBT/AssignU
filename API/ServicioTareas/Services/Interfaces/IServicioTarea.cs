using ServicioTareas.Data.DTOs;
using ServicioTareas.Models;

namespace ServicioTareas.Services.Interfaces;

public interface IServicioTarea
{
    Task<Tarea> CrearTareaAsync(CrearTareaDTO crearTareaDTO, HttpContext httpContext);
    Task<Tarea> EditarTareaAsync(EditarTareaDTO editarTareaDTO, HttpContext httpContext);
    Task EliminarTareaAsync(int idTarea, HttpContext httpContext);
    Task<List<Tarea>?> ObtenerTareasDeClaseAsync(int idClase);
}