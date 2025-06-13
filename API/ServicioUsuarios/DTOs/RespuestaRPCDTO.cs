namespace ServicioUsuarios.DTOs;

public class RespuestaRPCDTO
{
    public bool Success { get; set; }
    public List<AlumnoRespuestaRPCEstadisticasClaseDTO> Alumnos { get; set; }
    public List<ClaseEstadisticaPerfilDTO> Clases = new List<ClaseEstadisticaPerfilDTO>();
    public ErrorDTO Error { get; set; }
}

public class AlumnoRespuestaRPCEstadisticasClaseDTO
{
    public int IdAlumno { get; set; } = 0;
    public string NombreCompleto { get; set; } = string.Empty;
}

public class ClaseRespuestaRPCEstadisticasPerfilDTO
{
    public int IdClase { get; set; } = 0;
    public string Nombre { get; set; } = string.Empty;
    public DateTime UltimaConexion { get; set; } = new DateTime();
}

public class ErrorDTO
{
    public string Tipo { get; set; } = string.Empty;
    public string Mensaje { get; set; } = string.Empty;
}