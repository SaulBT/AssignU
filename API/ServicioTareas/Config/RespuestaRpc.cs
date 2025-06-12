namespace ServicioTareas.Config;

public class RespuestaCuestionario
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public Error Error { get; set; }
}

public class Error
{
    public string Tipo { get; set; }
    public string Mensaje { get; set; }
}