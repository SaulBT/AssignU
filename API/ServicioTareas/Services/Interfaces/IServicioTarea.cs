using ServicioTareas.Data.DTOs;
using ServicioTareas.Data.DTOs.RPC;
using ServicioTareas.Models;

namespace ServicioTareas.Services.Interfaces;

public interface IServicioTarea
{
    Task<TareaDTO> CrearTareaAsync(CrearTareaDTO crearTareaDTO, HttpContext httpContext);
    Task<TareaDTO> EditarTareaAsync(EditarTareaDTO editarTareaDTO, int idTarea, HttpContext httpContext);
    Task EliminarTareaAsync(int idTarea, HttpContext httpContext);
    Task<List<Tarea>?> ObtenerTareasDeClaseAsync(int idClase);
    Task<RespuestaRPCDTO> ObtenerTareasYRespuestasAsync(int idClase);
    Task<EstadisticasTareaDTO> ObtenerEstadisticasTareaAsync(int idTarea);
    Task<RespuestaRPCDTO> EliminarTareasDeClaseAsync(int idClase);
}