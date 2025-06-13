namespace ServicioClases.Data.DTOs;

public class RespuestaRPCDTO
{
    public bool Success { get; set; }
    public List<AlumnoRespuestaRPCEstadisticasClaseDTO> Alumnos { get; set; }
    public List<TareaEstadisticasClaseDTO> Tareas { get; set; }
    public List<RespuestasEstadisticaClaseDTO> Respuestas { get; set; }
    public List<ClaseEstadisticaPerfilDTO> Clases { get; set; } = new List<ClaseEstadisticaPerfilDTO>();
    public ErrorDTO Error { get; set; }
}

public class AlumnoRespuestaRPCEstadisticasClaseDTO
{
    public int IdAlumno { get; set; } = 0;
    public string NombreCompleto { get; set; } = string.Empty;
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

public class ErrorDTO
{
    public string Tipo { get; set; } = string.Empty;
    public string Mensaje { get; set; } = string.Empty;
}