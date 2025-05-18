using ServicioUsuarios.Entities;

namespace ServicioUsuarios.Data.Interfaces
{
    public interface IDocenteDAO
    {
        Task registrarAsync(docente docente);
        Task actualizarAsync(docente docente);
        Task eliminarAsync(int id);
        Task<docente?> obtenerPorIdAsync(int id);
    }
}