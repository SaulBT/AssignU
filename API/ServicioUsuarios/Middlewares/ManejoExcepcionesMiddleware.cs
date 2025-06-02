using System.Net;
using ServicioUsuarios.Exceptions;

namespace ServicioUsuarios.Middlewares;

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
            case DiscordanciaDeIdException badRequestEx:
                _logger.LogWarning(ex, "Discordancia de ID detectada.");
                contexto.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await contexto.Response.WriteAsJsonAsync(new { error = badRequestEx.Message });
                break;
            case RecursoYaExistenteException statusConflictEx:
                _logger.LogWarning(ex, "Recurso ya existe.");
                contexto.Response.StatusCode = (int)HttpStatusCode.Conflict;
                await contexto.Response.WriteAsJsonAsync(new { error = statusConflictEx.Message });
                break;
            case IdInvalidaException badRequestEx:
                _logger.LogWarning(ex, "ID inválido proporcionado.");
                contexto.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await contexto.Response.WriteAsJsonAsync(new { error = badRequestEx.Message });
                break;
            default:
                _logger.LogError(ex, "Ocurrió un error inesperado.");
                contexto.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await contexto.Response.WriteAsJsonAsync(new { error = "Ocurrió un error inesperado.", detalles = ex.Message });
                break;
        }
    }
}