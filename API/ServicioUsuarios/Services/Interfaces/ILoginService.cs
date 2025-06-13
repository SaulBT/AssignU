using ServicioUsuarios.DTOs;

namespace ServicioUsuarios.Services.Interfaces;

public interface IServicioLogin
{
    public Task<Object> IniciarSesion(IniciarSesionDTO usuarioDto);
}