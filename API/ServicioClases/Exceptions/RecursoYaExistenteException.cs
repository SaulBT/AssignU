namespace ServicioClases.Exceptions;

public class RecursoYaExistenteException : Exception
{
    public RecursoYaExistenteException(string mensaje) : base(mensaje) { }
}