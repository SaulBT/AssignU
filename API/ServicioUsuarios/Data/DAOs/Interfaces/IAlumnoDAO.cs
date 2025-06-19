using ServicioUsuarios.Data.DTOs.Alumno;
using ServicioUsuarios.Models;

namespace ServicioUsuarios.Data.DAOs.Interfaces;

public interface IAlumnoDAO
{
    Task ActualizarAsync(Alumno alumno);
    Task EliminarAsync(Alumno alumnoAEliminar);
    Task<Alumno> ObtenerAlumnoPorIdAsync(int id);
    Task<Alumno?> ObtenerPorNombreUsuarioAsync(string nombreUsuario);
    Task<Alumno?> ObtenerPorNombreUsuarioEIdAsync(string nombreUsuario, int id);
    Task<Alumno?> ObtenerPorCorreoAsync(string correo);
    Task AgregarAlumnoAsync(Alumno alumno);
    Task<Alumno> ObtenerPorNombreUsuarioOCorreoAsync(string nombreCompletoOCorreo);
}