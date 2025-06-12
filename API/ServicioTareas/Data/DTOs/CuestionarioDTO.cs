using Microsoft.Net.Http.Headers;

namespace ServicioTareas.Data.DTOs;

public class CuestionarioDTO
{
    public List<PreguntaDTO> preguntas { get; set; } = new();
}