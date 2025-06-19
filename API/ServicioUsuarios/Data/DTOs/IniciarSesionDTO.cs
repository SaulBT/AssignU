namespace ServicioUsuarios.Data.DTOs;

public class IniciarSesionDTO
{
    public string TipoUsuario { get; set; } // "alumno" o "docente"
    public string NombreUsuarioOCorreo { get; set; }
    public string Contrasena { get; set; }
}