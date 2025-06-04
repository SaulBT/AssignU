using ServicioUsuarios.DTOs;
using ServicioUsuarios.Entities;

namespace ServicioUsuarios.DAOs.Interfaces;

public interface IDocenteDAO
{
    Task actualizarAsync(docente docente);
    Task eliminarAsync(int id);
    Task<DocenteDTO?> obtenerPorIdDtoAsync(int id);
    Task<docente?> obtenerPorIdNormalAsync(int id);
    Task<docente?> obtenerPorNombreUsuarioAsync(string nombreUsuario);
    Task<docente?> obtenerPorNombreUsuarioEIdAsync(string nombreUsuario, int id);
    Task<docente?> obtenerPorCorreoAsync(string correo);
    Task agregarDocenteAsync(docente docente);
    Task<DocenteDTO?> obtenerPorNombreUsuarioOCorreoAsync(string nombreUsuarioOCorreo);
}