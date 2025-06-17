namespace ServicioUsuarios.Data.DTOs.RPC;

public class RespuestaRPCDTO
{
    public bool Success { get; set; }
    public List<AlumnoEstadisticasDTO> Alumnos { get; set; }
    public List<ClaseEstadisticaPerfilDTO> Clases { get; set; } = new List<ClaseEstadisticaPerfilDTO>();
    public ErrorDTO Error { get; set; }
}

public class ClaseRPCDTO
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