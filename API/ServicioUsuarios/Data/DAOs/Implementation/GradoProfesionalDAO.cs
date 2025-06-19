using Microsoft.EntityFrameworkCore;
using ServicioUsuarios.Data.DAOs.Interfaces;
using ServicioUsuarios.Models;

namespace ServicioUsuarios.Data.DAOs.Implementation;

public class GradoProfesionalDAO : IGradoProfesionalDAO
{
    private readonly UsuariosDbContext _context;

    public GradoProfesionalDAO(UsuariosDbContext context)
    {
        _context = context;
    }

    public async Task<List<GradoProfesional>> ObtenerTodosAsync()
    {
        try
        {
            return await _context.GradoProfesional.ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener los grados de estudios: " + ex.Message + " - " + ex.InnerException?.Message);
        }
    }

    public async Task<GradoProfesional> ObtenerPorIdAsync(int id)
    {
        try
        {
            var gradoProfesional = await _context.GradoProfesional.FindAsync(id);
            return gradoProfesional;
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener el grado profesional por ID: " + ex.Message + " - " + ex.InnerException?.Message);
        }
    }
}