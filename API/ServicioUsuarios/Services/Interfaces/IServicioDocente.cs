using ServicioUsuarios.Data.DTOs;
using ServicioUsuarios.Data.DTOs.Docente;

namespace ServicioUsuarios.Services.Interfaces;

public interface IServicioDocente
{
    Task<DocenteDTO> RegistrarAsync(RegistrarDocenteDTO docenteDto);
    Task<DocenteDTO> ActualizarAsync(HttpContext context, ActualizarDocenteDTO docenteDto);
    Task EliminarAsync(HttpContext context);
    Task<DocenteDTO?> ObtenerDocentePorIdAsync(int idDocente);
    Task CambiarContraseniaAsync(CambiarContraseniaDTO cambiarContraseniaDto, HttpContext context);
}
