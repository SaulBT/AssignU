namespace ServicioTareas.Exceptions;

public class PreguntaInvalidaException : Exception
{
    public PreguntaInvalidaException(string mensaje) : base(mensaje) { }
}