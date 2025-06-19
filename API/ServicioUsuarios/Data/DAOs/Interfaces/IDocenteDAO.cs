using ServicioUsuarios.Data.DTOs.Docente;
using ServicioUsuarios.Models;

namespace ServicioUsuarios.Data.DAOs.Interfaces;

public interface IDocenteDAO
{
    Task ActualizarAsync(Docente docente);
    Task EliminarAsync(Docente docente);
    Task<Docente?> ObtenerDocentePorIdAsync(int id);
    Task<Docente?> ObtenerDocentePorNombreUsuarioAsync(string nombreUsuario);
    Task<Docente?> ObtenerDocentePorNombreUsuarioEIdAsync(string nombreUsuario, int id);
    Task<Docente?> ObtenerDocentePorCorreoAsync(string correo);
    Task AgregarDocenteAsync(Docente docente);
    Task<Docente> ObtenerDocentePorNombreUsuarioOCorreoAsync(string nombreUsuarioOCorreo);
}