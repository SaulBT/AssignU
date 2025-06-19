namespace ServicioClases.Middlewares;

public static class ManejoExcepcionesMiddlewareExtensions
{
    public static IApplicationBuilder UseManejoExcepciones(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ManejoExcepcionesMiddleware>();
    }
}