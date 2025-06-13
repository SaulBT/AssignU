namespace ServicioTareas.Data.DTOs;

public class EditarTareaDTO
{
    public int IdTarea { get; set; } = 0;
    public string Nombre { get; set; } = null!;
    public int IdArchivo { get; set; } = 0!;
    public DateTime FechaLimite { get; set; }
    public CuestionarioDTO Cuestionario { get; set; } = new();
}