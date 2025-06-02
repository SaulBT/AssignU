namespace ServicioUsuarios.Exceptions;

public class IdInvalidaException : Exception
{
    public IdInvalidaException(string mensaje) : base(mensaje) { }
}