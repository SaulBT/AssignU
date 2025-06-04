using ServicioUsuarios.DTOs;

namespace ServicioUsuarios.Services.Interfaces;

public interface ILoginService
{
    public Task<Object> IniciarSesion(IniciarSesionDTO usuarioDto);
}