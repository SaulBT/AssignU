namespace ServicioUsuarios.Data.DTOs.Alumno;
public class ActualizarAlumnoDTO
{
    public int IdAlumno { get; set; } = 0;
    public string NombreCompleto { get; set; } = string.Empty;
    public string NombreUsuario { get; set; } = string.Empty;
    public int IdGradoEstudios { get; set; } = 0;
}