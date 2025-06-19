namespace ServicioUsuarios.Data.DTOs.Docente;

public class RegistrarDocenteDTO
{
    public string NombreCompleto { get; set; } = string.Empty;
    public string NombreUsuario { get; set; } = string.Empty;
    public string Contrasenia { get; set; } = string.Empty;
    public string CorreoElectronico { get; set; } = string.Empty;
    public int IdGradoProfesional { get; set; } = 0;
}