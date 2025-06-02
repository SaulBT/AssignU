using ServicioUsuarios.DTOs;
using ServicioUsuarios.Entities;
using ServicioUsuarios.Middlewares;

public class LoginService : ILoginService
{
    private readonly usuarios_bd_assignuContext _context;
    private readonly GeneradorToken _generadorToken;

    public LoginService(usuarios_bd_assignuContext context, GeneradorToken generadorToken)
    {
        _context = context;
        _generadorToken = generadorToken;
    }

    public async string IniciarSesion(IniciarSesionDTO usuarioDto)
    {
        //TODO
    }
}