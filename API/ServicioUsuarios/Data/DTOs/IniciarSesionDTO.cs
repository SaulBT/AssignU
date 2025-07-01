namespace ServicioUsuarios.Data.DTOs;

public class IniciarSesionDTO
{
    public string TipoUsuario { get; set; } = string.Empty; // "alumno" o "docente"
    public string NombreUsuarioOCorreo { get; set; } = string.Empty;
    public string Contrasenia { get; set; } = string.Empty;
}