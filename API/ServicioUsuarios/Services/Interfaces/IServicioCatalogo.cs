using ServicioUsuarios.Entities;

namespace ServicioUsuarios.Services.Interfaces;

public interface IServicioCatalogo
{
    Task<List<grado_estudio>> ObtenerGradosEstudiosAsync();
    Task<List<grado_profesional>> ObtenerGradosProfesionalesAsync();
    Task<grado_estudio> ObtenerGradoEstudioPorIdAsync(int id);
    Task<grado_profesional> ObtenerGradoProfesionalPorIdAsync(int id);
}
