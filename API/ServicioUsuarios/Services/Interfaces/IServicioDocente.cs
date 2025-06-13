using ServicioUsuarios.Entities;
using ServicioUsuarios.DTOs;

namespace ServicioUsuarios.Services.Interfaces;

public interface IServicioDocente
{
    Task<docente> RegistrarAsync(RegistrarDocenteDTO docenteDto);
    Task ActualizarAsync(HttpContext context, ActualizarDocenteDTO docenteDto);
    Task EliminarAsync(HttpContext context);
    Task<DocenteDTO?> ObtenerPorIdAsync(int id);
    Task CambiarContraseniaAsync(CambiarContraseniaDTO cambiarContraseniaDto, HttpContext context);
}
