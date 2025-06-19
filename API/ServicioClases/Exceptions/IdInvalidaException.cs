namespace ServicioClases.Exceptions;

public class IdInvalidaException : ArgumentException
{
    public IdInvalidaException(string mensaje) : base(mensaje) { }
}