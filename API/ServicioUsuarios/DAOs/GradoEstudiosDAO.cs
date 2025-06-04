using Microsoft.EntityFrameworkCore;
using ServicioUsuarios.DAOs.Interfaces;
using ServicioUsuarios.Entities;

namespace ServicioUsuarios.DAOs;

public class GradoEstudiosDAO : IGradoEstudiosDAO
{
    private readonly usuarios_bd_assignuContext _context;

    public GradoEstudiosDAO(usuarios_bd_assignuContext context)
    {
        _context = context;
    }

    public async Task<List<grado_estudio>> obtenerTodosAsync()
    {
        try
        {
            return await _context.grado_estudios.ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener los grados de estudios: " + ex.Message + " - " + ex.InnerException?.Message);
        }
    }

    public async Task<grado_estudio> obtenerPorIdAsync(int id)
    {
        try
        {
            var gradoEstudio = await _context.grado_estudios.FindAsync(id);
            return gradoEstudio;
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener el grado de estudio por ID: " + ex.Message + " - " + ex.InnerException?.Message);
        }
    }
}