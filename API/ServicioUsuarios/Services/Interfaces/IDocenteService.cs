using ServicioUsuarios.Entities;
using ServicioUsuarios.DTOs;

namespace ServicioUsuarios.Services.Interfaces;

public interface IDocenteService
{
    Task<docente> registrarAsync(RegistrarDocenteDTO docenteDto);
    Task actualizarAsync(HttpContext context, ActualizarDocenteDTO docenteDto);
    Task eliminarAsync(HttpContext context);
    Task<DocenteDTO?> obtenerPorIdAsync(int id);
    Task cambiarContraseniaAsync(CambiarContraseniaDTO cambiarContraseniaDto, HttpContext context);
}
