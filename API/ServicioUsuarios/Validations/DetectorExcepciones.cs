using ServicioUsuarios.Data.DTOs.RPC;
using ServicioUsuarios.Exceptions;

namespace ServicioUsuarios.Validations;

public static class DetectorExcepciones
{
    public static RespuestaRPCDTO detectarExcepcion(Exception ex)
    {
        if (ex is IdInvalidaException idEx)
        {
            return new RespuestaRPCDTO
            {
                Success = false,
                Error = new ErrorDTO
                {
                    Tipo = "IdInvalidaException",
                    Mensaje = ex.Message
                }
            };
        }

        return new RespuestaRPCDTO
        {
            Success = false,
            Error = new ErrorDTO
            {
                Tipo = "Desconocida",
                Mensaje = ex.Message
            }
        };
    }
}