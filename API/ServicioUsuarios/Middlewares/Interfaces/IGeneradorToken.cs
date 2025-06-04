namespace ServicioUsuarios.Middlewares.Interfaces;

public interface IGeneradorToken
{
    string GenerarToken(string nombreUsuario, string tipo, int idUsuario);
}