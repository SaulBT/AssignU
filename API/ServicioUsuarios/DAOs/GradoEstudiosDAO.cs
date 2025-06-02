using Microsoft.EntityFrameworkCore;
using ServicioUsuarios.Entities;

public class GradoEstudiosDAO: IGradoEstudiosDAO
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
            throw new Exception("Error al obtener los grados de estudios: " + ex.Message);
        }
    }
}