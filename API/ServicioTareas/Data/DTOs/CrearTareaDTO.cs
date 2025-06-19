using ServicioTareas.Models;

namespace ServicioTareas.Data.DTOs;

public class CrearTareaDTO
{
    public int IdClase { get; set; } = 0;
    public string Nombre { get; set; } = null!;
    public DateTime FechaLimite { get; set; } = default(DateTime);
    public CuestionarioDTO Cuestionario { get; set; } = new();
}