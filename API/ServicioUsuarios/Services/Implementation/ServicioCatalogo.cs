using ServicioUsuarios.Entities;
using ServicioUsuarios.Services.Interfaces;
using ServicioUsuarios.Exceptions;
using ServicioUsuarios.DAOs.Interfaces;

namespace ServicioUsuarios.Services.Implementation;

public class ServicioCatalogo : IServicioCatalogo
{
    private readonly IGradoEstudiosDAO _gradoEstudiosDAO;
    private readonly IGradoProfesionalDAO _gradoProfesionalDAO;

    public ServicioCatalogo(IGradoEstudiosDAO gradoEstudiosDAO, IGradoProfesionalDAO gradoProfesionalDAO)
    {
        _gradoEstudiosDAO = gradoEstudiosDAO;
        _gradoProfesionalDAO = gradoProfesionalDAO;
    }

    public async Task<List<grado_estudio>> ObtenerGradosEstudiosAsync()
    {
        var lista = await _gradoEstudiosDAO.ObtenerTodosAsync();
        verificarCatalogoGradoEstudio(lista);

        return lista;
    }

    public async Task<List<grado_profesional>> ObtenerGradosProfesionalesAsync()
    {
        var lista = await _gradoProfesionalDAO.ObtenerTodosAsync();
        verificarCatalogoGradoProfesional(lista);

        return lista;
    }

    public async Task<grado_estudio> ObtenerGradoEstudioPorIdAsync(int id)
    {
        var gradoEstudio = await _gradoEstudiosDAO.ObtenerPorIdAsync(id);
        verificarGradoEstudioNulo(gradoEstudio);

        return gradoEstudio;
    }

    public async Task<grado_profesional> ObtenerGradoProfesionalPorIdAsync(int id)
    {
        var gradoProfesional = await _gradoProfesionalDAO.ObtenerPorIdAsync(id);
        verificarGradoProfesionalNulo(gradoProfesional);

        return gradoProfesional;
    }

    private void verificarCatalogoGradoEstudio(List<grado_estudio> catalogo)
    {
        if (catalogo == null || catalogo.Count == 0)
        {
            throw new RecursoNoEncontradoException("No se encontraron grados de estudio.");
        }
    }

    private void verificarCatalogoGradoProfesional(List<grado_profesional> catalogo)
    {
        if (catalogo == null || catalogo.Count == 0)
        {
            throw new RecursoNoEncontradoException("No se encontraron grados profesionales.");
        }
    }

    private void verificarGradoEstudioNulo(grado_estudio gradoEstudio)
    {
        if (gradoEstudio == null)
        {
            throw new RecursoNoEncontradoException("El grado de estudio no puede ser nulo.");
        }
    }

    private void verificarGradoProfesionalNulo(grado_profesional gradoProfesional)
    {
        if (gradoProfesional == null)
        {
            throw new RecursoNoEncontradoException("El grado profesional no puede ser nulo.");
        }
    }
}