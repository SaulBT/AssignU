using ServicioUsuarios.Models;

namespace ServicioUsuarios.Data.DAOs.Interfaces;

public interface IGradoProfesionalDAO
{
    Task<List<GradoProfesional>> ObtenerTodosAsync();
    Task<GradoProfesional> ObtenerPorIdAsync(int id);
}