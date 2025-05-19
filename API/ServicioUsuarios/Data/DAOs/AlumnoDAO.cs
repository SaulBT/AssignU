using Microsoft.EntityFrameworkCore;
using ServicioUsuarios.Entities;

public class AlumnoDAO : IAlumnoDAO
{
    private readonly asingu_usuarios_bdContext _context;

    public AlumnoDAO(asingu_usuarios_bdContext context)
    {
        _context = context;
    }

    public async Task actualizarAsync(alumno alumno)
    {
        try
        {
            var alumnoExistente = await _context.alumnos.FindAsync(alumno.idAlumno);
            if (alumnoExistente != null)
            {
                alumnoExistente.nombreCompleto = alumno.nombreCompleto;
                alumnoExistente.nombreUsuario = alumno.nombreUsuario;
                alumnoExistente.correo = alumno.correo;
                alumnoExistente.idGradoEstudios = alumno.idGradoEstudios;

                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error al actualizar el alumno: " + ex.Message);
        }
    }

    public async Task eliminarAsync(int id)
    {
        try
        {
            var alumno = await _context.alumnos.FindAsync(id);
            if (alumno != null)
            {
                _context.alumnos.Remove(alumno);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error al eliminar el alumno: " + ex.Message);
        }
    }

    public async Task<AlumnoDTO?> obtenerPorIdAsync(int id)
    {
        try
        {
            return await _context.alumnos
                .Where(a => a.idAlumno == id)
                .Select(a => new AlumnoDTO
                {
                    idAlumno = a.idAlumno,
                    nombreCompleto = a.nombreCompleto,
                    nombreUsuario = a.nombreUsuario,
                    correo = a.correo,
                    idGradoEstudios = (int)a.idGradoEstudios
                })
                .FirstOrDefaultAsync(a => a.idAlumno == id);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener el alumno por ID: " + ex.Message);
        }
    }

    public async Task registrarAsync(alumno alumno)
    {
        try
        {
            _context.alumnos.Add(alumno);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al registrar el alumno: " + ex.Message);
        }
    }
}