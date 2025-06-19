namespace ServicioTareas.Exceptions;

public class IdInvalidaException : ArgumentException
{
    public IdInvalidaException(string mensaje) : base(mensaje) { }
}