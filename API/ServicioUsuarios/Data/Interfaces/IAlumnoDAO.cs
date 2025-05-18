using ServicioUsuarios.Entities;

public interface IAlumnoDAO
{
    Task registrarAsync(alumno alumno);
    Task actualizarAsync(alumno alumno);
    Task eliminarAsync(int id);
    Task<alumno?> obtenerPorIdAsync(int id);
}