namespace ServicioTareas.Exceptions;

public class ValorInvalidoException : Exception
{
    public ValorInvalidoException(string mensaje) : base(mensaje) { }
}