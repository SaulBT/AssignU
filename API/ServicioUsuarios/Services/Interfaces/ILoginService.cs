namespace ServicioUsuarios.DTOs;

public interface ILoginService
{
    public string IniciarSesion(IniciarSesionDTO usuarioDto);
}