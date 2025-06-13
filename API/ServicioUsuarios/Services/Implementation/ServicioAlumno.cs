using ServicioUsuarios.Entities;
using ServicioUsuarios.Services.Interfaces;
using ServicioUsuarios.Exceptions;
using ServicioUsuarios.DTOs;
using ServicioUsuarios.DAOs.Interfaces;

namespace ServicioUsuarios.Services.Implementation;

public class ServicioAlumno : IServicioAlumno
{
    private readonly IAlumnoDAO _alumnoDAO;

    public ServicioAlumno(IAlumnoDAO alumnoDAO)
    {
        _alumnoDAO = alumnoDAO;
    }

    public async Task<AlumnoDTO?> ObtenerPorIdAsync(int id)
    {
        verificarIdValida(id);
        var alumno = await _alumnoDAO.ObtenerPorIdDtoAsync(id);
        verificarExistenciaAlumno(alumno);
        return alumno;
    }

    public async Task<alumno> RegistrarAsync(RegistrarAlumnoDTO alumnoDto)
    {
        verificarParametrosAlumnoRegistro(alumnoDto);
        await verificarAlumnoNombreRegistroAsync(alumnoDto.nombreUsuario);
        await verificarAlumnoCorreoAsync(alumnoDto.correoElectronico);

        var alumnoNuevo = new alumno
        {
            nombreCompleto = alumnoDto.nombreCompleto,
            nombreUsuario = alumnoDto.nombreUsuario,
            contrasenia = alumnoDto.contrasenia,
            correo = alumnoDto.correoElectronico,
            idGradoEstudios = alumnoDto.idGradoEstudios
        };

        await _alumnoDAO.AgregarAlumnoAsync(alumnoNuevo);

        return alumnoNuevo;
    }

    public async Task ActualizarAsync(HttpContext context, ActualizarAlumnoDTO alumnoDto)
    {
        verificarAutorizacion(context);
        var idAlumno = int.Parse(context.User.FindFirst("idUsuario")!.Value);

        verificarIgualdadId(idAlumno, alumnoDto.idAlumno);
        verificarParametrosAlumnoActualizacion(alumnoDto);
        await verificarAlumnoNombreActualizacionAsync(alumnoDto.nombreUsuario, idAlumno);

        var alumno = await _alumnoDAO.ObtenerPorIdNormalAsync(idAlumno);
        alumno.nombreCompleto = alumnoDto.nombreCompleto;
        alumno.nombreUsuario = alumnoDto.nombreUsuario;
        alumno.idGradoEstudios = alumnoDto.idGradoEstudios;

        await _alumnoDAO.ActualizarAsync(alumno);
    }

    public async Task EliminarAsync(HttpContext context)
    {
        verificarAutorizacion(context);
        var idAlumno = int.Parse(context.User.FindFirst("idUsuario")!.Value);
        verificarIdValida(idAlumno);
        var alumno = await _alumnoDAO.ObtenerPorIdDtoAsync(idAlumno);
        verificarExistenciaAlumno(alumno);
        await _alumnoDAO.EliminarAsync(idAlumno);
    }

    public async Task CambiarContraseniaAsync(CambiarContraseniaDTO cambiarContraseniaDto, HttpContext context)
    {
        verificarParametrosCambiarContrasenia(cambiarContraseniaDto);
        verificarAutorizacion(context);

        var idAlumno = int.Parse(context.User.FindFirst("idUsuario")!.Value);
        verificarIdValida(idAlumno);
        var alumno = await ObtenerAlumnoPorIdAsync(idAlumno);
        verificarContraseniaActual(alumno, cambiarContraseniaDto.contraseniaActual);

        alumno.contrasenia = cambiarContraseniaDto.contraseniaNueva;
        await _alumnoDAO.ActualizarAsync(alumno);
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

    private void verificarParametrosAlumnoRegistro(RegistrarAlumnoDTO alumnoDto)
    {
        if (string.IsNullOrEmpty(alumnoDto.nombreCompleto) ||
            string.IsNullOrEmpty(alumnoDto.nombreUsuario) ||
            string.IsNullOrEmpty(alumnoDto.contrasenia) ||
            string.IsNullOrEmpty(alumnoDto.correoElectronico) ||
            alumnoDto.idGradoEstudios <= 0)
        {
            throw new ArgumentException("Los parámetros de registro del alumno son inválidos.");
        }
    }

    private void verificarParametrosAlumnoActualizacion(ActualizarAlumnoDTO alumnoDto)
    {
        if (string.IsNullOrEmpty(alumnoDto.nombreCompleto) ||
            string.IsNullOrEmpty(alumnoDto.nombreUsuario) ||
            alumnoDto.idGradoEstudios <= 0)
        {
            throw new ArgumentException("Los parámetros de actualización del alumno son inválidos.");
        }
    }

    private void verificarParametrosCambiarContrasenia(CambiarContraseniaDTO cambiarContraseniaDto)
    {
        if (string.IsNullOrEmpty(cambiarContraseniaDto.contraseniaNueva) ||
            string.IsNullOrEmpty(cambiarContraseniaDto.contraseniaActual))
        {
            throw new ArgumentException("Los parámetros de cambio de contraseña son inválidos.");
        }
    }

    private async Task verificarAlumnoNombreRegistroAsync(string nombreUsuario)
    {
        var alumnoExistente = await _alumnoDAO.ObtenerPorNombreUsuarioAsync(nombreUsuario);
        if (alumnoExistente != null)
        {
            throw new RecursoYaExistenteException($"El nombre de usuario '{nombreUsuario}' ya está en uso por otro alumno.");
        }
    }

    private async Task verificarAlumnoNombreActualizacionAsync(string nombreUsuario, int id)
    {
        var alumnoExistente = await _alumnoDAO.ObtenerPorNombreUsuarioEIdAsync(nombreUsuario, id);
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

    private async Task verificarAlumnoCorreoAsync(string correo)
    {
        var alumnoExistente = await _alumnoDAO.ObtenerPorCorreoAsync(correo);
        if (alumnoExistente != null)
        {
            throw new RecursoYaExistenteException($"El correo '{correo}' ya está en uso por otro alumno.");
        }
    }

    private void verificarAutorizacion(HttpContext context)
    {
        if (!context.User.Identity?.IsAuthenticated ?? true)
        {
            throw new UnauthorizedAccessException("El usuario no está autenticado.");
        }
    }

    private void verificarContraseniaActual(alumno alumno, string contraseniaActual)
    {
        if (alumno.contrasenia != contraseniaActual)
        {
            throw new ContraseniaDiferenteException("La contraseña actual es incorrecta.");
        }
    }

    private async Task<alumno> ObtenerAlumnoPorIdAsync(int id)
    {
        var alumno = await _alumnoDAO.ObtenerPorIdNormalAsync(id);
        if (alumno == null)
        {
            throw new RecursoNoEncontradoException($"El alumno con ID {id} no existe.");
        }
        return alumno;
    }
}