using ServicioUsuarios.DTOs;
using ServicioUsuarios.Entities;

namespace ServicioUsuarios.DAOs.Interfaces;

public interface IAlumnoDAO
{
    Task ActualizarAsync(alumno alumno);
    Task EliminarAsync(int id);
    Task<AlumnoDTO?> ObtenerPorIdDtoAsync(int id);
    Task<alumno> ObtenerPorIdNormalAsync(int id);
    Task<alumno?> ObtenerPorNombreUsuarioAsync(string nombreUsuario);
    Task<alumno?> ObtenerPorNombreUsuarioEIdAsync(string nombreUsuario, int id);
    Task<alumno?> ObtenerPorCorreoAsync(string correo);
    Task AgregarAlumnoAsync(alumno alumno);
    Task<AlumnoDTO?> ObtenerPorNombreUsuarioOCorreoAsync(string nombreCompletoOCorreo);
}