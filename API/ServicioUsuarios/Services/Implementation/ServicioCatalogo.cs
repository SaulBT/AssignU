using ServicioUsuarios.Models;
using ServicioUsuarios.Services.Interfaces;
using ServicioUsuarios.Exceptions;
using ServicioUsuarios.Data.DAOs.Interfaces;

namespace ServicioUsuarios.Services.Implementation;

public class ServicioCatalogo : IServicioCatalogo
{
    private readonly IGradoEstudiosDAO _gradoEstudiosDAO;
    private readonly IGradoProfesionalDAO _gradoProfesionalDAO;
    private readonly ILogger _logger;

    public ServicioCatalogo(IGradoEstudiosDAO gradoEstudiosDAO, IGradoProfesionalDAO gradoProfesionalDAO, ILogger logger)
    {
        _gradoEstudiosDAO = gradoEstudiosDAO;
        _gradoProfesionalDAO = gradoProfesionalDAO;
        _logger = logger;
    }

    public async Task<List<GradoEstudios>> ObtenerGradosEstudiosAsync()
    {
        _logger.LogInformation("Obteniendo todos los Grados de Estudio");
        var lista = await _gradoEstudiosDAO.ObtenerTodosAsync();
        verificarCatalogoGradoEstudio(lista);

        _logger.LogInformation("Se obtuvieron todos los Grados de Estudio");
        return lista;
    }

    public async Task<List<GradoProfesional>> ObtenerGradosProfesionalesAsync()
    {
        _logger.LogInformation("Obteniendo todos los Grados Profesionales");
        var lista = await _gradoProfesionalDAO.ObtenerTodosAsync();
        verificarCatalogoGradoProfesional(lista);

        _logger.LogInformation("Se obtuvieron todos los Grados Profesionales");
        return lista;
    }

    public async Task<GradoEstudios> ObtenerGradoEstudioPorIdAsync(int idGradoEstudios)
    {
        _logger.LogInformation("Buscando un Grado de Estudios");
        var gradoEstudio = await _gradoEstudiosDAO.ObtenerPorIdAsync(idGradoEstudios);
        verificarGradoEstudioNulo(gradoEstudio);

        _logger.LogInformation($"Grado de Estudio encontrado con la id {gradoEstudio.IdGradoEstudios}");
        return gradoEstudio;
    }

    public async Task<GradoProfesional> ObtenerGradoProfesionalPorIdAsync(int id)
    {
        _logger.LogInformation("Buscando un Grado Profesional");
        var gradoProfesional = await _gradoProfesionalDAO.ObtenerPorIdAsync(id);
        verificarGradoProfesionalNulo(gradoProfesional);

        _logger.LogInformation($"Grado Profesional encontrado con la id {gradoProfesional.IdGradoProfesional}");
        return gradoProfesional;
    }

    private void verificarCatalogoGradoEstudio(List<GradoEstudios> catalogo)
    {
        if (catalogo == null || catalogo.Count == 0)
        {
            throw new RecursoNoEncontradoException("No se encontraron grados de estudio.");
        }
    }

    private void verificarCatalogoGradoProfesional(List<GradoProfesional> catalogo)
    {
        if (catalogo == null || catalogo.Count == 0)
        {
            throw new RecursoNoEncontradoException("No se encontraron grados profesionales.");
        }
    }

    private void verificarGradoEstudioNulo(GradoEstudios gradoEstudio)
    {
        if (gradoEstudio == null)
        {
            throw new RecursoNoEncontradoException("El grado de estudio no puede ser nulo.");
        }
    }

    private void verificarGradoProfesionalNulo(GradoProfesional gradoProfesional)
    {
        if (gradoProfesional == null)
        {
            throw new RecursoNoEncontradoException("El grado profesional no puede ser nulo.");
        }
    }
}