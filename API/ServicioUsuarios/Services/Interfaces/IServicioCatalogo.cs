using ServicioUsuarios.Models;

namespace ServicioUsuarios.Services.Interfaces;

public interface IServicioCatalogo
{
    Task<List<GradoEstudios>> ObtenerGradosEstudiosAsync();
    Task<List<GradoProfesional>> ObtenerGradosProfesionalesAsync();
    Task<GradoEstudios> ObtenerGradoEstudioPorIdAsync(int id);
    Task<GradoProfesional> ObtenerGradoProfesionalPorIdAsync(int id);
}
