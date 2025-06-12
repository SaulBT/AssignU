namespace ServicioTareas.Data.DTOs;

public class PreguntaDTO
{
    public string texto { get; set; } = string.Empty;
    public string tipo { get; set; } = string.Empty; // opcion_multiple / verdadero_falso
    public List<OpcionDTO> opciones { get; set; } = new();
}