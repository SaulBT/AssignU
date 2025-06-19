namespace ServicioUsuarios.Validations;

public static class LanzarExcepciones
{
    public static Exception lanzarExcepcion(string tipo, string mensaje)
    {
        switch (tipo)
        {
            case "ExceptionInServicioClases":
                return new Exception($"Error en ServicioClases: {mensaje}");
            default:
                return new Exception(mensaje);
        }
    }
}