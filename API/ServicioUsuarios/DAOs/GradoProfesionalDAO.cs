using Microsoft.EntityFrameworkCore;
using ServicioUsuarios.DAOs.Interfaces;
using ServicioUsuarios.Entities;

namespace ServicioUsuarios.DAOs;

public class GradoProfesionalDAO : IGradoProfesionalDAO
{
    private readonly usuarios_bd_assignuContext _context;

    public GradoProfesionalDAO(usuarios_bd_assignuContext context)
    {
        _context = context;
    }

    public async Task<List<grado_profesional>> obtenerTodosAsync()
    {
        try
        {
            return await _context.grado_profesionals.ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener los grados de estudios: " + ex.Message + " - " + ex.InnerException?.Message);
        }
    }

    public async Task<grado_profesional> obtenerPorIdAsync(int id)
    {
        try
        {
            var gradoProfesional = await _context.grado_profesionals.FindAsync(id);
            return gradoProfesional;
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener el grado profesional por ID: " + ex.Message + " - " + ex.InnerException?.Message);
        }
    }
}