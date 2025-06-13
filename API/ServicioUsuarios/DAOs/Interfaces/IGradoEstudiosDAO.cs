using ServicioUsuarios.Entities;

namespace ServicioUsuarios.DAOs.Interfaces;

public interface IGradoEstudiosDAO
{
    Task<List<grado_estudio>> ObtenerTodosAsync();
    Task<grado_estudio> ObtenerPorIdAsync(int id);
}