using Microsoft.AspNetCore.Mvc;
using ServicioUsuarios.Entities;
using ServicioUsuarios.DAOs;
using ServicioUsuarios.Services.Interfaces;
using ServicioUsuarios.Exceptions;

namespace ServicioUsuarios.Services.Implementation;

public class CatalogosService : ICatalogosService
{
    private readonly GradoEstudiosDAO _gradoEstudiosDAO;
    private readonly GradoProfesionalDAO _gradoProfesionalDAO;

    public CatalogosService(GradoEstudiosDAO gradoEstudiosDAO, GradoProfesionalDAO gradoProfesionalDAO)
    {
        _gradoEstudiosDAO = gradoEstudiosDAO;
        _gradoProfesionalDAO = gradoProfesionalDAO;
    }

    public async Task<List<grado_estudio>> obtenerGradosEstudiosAsync()
    {
        var lista = await _gradoEstudiosDAO.obtenerTodosAsync();
        verificarCatalogoGradoEstudio(lista);

        return lista;
    }

    public async Task<List<grado_profesional>> obtenerGradosProfesionalesAsync()
    {
        var lista = await _gradoProfesionalDAO.obtenerTodosAsync();
        verificarCatalogoGradoProfesional(lista);

        return lista;
    }

    public async Task<grado_estudio> obtenerGradoEstudioPorIdAsync(int id)
    {
        var gradoEstudio = await _gradoEstudiosDAO.obtenerPorIdAsync(id);
        verificarGradoEstudioNulo(gradoEstudio);

        return gradoEstudio;
    }

    public async Task<grado_profesional> obtenerGradoProfesionalPorIdAsync(int id)
    {
        var gradoProfesional = await _gradoProfesionalDAO.obtenerPorIdAsync(id);
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