using ServicioClases.Data.DTOs.RPC;

namespace ServicioClases.Validations;

public static class DetectorExcepciones
{
    public static RespuestaRPCDTO detectarExcepcion(Exception ex)
    {
        if (ex is Exception)
        {
            return new RespuestaRPCDTO
            {
                Success = false,
                Error = new ErrorDTO
                {
                    Tipo = "ExceptionInServicioClases",
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