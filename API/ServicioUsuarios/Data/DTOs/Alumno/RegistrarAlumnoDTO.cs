namespace ServicioUsuarios.Data.DTOs.Alumno;

public class RegistrarAlumnoDTO
{
    public string NombreCompleto { get; set; } = string.Empty;
    public string NombreUsuario { get; set; } = string.Empty;
    public string Contrasenia { get; set; } = string.Empty;
    public string CorreoElectronico { get; set; } = string.Empty;
    public int IdGradoEstudios { get; set; } = 0;
}