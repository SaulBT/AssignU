using ServicioClases.Data.DTOs;
using ServicioClases.Models;

namespace ServicioClases.Services.Interfaces;

public interface IServicioClase
{
    Task<Clase> CrearClaseAsync(CrearClaseDTO crearClaseDto, HttpContext httpContext);
    Task<Clase> EditarClaseAsync(ActualizarClaseDTO actualizarClaseDto, HttpContext httpContext);
    Task EliminarClaseAsync(int idClase, HttpContext httpContext);
    Task<Clase?> ObtenerClasePorIdAsync(int idClase);
    Task<List<Clase>?> ObtenerClasesDeAlumnoAsync(HttpContext httpContext);
    Task<List<Clase>?> ObtenerClasesDeDocenteAsync(HttpContext httpContext);
    Task EnviarFechaVisualizacionAsync(int idClase, DateTime fechaVisualizacion, HttpContext httpContext);
    Task<Clase> UnirseAClaseAsync(string codigoClase, HttpContext httpContext);
    Task SalirDeClaseAsync(int idClase, HttpContext httpContext);
    Task<Registro> ObtenerRegistroAlumno(int idAlumno, int idClase, HttpContext httpContext);
    Task<EstadisticasClaseDTO> ObtenerEstadisticasDeLaClase(int idClase);
    Task<RespuestaRPCDTO> ObtenerClasesTareasRespuestasDeAlumnoAsync(int idAlumno);
}