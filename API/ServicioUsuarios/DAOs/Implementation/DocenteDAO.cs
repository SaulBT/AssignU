using Microsoft.EntityFrameworkCore;
using ServicioUsuarios.Entities;
using ServicioUsuarios.DTOs;
using ServicioUsuarios.DAOs.Interfaces;

namespace ServicioUsuarios.DAOs.Implementation;

public class DocenteDAO : IDocenteDAO
{
    private readonly usuarios_bd_assignuContext _context;

    public DocenteDAO(usuarios_bd_assignuContext context)
    {
        _context = context;
    }

    public async Task ActualizarAsync(docente docente)
    {
        try
        {
            var docenteExistente = await _context.docentes.FindAsync(docente.idDocente);
            if (docenteExistente != null)
            {
                docenteExistente.nombreCompleto = docente.nombreCompleto;
                docenteExistente.nombreUsuario = docente.nombreUsuario;
                docenteExistente.correo = docente.correo;
                docenteExistente.idGradoProfesional = docente.idGradoProfesional;

                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error al actualizar el docente: " + ex.Message + " - " + ex.InnerException?.Message);
        }
    }

    public async Task EliminarAsync(int id)
    {
        try
        {
            var docente = await _context.docentes.FindAsync(id);
            if (docente != null)
            {
                _context.docentes.Remove(docente);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error al eliminar el docente: " + ex.Message + " - " + ex.InnerException?.Message);
        }
    }

    public async Task<DocenteDTO?> ObtenerPorIdDtoAsync(int id)
    {
        try
        {
            return await _context.docentes
                .Where(d => d.idDocente == id)
                .Select(d => new DocenteDTO
                {
                    idDocente = d.idDocente,
                    nombreCompleto = d.nombreCompleto,
                    nombreUsuario = d.nombreUsuario,
                    correo = d.correo,
                    idGradoProfesional = (int)d.idGradoProfesional
                })
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener el docente por ID: " + ex.Message + " - " + ex.InnerException?.Message);
        }
    }

    public async Task<docente?> ObtenerPorIdNormalAsync(int id)
    {
        try
        {
            return await _context.docentes.FindAsync(id);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener el docente por ID: " + ex.Message + " - " + ex.InnerException?.Message);
        }
    }

    public async Task<docente?> ObtenerPorNombreUsuarioAsync(string nombreUsuario)
    {
        try
        {
            return await _context.docentes
                .FirstOrDefaultAsync(d => d.nombreUsuario == nombreUsuario);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener el docente por nombre de usuario: " + ex.Message + " - " + ex.InnerException?.Message);
        }
    }

    public async Task<docente?> ObtenerPorNombreUsuarioEIdAsync(string nombreUsuario, int id)
    {
        try
        {
            return await _context.docentes
                .FirstOrDefaultAsync(d => d.nombreUsuario == nombreUsuario && d.idDocente != id);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener el docente por nombre de usuario y ID: " + ex.Message + " - " + ex.InnerException?.Message);
        }
    }

    public async Task<docente?> ObtenerPorCorreoAsync(string correo)
    {
        try
        {
            return await _context.docentes
                .FirstOrDefaultAsync(d => d.correo == correo);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener el docente por correo: " + ex.Message + " - " + ex.InnerException?.Message);
        }
    }

    public async Task AgregarDocenteAsync(docente docente)
    {
        try
        {
            await _context.docentes.AddAsync(docente);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al registrar el docente: " + ex.Message + " - " + ex.InnerException?.Message);
        }
    }

    public async Task<DocenteDTO?> ObtenerPorNombreUsuarioOCorreoAsync(string nombreUsuarioOCorreo)
    {
        try
        {
            return await _context.docentes
                .Where(d => d.nombreUsuario == nombreUsuarioOCorreo || d.correo == nombreUsuarioOCorreo)
                .Select(d => new DocenteDTO
                {
                    idDocente = d.idDocente,
                    nombreCompleto = d.nombreCompleto,
                    nombreUsuario = d.nombreUsuario,
                    correo = d.correo,
                    idGradoProfesional = (int)d.idGradoProfesional
                })
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener el docente por nombre de usuario o correo: " + ex.Message + " - " + ex.InnerException?.Message);
        }
    }
}