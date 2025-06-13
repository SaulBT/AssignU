namespace ServicioTareas.Data.DTOs;

public class RespuestaCuestionarioDTO
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public ErrorDTO Error { get; set; }
    public RespuestaDTO Respuesta { get; set; }
}

public class ErrorDTO
{
    public string Tipo { get; set; }
    public string Mensaje { get; set; }
}

public class RespuestaDTO
{
    public int idAlumno { get; set; }
    public int idTarea { get; set; }
    public List<PreguntaRespuestaDTO> Preguntas { get; set; }
}

public class PreguntaRespuestaDTO
{
    public string Texto { get; set; }
    public OpcionRespuestaDTO Opcion { get; set; }
    public bool Correcta { get; set; }
}

public class OpcionRespuestaDTO
{
    public string Texto { get; set; }
}