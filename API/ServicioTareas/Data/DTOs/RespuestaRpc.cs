using ServicioTareas.Data.DTOs.RPC;

namespace ServicioTareas.Data.DTOs;

public class RespuestaCuestionarioDTO
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public ErrorDTO Error { get; set; } = new ErrorDTO();
    public RespuestaDTO Respuesta { get; set; } = new RespuestaDTO();
}

public class RespuestaDTO
{
    public int IdAlumno { get; set; } = 0;
    public int IdTarea { get; set; } = 0;
    public string Estado { get; set; } = string.Empty;
    public float Calificacion { get; set; } = 0;
    public List<PreguntaRespuestaDTO> Preguntas { get; set; } = [];
}

public class PreguntaRespuestaDTO
{
    public string Texto { get; set; } = string.Empty;
    public OpcionRespuestaDTO Opcion { get; set; }
    public bool Correcta { get; set; }
}

public class OpcionRespuestaDTO
{
    public string Texto { get; set; } = string.Empty;
}