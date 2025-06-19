namespace ServicioUsuarios.Data.DTOs.RPC;

public class SolicitudRPCDTO
{
    public string Accion { get; set; } = string.Empty;
    public List<int> IdAlumnos { get; set; } = new List<int>();
    public int IdAlumno { get; set; } = 0;
}