using Microsoft.AspNetCore.Mvc;
using ServicioUsuarios.Data.Interfaces;
using ServicioUsuarios.Entities;

[ApiController]
[Route("api/[controller]")]
public class DocenteController : ControllerBase
{
    private readonly IDocenteDAO _docenteDAO;

    public DocenteController(IDocenteDAO docenteDAO)
    {
        _docenteDAO = docenteDAO;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<docente>> obtenerDocentePorId(int id)
    {
        try
        {
            var docente = await _docenteDAO.obtenerPorIdAsync(id);
            if (docente == null)
            {
                return NotFound();
            }
            return Ok(docente);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Error al obtener el docente: " + ex.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<docente>> registrarDocente([FromBody] docente docente)
    {
        try
        {
            if (docente == null)
            {
                return BadRequest("El docente no puede ser nulo.");
            }

            await _docenteDAO.registrarAsync(docente);
            return CreatedAtAction(nameof(obtenerDocentePorId), new { id = docente.idDocente }, docente);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Error al registrar el docente: " + ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> actualizarDocente(int id, [FromBody] docente docente)
    {
        try
        {
            if (id != docente.idDocente)
            {
                return BadRequest("El ID del docente no coincide.");
            }

            await _docenteDAO.actualizarAsync(docente);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Error al actualizar el docente: " + ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> eliminarDocente(int id)
    {
        try
        {
            await _docenteDAO.eliminarAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Error al eliminar el docente: " + ex.Message);
        }
    }
}