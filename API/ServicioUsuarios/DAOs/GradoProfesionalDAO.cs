using Microsoft.EntityFrameworkCore;
using ServicioUsuarios.Entities;

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
            throw new Exception("Error al obtener los grados de estudios: " + ex.Message);
        }
    }
}