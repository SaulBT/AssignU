namespace ServicioUsuarios.Exceptions;

public class CampoObligatorioException : ArgumentException
{
    public CampoObligatorioException(string mensaje) : base(mensaje) { }
}