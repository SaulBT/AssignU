using ServicioClases.Data.DTOs;
using ServicioClases.Models;

namespace ServicioClases.Data.DAOs.Interfaces;

public interface IClaseDAO
{
    Task<Clase> crearClaseAsync(Clase clase, string codigoClase);
    Task actualizarClaseAsync(Clase clase);
    Task eliminarClaseAsync(Clase clase);
    Task<Clase> obtenerClasePorIdAsync(int idClase);
    Task<Clase?> obtenerClasePorCodigoAsync(string codigoClase);
    Task<List<Clase>> obtenerClasesDeAlumnoAsync(List<Registro> registros);
    Task<List<Clase>> obtenerClasesDeDocenteAsync(int idDocente);
}