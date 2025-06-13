using ServicioUsuarios.DTOs;
using ServicioUsuarios.Entities;

namespace ServicioUsuarios.DAOs.Interfaces;

public interface IDocenteDAO
{
    Task ActualizarAsync(docente docente);
    Task EliminarAsync(int id);
    Task<DocenteDTO?> ObtenerPorIdDtoAsync(int id);
    Task<docente?> ObtenerPorIdNormalAsync(int id);
    Task<docente?> ObtenerPorNombreUsuarioAsync(string nombreUsuario);
    Task<docente?> ObtenerPorNombreUsuarioEIdAsync(string nombreUsuario, int id);
    Task<docente?> ObtenerPorCorreoAsync(string correo);
    Task AgregarDocenteAsync(docente docente);
    Task<DocenteDTO?> ObtenerPorNombreUsuarioOCorreoAsync(string nombreUsuarioOCorreo);
}