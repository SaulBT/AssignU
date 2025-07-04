using Microsoft.EntityFrameworkCore;
using ServicioClases.Data.DAOs.Interfaces;
using ServicioClases.Models;

namespace ServicioClases.Data.DAOs.Implementations;

public class RegistroDAO : IRegistroDAO
{
    private readonly ClasesDbContext _context;

    public RegistroDAO(ClasesDbContext context)
    {
        _context = context;
    }

    public async Task CrearRegistroAsync(Registro registro)
    {
        try
        {
            _context.Registro.Add(registro);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al crear el registro: " + ex.Message + " - " + ex.InnerException?.Message);
        }
    }

    public async Task EliminarRegistroAsync(Registro registro)
    {
        try
        {
            _context.Registro.Remove(registro);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al eliminar el registro: " + ex.Message + " - " + ex.InnerException?.Message);
        }
    }

    public async Task ActualizarRegistroAsync(Registro registro)
    {
        try
        {
            var registroActual = await _context.Registro
                .FirstOrDefaultAsync(r => r.IdRegistro == registro.IdRegistro);
            registroActual.UltimoInicio = registro.UltimoInicio;
            _context.Registro.Update(registroActual);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al actualizar el registro: " + ex.Message + " - " + ex.InnerException?.Message);
        }
    }

    public async Task<List<Registro>> ObtenerRegistrosPorClaseAsync(int idClase)
    {
        try
        {
            return await _context.Registro
                .Where(r => r.IdClase == idClase)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener los registros de la clase: " + ex.Message + " - " + ex.InnerException?.Message);
        }
    }

    public async Task<List<Registro>> ObtenerRegistrosPorAlumnoAsync(int idAlumno)
    {
        try
        {
            return await _context.Registro
                .Where(r => r.IdAlumno == idAlumno)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener los registros del alumno: " + ex.Message + " - " + ex.InnerException?.Message);
        }
    }

    public async Task<Registro?> ObtenerRegistroPorIdAlumnoYClaseAsync(int idAlumno, int idClase)
    {
        try
        {
            return await _context.Registro
                .FirstOrDefaultAsync(r => r.IdAlumno == idAlumno && r.IdClase == idClase);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener el registro por alumno y clase: " + ex.Message + " - " + ex.InnerException?.Message);
        }
    }
}