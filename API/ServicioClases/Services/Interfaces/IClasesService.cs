using ServicioClases.Data.DTOs;
using ServicioClases.Models;

namespace ServicioClases.Services.Interfaces;

public interface IClasesService
{
    Task<Clase> crearClaseAsync(CrearClaseDTO crearClaseDto, HttpContext httpContext);
    Task<Clase> editarClaseAsync(ActualizarClaseDTO actualizarClaseDto, HttpContext httpContext);
    Task eliminarClaseAsync(int idClase, HttpContext httpContext);
    Task<Clase?> obtenerClasePorIdAsync(int idClase);
    Task<List<Clase>?> obtenerClasesDeAlumnoAsync(HttpContext httpContext);
    Task<List<Clase>?> obtenerClasesDeDocenteAsync(HttpContext httpContext);
    Task enviarFechaVisualizacionAsync(int idClase, DateTime fechaVisualizacion, HttpContext httpContext);
    Task<Clase> unirseAClaseAsync(string codigoClase, HttpContext httpContext);
    Task salirDeClaseAsync(int idClase, HttpContext httpContext);
    Task<Registro> obtenerRegistroAlumno(int idAlumno, int idClase, HttpContext httpContext);
}