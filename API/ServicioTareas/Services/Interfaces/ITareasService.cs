using ServicioTareas.Data.DTOs;
using ServicioTareas.Models;

namespace ServicioTareas.Services.Interfaces;

public interface ITareasServices
{
    Task<Tarea> crearTareaAsync(CrearTareaDTO crearTareaDTO, HttpContext httpContext);
    Task<Tarea> editarTareaAsync(EditarTareaDTO editarTareaDTO, HttpContext httpContext);
    Task eliminarTareaAsync(int idTarea, HttpContext httpContext);
    Task<List<Tarea>?> obtenerTareasDeClaseAsync(int idClase);
}