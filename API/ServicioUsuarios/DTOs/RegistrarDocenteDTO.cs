namespace ServicioUsuarios.DTOs;

public class RegistrarDocenteDTO
{
    public string nombreCompleto { get; set; } = string.Empty;
    public string nombreUsuario { get; set; } = string.Empty;
    public string contrasenia { get; set; } = string.Empty;
    public string correoElectronico { get; set; } = string.Empty;
    public int idGradoProfesional { get; set; }
}