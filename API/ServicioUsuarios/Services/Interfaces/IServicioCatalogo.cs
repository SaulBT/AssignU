using ServicioUsuarios.Data.DTOs.Catalogo;
using ServicioUsuarios.Models;

namespace ServicioUsuarios.Services.Interfaces;

public interface IServicioCatalogo
{
    Task<List<GradoEstudiosDTO>> ObtenerGradosEstudiosAsync();
    Task<List<GradoProfesionalDTO>> ObtenerGradosProfesionalesAsync();
    Task<GradoEstudiosDTO> ObtenerGradoEstudioPorIdAsync(int id);
    Task<GradoProfesionalDTO> ObtenerGradoProfesionalPorIdAsync(int id);
}
