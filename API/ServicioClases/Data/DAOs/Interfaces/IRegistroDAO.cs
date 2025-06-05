using ServicioClases.Models;

namespace ServicioClases.Data.DAOs.Interfaces;

public interface IRegistroDAO
{
    Task crearRegistroAsync(Registro registro);
    Task eliminarRegistroAsync(int idRegistro);
    Task actualizarRegistroAsync(Registro registro);
    Task<List<Registro>> obtenerRegistrosPorClaseAsync(int idClase);
    Task<List<Registro>> obtenerRegistrosPorAlumnoAsync(int idAlumno);
    Task<Registro?> obtenerRegistroPorIdAlumnoYClaseAsync(int idAlumno, int idClase);
}