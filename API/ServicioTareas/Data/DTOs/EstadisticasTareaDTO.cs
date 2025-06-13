namespace ServicioTareas.Data.DTOs;

public class EstadisticasTareaDTO
{
    public List<string> TextoPreguntas { get; set; } = new List<string>();
    public List<RespuestaDTO> Respuestas { get; set; } = new List<RespuestaDTO>();
    public List<AlumnoEstadisticaTareaDTO> Alumnos { get; set; } = new List<AlumnoEstadisticaTareaDTO>();
}

public class AlumnoEstadisticaTareaDTO
{
    public int IdAlumno { get; set; } = 0;
    public string NombreCompleto { get; set; } = string.Empty;
}