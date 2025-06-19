using ServicioClases.Data.DTOs;
using ServicioClases.Data.DTOs.RPC;
using ServicioClases.Models;

namespace ServicioClases.Services.Interfaces;

public interface IServicioClase
{
    Task<ClaseDTO> CrearClaseAsync(CrearClaseDTO crearClaseDto, HttpContext httpContext);
    Task<ClaseDTO> EditarClaseAsync(int idClase, ActualizarClaseDTO actualizarClaseDto, HttpContext httpContext);
    Task EliminarClaseAsync(int idClase, HttpContext httpContext);
    Task<ClaseDTO?> ObtenerClasePorIdAsync(int idClase);
    Task<List<Clase>?> ObtenerClasesDeAlumnoAsync(int idAlumno);
    Task<List<Clase>?> ObtenerClasesDeDocenteAsync(int idDocente);
    Task EnviarFechaVisualizacionAsync(int idClase, DateTime fechaVisualizacion, HttpContext httpContext);
    Task<Clase> UnirseAClaseAsync(string codigoClase, HttpContext httpContext);
    Task SalirDeClaseAsync(int idAlumno, int idClase, HttpContext httpContext);
    Task<Registro> ObtenerRegistroAlumno(int idAlumno, int idClase, HttpContext httpContext);
    Task<EstadisticasClaseDTO> ObtenerEstadisticasDeLaClase(int idClase);
    Task<RespuestaRPCDTO> ObtenerClasesTareasRespuestasDeAlumnoAsync(int idAlumno);
}