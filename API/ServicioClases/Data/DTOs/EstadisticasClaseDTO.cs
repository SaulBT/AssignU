namespace ServicioClases.Data.DTOs;

public class EstadisticasClaseDTO
{
    public int IdClase { get; set; } = 0;
    public List<AlumnoEstadisticasClaseDTO> Alumnos { get; set; }
    public List<TareaEstadisticasClaseDTO> Tareas { get; set; }
}

public class AlumnoEstadisticasClaseDTO
{
    public int IdAlumno { get; set; } = 0;
    public string NombreCompleto { get; set; } = string.Empty;
    public List<RespuestasEstadisticaClaseDTO> Respuestas { get; set; }
    public DateTime UltimaConexion { get; set; }
}

public class RespuestasEstadisticaClaseDTO
{
    public int IdAlumno { get; set; } = 0;
    public int IdTarea { get; set; } = 0;
    public float Calificacion { get; set; } = 0;
}

public class TareaEstadisticasClaseDTO
{
    public int IdTarea { get; set; } = 0;
    public string Nombre { get; set; } = string.Empty;
}