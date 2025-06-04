using ServicioUsuarios.Entities;

namespace ServicioUsuarios.DAOs.Interfaces;

public interface IGradoEstudiosDAO
{
    Task<List<grado_estudio>> obtenerTodosAsync();
    Task<grado_estudio> obtenerPorIdAsync(int id);
}