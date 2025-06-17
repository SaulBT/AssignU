
using ServicioUsuarios.Data.DTOs;
using ServicioUsuarios.Middlewares;
using ServicioUsuarios.Services.Interfaces;
using ServicioUsuarios.Validations;

namespace ServicioUsuarios.Services.Implementation;

public class ServicioLogin : IServicioLogin
{
    private readonly GeneradorToken _generadorToken;
    private readonly LoginValidaciones _validaciones;
    private readonly ILogger _logger;

    public ServicioLogin(GeneradorToken generadorToken, LoginValidaciones validaciones, ILogger logger)
    {
        _generadorToken = generadorToken;
        _validaciones = validaciones;
        _logger = logger;
    }

    public async Task<Object> IniciarSesion(IniciarSesionDTO usuarioDto)
    {
        _logger.LogInformation("Iniciando la sesión");
        _validaciones.VerificarParametrosUsuarioDto(usuarioDto);
        var idUsuario = 0;
        var nombreUsuario = "";
        if (usuarioDto.TipoUsuario == "alumno")
        {
            var usuario = await _validaciones.verificarCredencialesAlumnoAsync(usuarioDto);
            idUsuario = usuario.IdAlumno;
            nombreUsuario = usuario.NombreUsuario;
        }
        else if (usuarioDto.TipoUsuario == "docente")
        {
            var usuario = await _validaciones.verificarCredencialesDocenteAsync(usuarioDto);
            idUsuario = usuario.IdDocente;
            nombreUsuario = usuario.NombreUsuario;
        }

        string token = _generadorToken.GenerarToken(nombreUsuario, usuarioDto.TipoUsuario, idUsuario);

        _logger.LogInformation($"Se inició sesión al usuario con la id {idUsuario}");
        return new
        {
            IdUsuario = idUsuario,
            Token = token
        };
    }

    
}