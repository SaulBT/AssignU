using Microsoft.EntityFrameworkCore;
using ServicioUsuarios.Models;
using ServicioUsuarios.Data.DAOs.Interfaces;
using ServicioUsuarios.Data.DTOs.Docente;

namespace ServicioUsuarios.Data.DAOs.Implementation;

public class DocenteDAO : IDocenteDAO
{
    private readonly UsuariosDbContext _context;

    public DocenteDAO(UsuariosDbContext context)
    {
        _context = context;
    }

    public async Task ActualizarAsync(Docente docente)
    {
        try
        {
            var docenteExistente = await _context.Docente.FindAsync(docente.IdDocente);
            if (docenteExistente != null)
            {
                docenteExistente.NombreCompleto = docente.NombreCompleto;
                docenteExistente.NombreUsuario = docente.NombreUsuario;
                docenteExistente.Correo = docente.Correo;
                docenteExistente.IdGradoProfesional = docente.IdGradoProfesional;

                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error al actualizar el docente: " + ex.Message + " - " + ex.InnerException?.Message);
        }
    }

    public async Task EliminarAsync(Docente docente)
    {
        try
        {
            _context.Docente.Remove(docente);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al eliminar el docente: " + ex.Message + " - " + ex.InnerException?.Message);
        }
    }

    public async Task<Docente?> ObtenerDocentePorIdAsync(int id)
    {
        try
        {
            return await _context.Docente.FindAsync(id);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener el docente por ID: " + ex.Message + " - " + ex.InnerException?.Message);
        }
    }

    public async Task<Docente?> ObtenerDocentePorNombreUsuarioAsync(string nombreUsuario)
    {
        try
        {
            return await _context.Docente
                .FirstOrDefaultAsync(d => d.NombreUsuario == nombreUsuario);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener el docente por nombre de usuario: " + ex.Message + " - " + ex.InnerException?.Message);
        }
    }

    public async Task<Docente?> ObtenerDocentePorNombreUsuarioEIdAsync(string nombreUsuario, int id)
    {
        try
        {
            return await _context.Docente
                .FirstOrDefaultAsync(d => d.NombreUsuario == nombreUsuario && d.IdDocente != id);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener el docente por nombre de usuario y ID: " + ex.Message + " - " + ex.InnerException?.Message);
        }
    }

    public async Task<Docente?> ObtenerDocentePorCorreoAsync(string correo)
    {
        try
        {
            return await _context.Docente
                .FirstOrDefaultAsync(d => d.Correo == correo);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener el docente por correo: " + ex.Message + " - " + ex.InnerException?.Message);
        }
    }

    public async Task AgregarDocenteAsync(Docente docente)
    {
        try
        {
            await _context.Docente.AddAsync(docente);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al registrar el docente: " + ex.Message + " - " + ex.InnerException?.Message);
        }
    }

    public async Task<Docente> ObtenerDocentePorNombreUsuarioOCorreoAsync(string nombreUsuarioOCorreo)
    {
        try
        {
            return await _context.Docente
                .Where(d => d.NombreUsuario == nombreUsuarioOCorreo || d.Correo == nombreUsuarioOCorreo)
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener el docente por nombre de usuario o correo: " + ex.Message + " - " + ex.InnerException?.Message);
        }
    }
}