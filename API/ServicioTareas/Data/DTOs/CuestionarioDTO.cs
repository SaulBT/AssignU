using Microsoft.Net.Http.Headers;

namespace ServicioTareas.Data.DTOs;

public class CuestionarioDTO
{
    public List<PreguntaDTO> Preguntas { get; set; } = new();
}