namespace ServicioUsuarios.DTOs;

public class RegistrarAlumnoDTO
{
    public string nombreCompleto { get; set; } = string.Empty;
    public string nombreUsuario { get; set; } = string.Empty;
    public string contrasenia { get; set; } = string.Empty;
    public string correoElectronico { get; set; } = string.Empty;
    public int idGradoEstudios { get; set; }
}