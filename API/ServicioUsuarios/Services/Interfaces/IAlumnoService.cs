using ServicioUsuarios.DTOs;
using ServicioUsuarios.Entities;

namespace ServicioUsuarios.Services.Interfaces;

public interface IAlumnoService
{
    Task<alumno> registrarAsync(RegistrarAlumnoDTO alumnoDto);
    Task actualizarAsync(HttpContext context, ActualizarAlumnoDTO alumnoDto);
    Task eliminarAsync(HttpContext context);
    Task<AlumnoDTO?> obtenerPorIdAsync(int id);
    Task cambiarContraseniaAsync(CambiarContraseniaDTO cambiarContraseniaDto, HttpContext context);
}