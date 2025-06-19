namespace ServicioUsuarios.Data.DTOs.Docente;
public class DocenteDTO
{
    public int IdDocente { get; set; } = 0;
    public string NombreCompleto { get; set; } = string.Empty;
    public string NombreUsuario { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public int IdGradoProfesional { get; set; } = 0;
}