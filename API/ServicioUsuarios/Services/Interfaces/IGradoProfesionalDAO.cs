using ServicioUsuarios.Entities;

public interface IGradoProfesionalDAO
{
    Task<List<grado_profesional>> obtenerTodosAsync();
}
