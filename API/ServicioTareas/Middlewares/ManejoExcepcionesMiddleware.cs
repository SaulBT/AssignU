using System.Net;
using ServicioTareas.Exceptions;

namespace ServicioTareas.Middlewares;

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
            case RecursoNoEncontradoException notFoundEx:
                _logger.LogWarning(ex, "Recurso no encontrado.");
                contexto.Response.StatusCode = (int)HttpStatusCode.NotFound;
                await contexto.Response.WriteAsJsonAsync(new { error = notFoundEx.Message });
                break;
            case RecursoYaExistenteException exception:
                _logger.LogWarning(ex, "Recurso existente");
                contexto.Response.StatusCode = (int)HttpStatusCode.Conflict;
                await contexto.Response.WriteAsJsonAsync(new { error = exception.Message });
                break;
            case CuestionarioInvalidoException exception:
                _logger.LogWarning(ex, "Cuestionario Invalido");
                contexto.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await contexto.Response.WriteAsJsonAsync(new { error = exception.Message });
                break;
            case PreguntaInvalidaException exception:
                _logger.LogWarning(ex, "Pregunta Invalida");
                contexto.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await contexto.Response.WriteAsJsonAsync(new { error = exception.Message });
                break;
            case ValorInvalidoException exception:
                _logger.LogWarning(ex, "Valor Invalido");
                contexto.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await contexto.Response.WriteAsJsonAsync(new { error = exception.Message });
                break;
            default:
                _logger.LogError(ex, "Ocurrió un error inesperado.");
                contexto.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await contexto.Response.WriteAsJsonAsync(new { error = "Ocurrió un error inesperado.", detalles = ex.Message });
                break;
        }
    }
}