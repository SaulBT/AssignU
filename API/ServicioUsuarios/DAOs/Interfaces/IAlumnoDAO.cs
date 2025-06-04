using ServicioUsuarios.DTOs;
using ServicioUsuarios.Entities;

namespace ServicioUsuarios.DAOs.Interfaces;

public interface IAlumnoDAO
{
    Task actualizarAsync(alumno alumno);
    Task eliminarAsync(int id);
    Task<AlumnoDTO?> obtenerPorIdDtoAsync(int id);
    Task<alumno> obtenerPorIdNormalAsync(int id);
    Task<alumno?> obtenerPorNombreUsuarioAsync(string nombreUsuario);
    Task<alumno?> obtenerPorNombreUsuarioEIdAsync(string nombreUsuario, int id);
    Task<alumno?> obtenerPorCorreoAsync(string correo);
    Task agregarAlumnoAsync(alumno alumno);
    Task<AlumnoDTO?> obtenerPorNombreUsuarioOCorreoAsync(string nombreCompletoOCorreo);
}