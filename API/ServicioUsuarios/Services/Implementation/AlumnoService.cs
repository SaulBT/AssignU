using ServicioUsuarios.Entities;
using ServicioUsuarios.DAOs;
using ServicioUsuarios.Services.Interfaces;
using ServicioUsuarios.Exceptions;
using ServicioUsuarios.DTOs;

namespace ServicioUsuarios.Services.Implementation;

public class AlumnoService : IAlumnoService
{
    private readonly AlumnoDAO _alumnoDAO;

    public AlumnoService(AlumnoDAO alumnoDAO)
    {
        _alumnoDAO = alumnoDAO;
    }

    public async Task<AlumnoDTO?> obtenerPorIdAsync(int id)
    {
        verificarIdValida(id);
        var alumno = await _alumnoDAO.obtenerPorIdDtoAsync(id);
        verificarExistenciaAlumno(alumno);
        return alumno;
    }

    public async Task<alumno> registrarAsync(RegistrarAlumnoDTO alumnoDto)
    {
        verificarAlumnoNuloRegistro(alumnoDto);
        verificarAlumnoNombreRegistroAsync(alumnoDto.nombreUsuario);
        verificarAlumnoCorreoAsync(alumnoDto.correoElectronico);

        var alumnoNuevo = new alumno
        {
            nombreCompleto = alumnoDto.nombreCompleto,
            nombreUsuario = alumnoDto.nombreUsuario,
            contrasenia = alumnoDto.contrasenia,
            correo = alumnoDto.correoElectronico,
            idGradoEstudios = alumnoDto.idGradoEstudios
        };

        await _alumnoDAO.agregarAlumnoAsync(alumnoNuevo);

        return alumnoNuevo;
    }

    public async Task actualizarAsync(int id, ActualizarAlumnoDTO alumnoDto)
    {
        verificarIgualdadId(id, alumnoDto.idAlumno);
        verificarAlumnoNuloActualizacion(alumnoDto);
        verificarAlumnoNombreActualizacionAsync(alumnoDto.nombreUsuario, id);

        var alumno = await _alumnoDAO.obtenerPorIdNormalAsync(id);
        alumno.nombreCompleto = alumnoDto.nombreCompleto;
        alumno.nombreUsuario = alumnoDto.nombreUsuario;
        alumno.idGradoEstudios = alumnoDto.idGradoEstudios;
        
        await _alumnoDAO.actualizarAsync(alumno);
    }

    public async Task eliminarAsync(int id)
    {
        verificarIdValida(id);
        await _alumnoDAO.eliminarAsync(id);
    }

    private void verificarIdValida(int id)
    {
        if (id <= 0)
        {
            throw new IdInvalidaException("El ID del alumno debe ser mayor que cero.");
        }
    }

    private void verificarExistenciaAlumno(AlumnoDTO alumno)
    {
        if (alumno == null)
        {
            throw new RecursoNoEncontradoException("El alumno no existe.");
        }
    }

    private void verificarAlumnoNuloRegistro(RegistrarAlumnoDTO alumnoDto)
    {
        if (alumnoDto == null)
        {
            throw new ArgumentNullException(nameof(alumnoDto), "No se envió ningún alumno para registrar.");
        }
    }

    private void verificarAlumnoNuloActualizacion(ActualizarAlumnoDTO alumnoDto)
    {
        if (alumnoDto == null)
        {
            throw new ArgumentNullException(nameof(alumnoDto), "No se envió ningún alumno para actualizar.");
        }
    }

    private async void verificarAlumnoNombreRegistroAsync(string nombreUsuario)
    {
        var alumnoExistente = await _alumnoDAO.obtenerPorNombreUsuarioAsync(nombreUsuario);
        if (alumnoExistente != null)
        {
            throw new RecursoYaExistenteException($"El nombre de usuario '{nombreUsuario}' ya está en uso por otro alumno.");
        }
    }

    private async void verificarAlumnoNombreActualizacionAsync(string nombreUsuario, int id)
    {
        var alumnoExistente = await _alumnoDAO.obtenerPorNombreUsuarioEIdAsync(nombreUsuario, id);
        if (alumnoExistente != null)
        {
            throw new RecursoYaExistenteException($"El nombre de usuario '{nombreUsuario}' ya está en uso por otro alumno.");
        }
    }

    private void verificarIgualdadId(int id, int idAlumno)
    {
        if (id != idAlumno)
        {
            throw new DiscordanciaDeIdException("El ID del alumno en la URL no coincide con el ID del alumno en el cuerpo de la solicitud.");
        }
    }
    
    private async void verificarAlumnoCorreoAsync(string correo)
    {
        var alumnoExistente = await _alumnoDAO.obtenerPorCorreoAsync(correo);
        if (alumnoExistente != null)
        {
            throw new RecursoYaExistenteException($"El correo '{correo}' ya está en uso por otro alumno.");
        }
    }
}