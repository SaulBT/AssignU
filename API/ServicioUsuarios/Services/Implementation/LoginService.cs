using ServicioUsuarios.DAOs.Interfaces;
using ServicioUsuarios.DTOs;
using ServicioUsuarios.Entities;
using ServicioUsuarios.Middlewares;
using ServicioUsuarios.Services.Interfaces;

namespace ServicioUsuarios.Services.Implementation;

public class LoginService : IServicioLogin
{
    private readonly usuarios_bd_assignuContext _context;
    private IAlumnoDAO _alumnoDAO;
    private IDocenteDAO _docenteDAO;
    private readonly GeneradorToken _generadorToken;

    public LoginService(usuarios_bd_assignuContext context, IAlumnoDAO alumnoDAO, IDocenteDAO docenteDAO, GeneradorToken generadorToken)
    {
        _context = context;
        _alumnoDAO = alumnoDAO;
        _docenteDAO = docenteDAO;
        _generadorToken = generadorToken;
    }

    public async Task<Object> IniciarSesion(IniciarSesionDTO usuarioDto)
    {
        verificarParametrosUsuarioDto(usuarioDto);
        var idUsuario = 0;
        var nombreUsuario = "";
        if (usuarioDto.tipoUsuario == "alumno")
        {
            var usuario = await verificarCredencialesAlumnoAsync(usuarioDto);
            idUsuario = usuario.idAlumno;
            nombreUsuario = usuario.nombreUsuario;
        }
        else if (usuarioDto.tipoUsuario == "docente")
        {
            var usuario = await verificarCredencialesDocenteAsync(usuarioDto);
            idUsuario = usuario.idDocente;
            nombreUsuario = usuario.nombreUsuario;
        }

        string token = _generadorToken.GenerarToken(nombreUsuario, usuarioDto.tipoUsuario, idUsuario);

        return new
        {
            usuario = usuarioDto,
            token = token
        };
    }

    private void verificarParametrosUsuarioDto(IniciarSesionDTO usuarioDto)
    {
        if (string.IsNullOrEmpty(usuarioDto.tipoUsuario) ||
            string.IsNullOrEmpty(usuarioDto.nombreUsuarioOCorreo) ||
            string.IsNullOrEmpty(usuarioDto.contrasena))
        {
            throw new ArgumentNullException("Los parámetros del usuario no pueden ser nulos.");
        }
    }

    private async Task<AlumnoDTO> verificarCredencialesAlumnoAsync(IniciarSesionDTO usuarioDto)
    {
        var alumnoDto = await _alumnoDAO.ObtenerPorNombreUsuarioOCorreoAsync(usuarioDto.nombreUsuarioOCorreo);
        if (alumnoDto == null)
        {
            throw new UnauthorizedAccessException("Credenciales incorrectas para el alumno.");
        }

        return alumnoDto;
    }

    private async Task<DocenteDTO> verificarCredencialesDocenteAsync(IniciarSesionDTO usuarioDto)
    {
        var docenteDto = await _docenteDAO.ObtenerPorNombreUsuarioOCorreoAsync(usuarioDto.nombreUsuarioOCorreo);
        if (docenteDto == null)
        {
            throw new UnauthorizedAccessException("Credenciales incorrectas para el docente.");
        }

        return docenteDto;
    }
}