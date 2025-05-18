using Microsoft.EntityFrameworkCore;
using ServicioUsuarios.Entities;

public class GradoProfesionalDAO : IGradoProfesionalDAO
{
    private readonly asingu_usuarios_bdContext _context;

    public GradoProfesionalDAO(asingu_usuarios_bdContext context)
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