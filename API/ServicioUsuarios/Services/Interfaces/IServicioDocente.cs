using ServicioUsuarios.Data.DTOs;
using ServicioUsuarios.Data.DTOs.Docente;

namespace ServicioUsuarios.Services.Interfaces;

public interface IServicioDocente
{
    Task RegistrarAsync(RegistrarDocenteDTO docenteDto);
    Task<DocenteDTO> ActualizarAsync(HttpContext context, int idDocente, ActualizarDocenteDTO docenteDto);
    Task EliminarAsync(HttpContext context, int idDocente);
    Task<DocenteDTO?> ObtenerDocentePorIdAsync(int idDocente);
    Task CambiarContraseniaAsync(CambiarContraseniaDTO cambiarContraseniaDto, int idDocente, HttpContext context);
}
