using Microsoft.Net.Http.Headers;

namespace ServicioTareas.Data.DTOs;

public class CuestionarioDTO
{
    public int idTarea = 0;
    public List<PreguntaDTO> preguntas = new();
}