using Microsoft.EntityFrameworkCore;
using ServicioUsuarios.DAOs.Interfaces;
using ServicioUsuarios.DTOs;
using ServicioUsuarios.Entities;

namespace ServicioUsuarios.DAOs.Implementation;

public class AlumnoDAO : IAlumnoDAO
{
    private readonly usuarios_bd_assignuContext _context;

    public AlumnoDAO(usuarios_bd_assignuContext context)
    {
        _context = context;
    }

    public async Task ActualizarAsync(alumno alumno)
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
            throw new Exception("Error al actualizar el alumno: " + ex.Message + " - " + ex.InnerException?.Message);
        }
    }

    public async Task EliminarAsync(int id)
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
            throw new Exception("Error al eliminar el alumno: " + ex.Message + " - " + ex.InnerException?.Message);
        }
    }

    public async Task<AlumnoDTO?> ObtenerPorIdDtoAsync(int id)
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
            throw new Exception("Error al obtener el alumno por ID: " + ex.Message + " - " + ex.InnerException?.Message);
        }
    }

    public async Task<alumno> ObtenerPorIdNormalAsync(int id)
    {
        try
        {
            var alumno = await _context.alumnos.FindAsync(id);
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

    public async Task<alumno?> ObtenerPorNombreUsuarioAsync(string nombreUsuario)
    {
        try
        {
            return await _context.alumnos
                .FirstOrDefaultAsync(a => a.nombreUsuario == nombreUsuario);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener el alumno por nombre de usuario: " + ex.Message + " - " + ex.InnerException?.Message);
        }
    }

    public async Task<alumno?> ObtenerPorNombreUsuarioEIdAsync(string nombreUsuario, int id)
    {
        try
        {
            return await _context.alumnos
                .FirstOrDefaultAsync(a => a.idAlumno != id && a.nombreUsuario == nombreUsuario);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener al usuario por nombre de usuario y diferente id: " + ex.Message + " - " + ex.InnerException?.Message);
        }
    }

    public async Task<alumno?> ObtenerPorCorreoAsync(string correo)
    {
        try
        {
            return await _context.alumnos
                .FirstOrDefaultAsync(a => a.correo == correo);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener el alumno por correo: " + ex.Message + " - " + ex.InnerException?.Message);
        }
    }

    public async Task AgregarAlumnoAsync(alumno alumno)
    {
        try
        {
            _context.alumnos.Add(alumno);
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
            return await _context.alumnos
                .Where(a => a.nombreUsuario == nombreCompletoOCorreo || a.correo == nombreCompletoOCorreo)
                .Select(a => new AlumnoDTO
                {
                    idAlumno = a.idAlumno,
                    nombreCompleto = a.nombreCompleto,
                    nombreUsuario = a.nombreUsuario,
                    correo = a.correo,
                    idGradoEstudios = (int)a.idGradoEstudios
                })
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener el alumno por nombre de usuario o correo: " + ex.Message + " - " + ex.InnerException?.Message);
        }
    }
}