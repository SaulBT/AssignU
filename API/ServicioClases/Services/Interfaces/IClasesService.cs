using ServicioClases.Data.DTOs;
using ServicioClases.Models;

namespace ServicioClases.Services.Interfaces;

public interface IClasesService
{
    Task<Clase> crearClaseAsync(CrearClaseDTO crearClaseDto, HttpContext httpContext);
    Task<Clase> editarClase(ActualizarClaseDTO actualizarClaseDto, HttpContext httpContext);
    Task eliminarClase(int idClase, HttpContext httpContext);
    Task<Clase?> obtenerClasePorId(int idClase, HttpContext httpContext);
    Task<List<Clase>?> obtenerClasesDeAlumno(HttpContext httpContext);
    Task<List<Clase>?> obtenerClasesDeDocente(HttpContext httpContext);
    Task enviarFechaVisualizacion(int idClase, DateTime fechaVisualizacion, HttpContext httpContext);
    Task<Clase> unirseAClase(string codigoClase, HttpContext httpContext);
    Task salirDeClase(int idClase, HttpContext httpContext); 
}