using ServicioClases.Models;

namespace ServicioClases.Data.DAOs.Interfaces;

public interface IClaseDAO
{
    Task<Clase> CrearClaseAsync(Clase clase, string codigoClase);
    Task ActualizarClaseAsync(Clase clase);
    Task EliminarClaseAsync(Clase clase);
    Task<Clase> ObtenerClasePorIdAsync(int idClase);
    Task<Clase?> ObtenerClasePorCodigoAsync(string codigoClase);
    Task<List<Clase>> ObtenerClasesDeAlumnoAsync(List<Registro> registros);
    Task<List<Clase>> ObtenerClasesDeDocenteAsync(int idDocente);
}