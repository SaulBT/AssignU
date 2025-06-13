using ServicioUsuarios.DTOs;
using ServicioUsuarios.Entities;

namespace ServicioUsuarios.Services.Interfaces;

public interface IServicioAlumno
{
    Task<alumno> RegistrarAsync(RegistrarAlumnoDTO alumnoDto);
    Task ActualizarAsync(HttpContext context, ActualizarAlumnoDTO alumnoDto);
    Task EliminarAsync(HttpContext context);
    Task<AlumnoDTO?> ObtenerPorIdAsync(int id);
    Task CambiarContraseniaAsync(CambiarContraseniaDTO cambiarContraseniaDto, HttpContext context);
    Task<RespuestaRPCDTO> ObtenerListaAlumnosAsync(List<int> idAlumnos);
}