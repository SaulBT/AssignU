using Microsoft.AspNetCore.Mvc;
using ServicioUsuarios.Entities;

[ApiController]
[Route("api/[controller]")]
public class CatalogosController : ControllerBase
{
    private readonly IGradoEstudiosDAO _gradoEstudiosDAO;
    private readonly IGradoProfesionalDAO _gradoProfesionalDAO;

    public CatalogosController(IGradoEstudiosDAO gradoEstudiosDAO, IGradoProfesionalDAO gradoProfesionalDAO)
    {
        _gradoEstudiosDAO = gradoEstudiosDAO;
        _gradoProfesionalDAO = gradoProfesionalDAO;
    }

    [HttpGet("grados_estudios")]
    public async Task<ActionResult<List<grado_estudio>>> obtenerGradosEstudios()
    {
        try
        {
            return Ok(await _gradoEstudiosDAO.obtenerTodosAsync());
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Error al obtener los grados de estudios: " + ex.Message);
        }
    }

    [HttpGet("grados_profesionales")]
    public async Task<ActionResult<List<grado_profesional>>> obtenerGradosProfesionales()
    {
        try
        {
            return Ok(await _gradoProfesionalDAO.obtenerTodosAsync());
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Error al obtener los grados profesionales: " + ex.Message);
        }
    }
}