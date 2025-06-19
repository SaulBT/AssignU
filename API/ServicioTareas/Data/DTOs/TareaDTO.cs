namespace ServicioTareas.Data.DTOs;

public class TareaDTO
{
    public int IdTarea { get; set; } = 0;
    public int IdClase { get; set; } = 0;
    public string Nombre { get; set; } = string.Empty;
    public DateTime FechaLimite { get; set; } = default(DateTime);
}