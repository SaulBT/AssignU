using Microsoft.Net.Http.Headers;

namespace ServicioTareas.Data.DTOs;

public class CuestionarioDTO
{
    public int IdTarea { get; set; } = 0;
    public List<PreguntaDTO> Preguntas { get; set; } = new();
}