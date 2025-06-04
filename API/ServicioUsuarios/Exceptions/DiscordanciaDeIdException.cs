namespace ServicioUsuarios.Exceptions;

public class DiscordanciaDeIdException : Exception
{
    public DiscordanciaDeIdException(string mensaje) : base(mensaje) { }
}