using Microsoft.EntityFrameworkCore;
using ServicioUsuarios.Data.DAOs.Interfaces;
using ServicioUsuarios.Data.DTOs.Alumno;
using ServicioUsuarios.Models;

namespace ServicioUsuarios.Data.DAOs.Implementation;

public class AlumnoDAO : IAlumnoDAO
{
    private readonly UsuariosDbContext _context;

    public AlumnoDAO(UsuariosDbContext context)
    {
        _context = context;
    }

    public async Task ActualizarAsync(Alumno alumno)
    {
        try
        {
            var alumnoExistente = await _context.Alumno.FindAsync(alumno.IdAlumno);
            if (alumnoExistente != null)
            {
                alumnoExistente.NombreCompleto = alumno.NombreCompleto;
                alumnoExistente.NombreUsuario = alumno.NombreUsuario;
                alumnoExistente.Correo = alumno.Correo;
                alumnoExistente.IdGradoEstudios = alumno.IdGradoEstudios;

                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error al actualizar el alumno: " + ex.Message + " - " + ex.InnerException?.Message);
        }
    }

    public async Task EliminarAsync(Alumno alumnoAEliminar)
    {
        try
        {
            _context.Alumno.Remove(alumnoAEliminar);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al eliminar el alumno: " + ex.Message + " - " + ex.InnerException?.Message);
        }
    }

    public async Task<Alumno> ObtenerAlumnoPorIdAsync(int id)
    {
        try
        {
            var alumno = await _context.Alumno.FindAsync(id);
            if (alumno == null)
            {
                throw new Exception("Alumno no encontrado.");
            }
            return alumno;
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener el alumno por ID: " + ex.Message + " - " + ex.InnerException?.Message);
        }
    }

    public async Task<Alumno?> ObtenerPorNombreUsuarioAsync(string nombreUsuario)
    {
        try
        {
            return await _context.Alumno
                .FirstOrDefaultAsync(a => a.NombreUsuario == nombreUsuario);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener el alumno por nombre de usuario: " + ex.Message + " - " + ex.InnerException?.Message);
        }
    }

    public async Task<Alumno?> ObtenerPorNombreUsuarioEIdAsync(string nombreUsuario, int id)
    {
        try
        {
            return await _context.Alumno
                .FirstOrDefaultAsync(a => a.IdAlumno != id && a.NombreUsuario == nombreUsuario);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener al usuario por nombre de usuario y diferente id: " + ex.Message + " - " + ex.InnerException?.Message);
        }
    }

    public async Task<Alumno?> ObtenerPorCorreoAsync(string correo)
    {
        try
        {
            return await _context.Alumno
                .FirstOrDefaultAsync(a => a.Correo == correo);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener el alumno por correo: " + ex.Message + " - " + ex.InnerException?.Message);
        }
    }

    public async Task AgregarAlumnoAsync(Alumno alumno)
    {
        try
        {
            _context.Alumno.Add(alumno);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al registrar el alumno: " + ex.Message + " - " + ex.InnerException?.Message);
        }
    }

    public async Task<AlumnoDTO?> ObtenerPorNombreUsuarioOCorreoAsync(string nombreCompletoOCorreo)
    {
        try
        {
            return await _context.Alumno
                .Where(a => a.NombreUsuario == nombreCompletoOCorreo || a.Correo == nombreCompletoOCorreo)
                .Select(a => new AlumnoDTO
                {
                    IdAlumno = a.IdAlumno,
                    NombreCompleto = a.NombreCompleto,
                    NombreUsuario = a.NombreUsuario,
                    Correo = a.Correo,
                    IdGradoEstudios = (int)a.IdGradoEstudios
                })
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener el alumno por nombre de usuario o correo: " + ex.Message + " - " + ex.InnerException?.Message);
        }
    }
}