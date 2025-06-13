namespace ServicioTareas.Data.DTOs;

public class RespuestaRPCDTO
{
    public bool Success { get; set; }
    public List<AlumnoRespuestaRPCEstadisticasClaseDTO> Alumnos { get; set; }
    public List<TareaEstadisticasClaseDTO> Tareas { get; set; }
    public List<RespuestasEstadisticaClaseDTO> Respuestas { get; set; }
    public ErrorDTO Error { get; set; }
}

public class AlumnoRespuestaRPCEstadisticasClaseDTO
{
    public int IdAlumno { get; set; } = 0;
    public string NombreCompleto { get; set; } = string.Empty;
}

public class RespuestasEstadisticaClaseDTO
{
    public int IdAlumno { get; set; } = 0;
    public int IdTarea { get; set; } = 0;
    public int Calificacion { get; set; } = 0;
}

public class TareaEstadisticasClaseDTO
{
    public int IdTarea { get; set; } = 0;
    public string Nombre { get; set; } = string.Empty;
}