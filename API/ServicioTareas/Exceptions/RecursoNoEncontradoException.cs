namespace ServicioTareas.Exceptions;

public class RecursoNoEncontradoException : Exception
{
    public RecursoNoEncontradoException(string mensaje) : base(mensaje) { }
}