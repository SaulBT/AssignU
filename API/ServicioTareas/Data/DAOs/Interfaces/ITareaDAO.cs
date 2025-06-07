using ServicioTareas.Data.DTOs;
using ServicioTareas.Models;

namespace ServicioTareas.Data.DAOs.Interfaces;

public interface ITareaDAO
{
    Task<Tarea> crearTareaAsync(CrearTareaDTO crearTareaDTO);
    Task<Tarea> editarTareaAsync(EditarTareaDTO editarTareaDTO);
    Task eliminarTareaAsync(Tarea tarea);
    Task<Tarea?> obtenerTareaPorIdAsync(int idTarea);
    Task<Tarea?> obtenerTareaPorIdClaseYNombreAsync(int idClase, string nombre);
    Task<Tarea?> obtenerTareaPorIdTareaYNombreAsync(int idTarea, string nombre);
    Task<List<Tarea>?> obtenerTareasPorIdClaseAsync(int idClase);
}