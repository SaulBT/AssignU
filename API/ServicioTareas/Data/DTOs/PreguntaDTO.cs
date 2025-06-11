namespace ServicioTareas.Data.DTOs;

public class PreguntaDTO
{
    private string texto { get; set; } = string.Empty;
    private string tipo { get; set; } = string.Empty; // opcion_multiple / verdadero_falso
    public List<OpcionDTO> opciones { get; set; } = new();
}