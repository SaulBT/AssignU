namespace ServicioTareas.Data.DTOs;

public class EditarTareaDTO
{
    public int idTarea { get; set; } = 0;
    public string nombre { get; set; } = null!;
    public int idArchivo { get; set; } = 0!;
    public DateTime fechaLimite { get; set; }
    public CuestionarioDTO cuestionario { get; set; } = new();
}