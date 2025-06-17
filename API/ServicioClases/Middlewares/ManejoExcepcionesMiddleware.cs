using System.Net;
using ServicioClases.Exceptions;

namespace ServicioClases.Middlewares;

public class ManejoExcepcionesMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ManejoExcepcionesMiddleware> _logger;

    public ManejoExcepcionesMiddleware(RequestDelegate next, ILogger<ManejoExcepcionesMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext contexto)
    {
        try
        {
            await _next(contexto);
        }
        catch (Exception ex)
        {
            await ManejarExcepcionAsync(contexto, ex);
        }
    }

    private async Task ManejarExcepcionAsync(HttpContext contexto, Exception ex)
    {
        contexto.Response.ContentType = "application/json";

        switch (ex)
        {
            //BadRequest
            case IdInvalidaException badRequestEx:
                _logger.LogWarning(ex, "Id inválida.");
                contexto.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await contexto.Response.WriteAsJsonAsync(new { error = badRequestEx.Message });
                break;
            case CampoObligatorioException badRequestEx:
                _logger.LogWarning(ex, "Campo nulo.");
                contexto.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await contexto.Response.WriteAsJsonAsync(new { error = badRequestEx.Message });
                break;
            case CodigoClaseInvalidoException badRequestEx:
                _logger.LogWarning(ex, "Código de la clase inválido");
                contexto.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await contexto.Response.WriteAsJsonAsync(new { error = badRequestEx.Message });
                break;
            //NotFound
            case RecursoNoEncontradoException notFoundEx:
                _logger.LogWarning(ex, "Recurso no encontrado.");
                contexto.Response.StatusCode = (int)HttpStatusCode.NotFound;
                await contexto.Response.WriteAsJsonAsync(new { error = notFoundEx.Message });
                break;
            //Conflict
            case ClaseTerminadaException conflictEx:
                _logger.LogWarning(ex, "Clase terminada");
                contexto.Response.StatusCode = (int)HttpStatusCode.Conflict;
                await contexto.Response.WriteAsJsonAsync(new { error = conflictEx.Message });
                break;
            default:
                _logger.LogError(ex, "Ocurrió un error inesperado.");
                contexto.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await contexto.Response.WriteAsJsonAsync(new { error = "Ocurrió un error inesperado.", detalles = ex.Message });
                break;
        }
    }
}