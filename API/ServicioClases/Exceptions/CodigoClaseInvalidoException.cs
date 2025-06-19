namespace ServicioClases.Exceptions;

public class CodigoClaseInvalidoException : ArgumentException
{
    public CodigoClaseInvalidoException(string mensaje) : base(mensaje) { }
}