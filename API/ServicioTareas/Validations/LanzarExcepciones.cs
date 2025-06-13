using ServicioTareas.Exceptions;

namespace ServicioTareas.Validations;

public static class LanzarExcepciones
{
    public static Exception lanzarExcepcion(string tipo, string mensaje)
    {
        switch (tipo)
        {
            case "CuestionarioInvalido":
                return new CuestionarioInvalidoException(mensaje);
            case "PreguntaInvalida":
                return new PreguntaInvalidaException(mensaje);
            case "ValorInvalido":
                return new ValorInvalidoException(mensaje);
            default:
                return new Exception(mensaje);
        }
    }
}