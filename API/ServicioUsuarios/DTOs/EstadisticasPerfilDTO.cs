namespace ServicioUsuarios.DTOs;

public class EstadisticasPerfilDTO
{
    public int IdAlumno { get; set; } = 0;
    public string NombreUsuario { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public int idGradoEstudios { get; set; } = 0;
    public List<ClaseEstadisticaPerfilDTO> Clases = new List<ClaseEstadisticaPerfilDTO>();
}

public class ClaseEstadisticaPerfilDTO
{
    public int IdClase { get; set; } = 0;
    public string Nombre { get; set; } = string.Empty;
    public DateTime UltimaConexion { get; set; } = new DateTime();
    public List<TareaEstadisticaPerfilDTO> Tareas { get; set; } = new List<TareaEstadisticaPerfilDTO>();
}

public class TareaEstadisticaPerfilDTO
{
    public int IdTarea { get; set; } = 0;
    public string Nombre { get; set; } = string.Empty;
    public int Calificacion { get; set; } = 0;
}