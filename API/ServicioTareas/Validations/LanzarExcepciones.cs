namespace ServicioTareas.Validations;

public static class LanzarExcepciones
{
    public static Exception lanzarExcepcion(string tipo, string mensaje)
    {
        switch (tipo)
        {
            default:
                return new Exception(mensaje);
        }
    }
}