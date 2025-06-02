using ServicioUsuarios.Entities;
using ServicioUsuarios.DTOs;

namespace ServicioUsuarios.Services.Interfaces;

public interface IDocenteService
{
    Task<docente> registrarAsync(RegistrarDocenteDTO docenteDto);
    Task actualizarAsync(int id, ActualizarDocenteDTO docenteDto);
    Task eliminarAsync(int id);
    Task<DocenteDTO?> obtenerPorIdAsync(int id);
}
