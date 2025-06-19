using ServicioTareas.Data.DTOs;
using ServicioTareas.Models;

namespace ServicioTareas.Data.DAOs.Interfaces;

public interface ITareaDAO
{
    Task<Tarea> CrearTareaAsync(CrearTareaDTO crearTareaDTO);
    Task<Tarea> EditarTareaAsync(EditarTareaDTO editarTareaDTO);
    Task EliminarTareaAsync(Tarea tarea);
    Task<Tarea?> ObtenerTareaPorIdAsync(int idTarea);
    Task<Tarea?> ObtenerTareaPorIdClaseYNombreAsync(int idClase, string nombre);
    Task<Tarea?> ObtenerTareaPorIdTareaYNombreAsync(int idTarea, string nombre);
    Task<List<Tarea>?> ObtenerTareasPorIdClaseAsync(int idClase);
}