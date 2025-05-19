using Microsoft.EntityFrameworkCore;
using ServicioUsuarios.Entities;

public class DocenteDAO : IDocenteDAO
{
    private readonly asingu_usuarios_bdContext _context;

    public DocenteDAO(asingu_usuarios_bdContext context)
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

    public async Task<docente?> obtenerPorIdAsync(int id)
    {
        try
        {
            return await _context.docentes
                .Include(d => d.idGradoProfesionalNavigation)
                .FirstOrDefaultAsync(d => d.idDocente == id);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener el docente por ID: " + ex.Message);
        }
    }

    public async Task registrarAsync(docente docente)
    {
        try
        {
            await _context.docentes.AddAsync(docente);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al registrar el docente: " + ex.Message);
        }
    }
}