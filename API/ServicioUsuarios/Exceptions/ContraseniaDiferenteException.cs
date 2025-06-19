namespace ServicioUsuarios.Exceptions;

public class ContraseniaDiferenteException : Exception
{
    public ContraseniaDiferenteException(string mensaje) : base(mensaje) { }
}