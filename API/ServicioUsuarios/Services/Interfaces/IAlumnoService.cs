using ServicioUsuarios.DTOs;
using ServicioUsuarios.Entities;

namespace ServicioUsuarios.Services.Interfaces;

public interface IAlumnoService
{
    Task<alumno> registrarAsync(RegistrarAlumnoDTO alumnoDto);
    Task actualizarAsync(int id, ActualizarAlumnoDTO alumnoDto);
    Task eliminarAsync(int id);
    Task<AlumnoDTO?> obtenerPorIdAsync(int id);
}