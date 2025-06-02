using Microsoft.EntityFrameworkCore;
using ServicioUsuarios.Entities;
using ServicioUsuarios.DTOs;

namespace ServicioUsuarios.DAOs;

public class DocenteDAO
{
    private readonly usuarios_bd_assignuContext _context;

    public DocenteDAO(usuarios_bd_assignuContext context)
    {
        _context = context;
    }

    public async Task actualizarAsync(docente docente)
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
            throw new Exception("Error al actualizar el docente: " + ex.Message);
        }
    }

    public async Task eliminarAsync(int id)
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
            throw new Exception("Error al eliminar el docente: " + ex.Message);
        }
    }

    public async Task<DocenteDTO?> obtenerPorIdDtoAsync(int id)
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
            throw new Exception("Error al obtener el docente por ID: " + ex.Message);
        }
    }

    public async Task<docente?> obtenerPorIdNormalAsync(int id)
    {
        try
        {
            return await _context.docentes.FindAsync(id);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener el docente por ID: " + ex.Message);
        }
    }

    public async Task<docente?> obtenerPorNombreUsuarioAsync(string nombreUsuario)
    {
        try
        {
            return await _context.docentes
                .FirstOrDefaultAsync(d => d.nombreUsuario == nombreUsuario);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener el docente por nombre de usuario: " + ex.Message);
        }
    }

    public async Task<docente?> obtenerPorNombreUsuarioEIdAsync(string nombreUsuario, int id)
    {
        try
        {
            return await _context.docentes
                .FirstOrDefaultAsync(d => d.nombreUsuario == nombreUsuario && d.idDocente != id);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener el docente por nombre de usuario y ID: " + ex.Message);
        }
    }

    public async Task<docente?> obtenerPorCorreoAsync(string correo)
    {
        try
        {
            return await _context.docentes
                .FirstOrDefaultAsync(d => d.correo == correo);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener el docente por correo: " + ex.Message);
        }
    }

    public async Task agregarDocenteAsync(docente docente)
    {
        try
        {
            await _context.docentes.AddAsync(docente);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al registrar el docente: " + ex.Message + "\nExcepción interna:" + ex.InnerException?.Message);
        }
    }
}