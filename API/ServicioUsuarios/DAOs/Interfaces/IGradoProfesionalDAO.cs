using ServicioUsuarios.Entities;

namespace ServicioUsuarios.DAOs.Interfaces;

public interface IGradoProfesionalDAO
{
    Task<List<grado_profesional>> ObtenerTodosAsync();
    Task<grado_profesional> ObtenerPorIdAsync(int id);
}