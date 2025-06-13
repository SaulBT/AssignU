namespace ServicioTareas.Data.DTOs;

public class PreguntaDTO
{
    public string Texto { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty; // opcion_multiple / verdadero_falso
    public List<OpcionDTO> Opciones { get; set; } = new();
}