using ServicioTareas.Models;

namespace ServicioTareas.Data.DTOs;

public class CrearTareaDTO
{
    public int idClase { get; set; } = 0;
    public string nombre { get; set; } = null!;
    public int idArchivo { get; set; } = 0!;
    public DateTime fechaLimite { get; set; }
    public CuestionarioDTO cuestionario { get; set; } = new();
}