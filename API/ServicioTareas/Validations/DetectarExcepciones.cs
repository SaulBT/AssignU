using ServicioTareas.Data.DTOs.RPC;
using ServicioTareas.Exceptions;

namespace ServicioTareas.Validations;

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
                    Mensaje = $"Excepción en ServicioTareas: {ex.Message}"
                }
            };
        }

        if (ex is DataPeticionException)
        {
            return new RespuestaRPCDTO
            {
                Success = false,
                Error = new ErrorDTO
                {
                    Tipo = "DataException",
                    Mensaje = $"Excepción en ServicioTareas: {ex.Message}"
                }
            };
        }

        return new RespuestaRPCDTO
        {
            Success = false,
            Error = new ErrorDTO
            {
                Tipo = "Desconocida",
                Mensaje = $"Excepción en ServicioTareas: {ex.Message}"
            }
        };
    }
}