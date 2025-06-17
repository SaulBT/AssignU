namespace ServicioClases.Exceptions;

public class CampoObligatorioException : ArgumentException
{
    public CampoObligatorioException(string mensaje) : base(mensaje) { }
}