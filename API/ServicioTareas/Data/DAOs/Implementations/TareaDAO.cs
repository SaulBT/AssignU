using Microsoft.EntityFrameworkCore;
using ServicioTareas.Data.DAOs.Interfaces;
using ServicioTareas.Data.DTOs;
using ServicioTareas.Models;

namespace ServicioTareas.Data.DAOs.Implementations;

public class TareaDAO : ITareaDAO
{
    private readonly TareasDbContext _contexto;

    public TareaDAO(TareasDbContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<Tarea> crearTareaAsync(CrearTareaDTO crearTareaDTO)
    {
        try
        {
            var tarea = new Tarea
            {
                IdClase = crearTareaDTO.idClase,
                Nombre = crearTareaDTO.nombre,
                IdArchivo = crearTareaDTO.idArchivo,
                FechaLimite = crearTareaDTO.fechaLimite,
                Estado = "activa"
            };
            _contexto.Tarea.Add(tarea);
            await _contexto.SaveChangesAsync();

            tarea = await obtenerTareaPorIdClaseYNombreAsync(crearTareaDTO.idClase, crearTareaDTO.nombre);
            return tarea;
        }
        catch (Exception ex)
        {
            throw new Exception("Error al guardar la tarea: " + ex.Message + " - " + ex.InnerException?.Message);
        }
    }

    public async Task<Tarea> editarTareaAsync(EditarTareaDTO editarTareaDTO)
    {
        try
        {
            var tarea = await obtenerTareaPorIdAsync(editarTareaDTO.idTarea);
            tarea.Nombre = editarTareaDTO.nombre;
            tarea.IdArchivo = editarTareaDTO.idArchivo;
            tarea.FechaLimite = editarTareaDTO.fechaLimite;
            tarea.Estado = "activa";
            _contexto.Tarea.Update(tarea);
            await _contexto.SaveChangesAsync();

            return tarea;
        }
        catch (Exception ex)
        {
            throw new Exception("Error al editar la tarea: " + ex.Message + " - " + ex.InnerException?.Message);
        }
    }

    public async Task eliminarTareaAsync(Tarea tarea)
    {
        try
        {
            _contexto.Tarea.Remove(tarea);
            await _contexto.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al eliminar la tarea: " + ex.Message + " - " + ex.InnerException?.Message);
        }
    }

    public async Task<Tarea?> obtenerTareaPorIdAsync(int idTarea)
    {
        try
        {
            var tarea = await _contexto.Tarea.FirstOrDefaultAsync(t => t.IdTarea == idTarea);
            return tarea;
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener la tarea por id: " + ex.Message + " - " + ex.InnerException?.Message);
        }
    }

    public async Task<Tarea?> obtenerTareaPorIdClaseYNombreAsync(int idClase, string nombre)
    {
        try
        {
            var tarea = await _contexto.Tarea.FirstOrDefaultAsync(t => t.IdClase == idClase && t.Nombre == nombre);
            return tarea;
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener la tarea por idClase y nombre: " + ex.Message + " - " + ex.InnerException?.Message);
        }
    }

    public async Task<Tarea?> obtenerTareaPorIdTareaYNombreAsync(int idTarea, string nombre)
    {
        try
        {
            var tarea = await _contexto.Tarea.FirstOrDefaultAsync(t => t.IdTarea != idTarea && t.Nombre == nombre);
            return tarea;
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener la tarea por idTarea y nombre: " + ex.Message + " - " + ex.InnerException?.Message);
        }
    }

    public async Task<List<Tarea>?> obtenerTareasPorIdClaseAsync(int idClase)
    {
        try
        {
            var tarea = await _contexto.Tarea.Where(t => t.IdClase == idClase)
                .ToListAsync();

            return tarea;
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener las tareas de una clase: " + ex.Message + " - " + ex.InnerException?.Message);
        }
    }
}