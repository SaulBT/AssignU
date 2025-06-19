using ServicioClases.Exceptions;

namespace ServicioClases.Validations;

public static class LanzarExcepciones
{
    public static Exception lanzarExcepcion(string tipo, string mensaje)
    {
        switch (tipo)
        {
            case "IdInvalidaException":
                return new IdInvalidaException(mensaje);
            case "CampoObligatorioException":
                return new CampoObligatorioException(mensaje);
            case "DataException":
                return new DataPeticionException(mensaje);
            default:
                return new Exception(mensaje);
        }
    }
}