using ServicioUsuarios.Data.DTOs;
using ServicioUsuarios.Data.DTOs.Alumno;
using ServicioUsuarios.Data.DTOs.RPC;

namespace ServicioUsuarios.Services.Interfaces;

public interface IServicioAlumno
{
    Task<AlumnoDTO> RegistrarAsync(RegistrarAlumnoDTO alumnoDto);
    Task<AlumnoDTO> ActualizarAsync(HttpContext context, int idAlumno, ActualizarAlumnoDTO alumnoDto);
    Task EliminarAsync(HttpContext context, int idAlumno);
    Task<AlumnoDTO?> ObtenerAlumnoPorIdAsync(int idAlumno);
    Task CambiarContraseniaAsync(CambiarContraseniaDTO cambiarContraseniaDto, int idAlumno, HttpContext context);
    Task<RespuestaRPCDTO> ObtenerListaAlumnosAsync(List<int> idAlumnos);
    Task<EstadisticasPerfilDTO> ObtenerEstadisticasPerfilAlumnoAsync(HttpContext httpContext, int idAlumno);
}