using Microsoft.EntityFrameworkCore;
using ServicioUsuarios.Data.DAOs.Interfaces;
using ServicioUsuarios.Models;

namespace ServicioUsuarios.Data.DAOs.Implementation;

public class GradoEstudiosDAO : IGradoEstudiosDAO
{
    private readonly UsuariosDbContext _context;

    public GradoEstudiosDAO(UsuariosDbContext context)
    {
        _context = context;
    }

    public async Task<List<GradoEstudios>> ObtenerTodosAsync()
    {
        try
        {
            return await _context.GradoEstudios.ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener los grados de estudios: " + ex.Message + " - " + ex.InnerException?.Message);
        }
    }

    public async Task<GradoEstudios> ObtenerPorIdAsync(int id)
    {
        try
        {
            var gradoEstudio = await _context.GradoEstudios.FindAsync(id);
            return gradoEstudio;
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener el grado de estudio por ID: " + ex.Message + " - " + ex.InnerException?.Message);
        }
    }
}