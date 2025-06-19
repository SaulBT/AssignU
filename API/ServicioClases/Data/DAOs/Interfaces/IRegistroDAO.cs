using ServicioClases.Models;

namespace ServicioClases.Data.DAOs.Interfaces;

public interface IRegistroDAO
{
    Task CrearRegistroAsync(Registro registro);
    Task EliminarRegistroAsync(Registro registro);
    Task ActualizarRegistroAsync(Registro registro);
    Task<List<Registro>> ObtenerRegistrosPorClaseAsync(int idClase);
    Task<List<Registro>> ObtenerRegistrosPorAlumnoAsync(int idAlumno);
    Task<Registro?> ObtenerRegistroPorIdAlumnoYClaseAsync(int idAlumno, int idClase);
}