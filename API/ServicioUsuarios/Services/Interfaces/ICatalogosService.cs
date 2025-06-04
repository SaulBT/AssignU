using ServicioUsuarios.Entities;

namespace ServicioUsuarios.Services.Interfaces;

public interface ICatalogosService
{
    Task<List<grado_estudio>> obtenerGradosEstudiosAsync();
    Task<List<grado_profesional>> obtenerGradosProfesionalesAsync();
    Task<grado_estudio> obtenerGradoEstudioPorIdAsync(int id);
    Task<grado_profesional> obtenerGradoProfesionalPorIdAsync(int id);
}
