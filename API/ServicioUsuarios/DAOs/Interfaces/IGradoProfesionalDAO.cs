using ServicioUsuarios.Entities;

namespace ServicioUsuarios.DAOs.Interfaces;

public interface IGradoProfesionalDAO
{
    Task<List<grado_profesional>> obtenerTodosAsync();
    Task<grado_profesional> obtenerPorIdAsync(int id);
}