using ServicioUsuarios.Models;

namespace ServicioUsuarios.Data.DAOs.Interfaces;

public interface IGradoEstudiosDAO
{
    Task<List<GradoEstudios>> ObtenerTodosAsync();
    Task<GradoEstudios> ObtenerPorIdAsync(int id);
}