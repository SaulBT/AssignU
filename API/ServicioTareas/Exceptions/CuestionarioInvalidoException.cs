namespace ServicioTareas.Exceptions;

public class CuestionarioInvalidoException : Exception
{
    public CuestionarioInvalidoException(string mensaje) : base(mensaje) { }
}