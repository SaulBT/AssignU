using Microsoft.EntityFrameworkCore;
using ServicioClases.Data.DAOs.Interfaces;
using ServicioClases.Data.DTOs;
using ServicioClases.Models;

namespace ServicioClases.Data.DAOs.Implementations;

public class ClaseDAO : IClaseDAO
{
    private readonly ClasesDbContext _context;

    public ClaseDAO(ClasesDbContext context)
    {
        _context = context;
    }

    public async Task<Clase> crearClaseAsync(CrearClaseDTO crearClaseDTO, string codigoClase)
    {
        try
        {
            var clase = new Clase
            {
                Codigo = codigoClase,
                Nombre = crearClaseDTO.nombre
            };
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

    public async Task actualizarClaseAsync(Clase clase)
    {
        try
        {
            var claseExistente = await _context.Clase
                .FirstOrDefaultAsync(c => c.IdClase == clase.IdClase);

            claseExistente.Nombre = clase.Nombre;
            _context.Clase.Update(claseExistente);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al actualizar la clase: " + ex.Message + " - " + ex.InnerException?.Message);
        }
    }

    public async Task eliminarClaseAsync(int idClase)
    {
        try
        {
            await _context.Clase
                .Where(c => c.IdClase == idClase)
                .ExecuteDeleteAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al eliminar la clase: " + ex.Message + " - " + ex.InnerException?.Message);
        }
    }

    public async Task<Clase> obtenerClasePorIdAsync(int idClase)
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

    public async Task<Clase?> obtenerClasePorCodigoAsync(string codigoClase)
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

    public async Task<List<Clase>> obtenerClasesDeAlumnoAsync(List<Registro> registros)
    {
        try
        {
            var clases = new List<Clase>();
            foreach (var registro in registros)
            {
                var clase = await obtenerClasePorIdAsync(registro.IdClase);
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

    public async Task<List<Clase>> obtenerClasesDeDocenteAsync(int idDocente)
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