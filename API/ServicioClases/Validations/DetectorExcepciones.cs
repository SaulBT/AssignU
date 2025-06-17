using ServicioClases.Data.DTOs.RPC;

namespace ServicioClases.Validations;

public static class DetectorExcepciones
{
    public static RespuestaRPCDTO detectarExcepcion(Exception ex)
    {
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