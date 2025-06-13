namespace ServicioUsuarios.DTOs;

public class SolicitudRPCDTO
{
    public string Accion { get; set; } = string.Empty;
    public List<int> IdAlumnos { get; set; }
}