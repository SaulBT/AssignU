namespace ServicioUsuarios.Data.DTOs.Docente;
public class ActualizarDocenteDTO
{
    public string NombreUsuario { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public int IdGradoProfesional { get; set; } = 0;
}