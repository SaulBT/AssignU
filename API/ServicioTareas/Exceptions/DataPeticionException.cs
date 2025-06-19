namespace ServicioTareas.Exceptions;

public class DataPeticionException : ArgumentException
{
    public DataPeticionException(string mensaje) : base(mensaje) { }
}