namespace ServicioUsuarios.Exceptions;

public class TipoUsuarioInvalidoException : ArgumentException
{
    public TipoUsuarioInvalidoException(string mensaje) : base(mensaje) { }
}