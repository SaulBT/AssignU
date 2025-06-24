using ServicioUsuarios.Models;
using ServicioUsuarios.Services.Interfaces;
using ServicioUsuarios.Exceptions;
using ServicioUsuarios.Data.DAOs.Interfaces;
using ServicioUsuarios.Data.DTOs.Catalogo;

namespace ServicioUsuarios.Services.Implementation;

public class ServicioCatalogo : IServicioCatalogo
{
    private readonly IGradoEstudiosDAO _gradoEstudiosDAO;
    private readonly IGradoProfesionalDAO _gradoProfesionalDAO;
    private readonly ILogger<ServicioCatalogo> _logger;

    public ServicioCatalogo(IGradoEstudiosDAO gradoEstudiosDAO, IGradoProfesionalDAO gradoProfesionalDAO, ILogger<ServicioCatalogo> logger)
    {
        _gradoEstudiosDAO = gradoEstudiosDAO;
        _gradoProfesionalDAO = gradoProfesionalDAO;
        _logger = logger;
    }

    public async Task<List<GradoEstudiosDTO>> ObtenerGradosEstudiosAsync()
    {
        _logger.LogInformation("Obteniendo todos los Grados de Estudio");
        var lista = await _gradoEstudiosDAO.ObtenerTodosAsync();
        verificarCatalogoGradoEstudio(lista);

        List<GradoEstudiosDTO> listaGradoEstudios = [];
        foreach (var grado in lista)
        {
            var gradoEstudio = new GradoEstudiosDTO
            {
                IdGradoEstudios = grado.IdGradoEstudios,
                Nombre = grado.Nombre
            };
            listaGradoEstudios.Add(gradoEstudio);
        }

        _logger.LogInformation("Se obtuvieron todos los Grados de Estudio");
        return listaGradoEstudios;
    }

    public async Task<List<GradoProfesionalDTO>> ObtenerGradosProfesionalesAsync()
    {
        _logger.LogInformation("Obteniendo todos los Grados Profesionales");
        var lista = await _gradoProfesionalDAO.ObtenerTodosAsync();
        verificarCatalogoGradoProfesional(lista);

        List<GradoProfesionalDTO> listaGradoProfesional = [];
        foreach (var grado in lista)
        {
            var gradoProfesional = new GradoProfesionalDTO
            {
                IdGradoProfesional = grado.IdGradoProfesional,
                Nombre = grado.Nombre
            };
            listaGradoProfesional.Add(gradoProfesional);
        }

        _logger.LogInformation("Se obtuvieron todos los Grados Profesionales");
        return listaGradoProfesional;
    }

    public async Task<GradoEstudiosDTO> ObtenerGradoEstudioPorIdAsync(int idGradoEstudios)
    {
        _logger.LogInformation("Buscando un Grado de Estudios");
        var gradoEstudio = await _gradoEstudiosDAO.ObtenerPorIdAsync(idGradoEstudios);
        verificarGradoEstudioNulo(gradoEstudio);

        var gradoRetorno = new GradoEstudiosDTO
        {
            IdGradoEstudios = gradoEstudio.IdGradoEstudios,
            Nombre = gradoEstudio.Nombre
        };

        _logger.LogInformation($"Grado de Estudio encontrado con la id {gradoEstudio.IdGradoEstudios}");
        return gradoRetorno;
    }

    public async Task<GradoProfesionalDTO> ObtenerGradoProfesionalPorIdAsync(int id)
    {
        _logger.LogInformation("Buscando un Grado Profesional");
        var gradoProfesional = await _gradoProfesionalDAO.ObtenerPorIdAsync(id);
        verificarGradoProfesionalNulo(gradoProfesional);

        var gradoRetorno = new GradoProfesionalDTO
        {
            IdGradoProfesional = gradoProfesional.IdGradoProfesional,
            Nombre = gradoProfesional.Nombre
        };

        _logger.LogInformation($"Grado Profesional encontrado con la id {gradoProfesional.IdGradoProfesional}");
        return gradoRetorno;
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