namespace ServicioTareas.Data.DTOs.RPC;

public class SolicitudRPCDTO
{
    public string Accion { get; set; } = string.Empty;
    public List<int> IdAlumnos { get; set; }
    public int IdClase { get; set; } = 0;
}