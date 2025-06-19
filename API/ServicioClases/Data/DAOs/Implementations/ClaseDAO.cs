using Microsoft.EntityFrameworkCore;
using ServicioClases.Data.DAOs.Interfaces;
using ServicioClases.Models;

namespace ServicioClases.Data.DAOs.Implementations;

public class ClaseDAO : IClaseDAO
{
    private readonly ClasesDbContext _context;

    public ClaseDAO(ClasesDbContext context)
    {
        _context = context;
    }

    public async Task<Clase> CrearClaseAsync(Clase clase, string codigoClase)
    {
        try
        {
            _context.Clase.Add(clase);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al crear la clase: " + ex.Message + " - " + ex.InnerException?.Message);
        }

        try
        {
            return _context.Clase
                .FirstOrDefault(c => c.Codigo == codigoClase)
                ?? throw new Exception("Clase no encontrada después de la creación.");
        }
        catch (Exception ex)
        {
            throw new Exception("Error al retornar la clase: " + ex.Message + " - " + ex.InnerException?.Message);
        }
    }

    public async Task ActualizarClaseAsync(Clase claseActualizada)
    {
        try
        {
            var claseExistente = await _context.Clase
                .FirstOrDefaultAsync(c => c.IdClase == claseActualizada.IdClase);
            claseExistente.Nombre = claseActualizada.Nombre;
            _context.Clase.Update(claseExistente);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al actualizar la clase: " + ex.Message + " - " + ex.InnerException?.Message);
        }
    }

    public async Task EliminarClaseAsync(Clase claseEliminar)
    {
        try
        {
            _context.Clase.Remove(claseEliminar);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al eliminar la clase: " + ex.Message + " - " + ex.InnerException?.Message);
        }
    }

    public async Task<Clase> ObtenerClasePorIdAsync(int idClase)
    {
        try
        {
            return await _context.Clase
                .FirstOrDefaultAsync(c => c.IdClase == idClase);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener la clase por ID: " + ex.Message + " - " + ex.InnerException?.Message);
        }
    }

    public async Task<Clase?> ObtenerClasePorCodigoAsync(string codigoClase)
    {
        try
        {
            return await _context.Clase
                .FirstOrDefaultAsync(c => c.Codigo == codigoClase);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener la clase por código" + ex.Message + " - " + ex.InnerException?.Message);
        }
    }

    public async Task<List<Clase>> ObtenerClasesDeAlumnoAsync(List<Registro> registros)
    {
        try
        {
            var clases = new List<Clase>();
            foreach (var registro in registros)
            {
                var clase = await ObtenerClasePorIdAsync(registro.IdClase);
                if (clase != null)
                {
                    clases.Add(clase);
                }
            }

            return clases;
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener la clase por código" + ex.Message + " - " + ex.InnerException?.Message);
        }
        
    }

    public async Task<List<Clase>> ObtenerClasesDeDocenteAsync(int idDocente)
    {
         try
        {
            return await _context.Clase
                .Where(r => r.IdDocente == idDocente)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener la clase por código" + ex.Message + " - " + ex.InnerException?.Message);
        }
    }
}