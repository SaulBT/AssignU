using Microsoft.AspNetCore.Mvc;
using ServicioUsuarios.Entities;

[ApiController]
[Route("api/[controller]")]
public class AlumnoController : ControllerBase
{
    private readonly IAlumnoDAO _alumnoDAO;

    public AlumnoController(IAlumnoDAO alumnoDAO)
    {
        _alumnoDAO = alumnoDAO;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<alumno>> obtenerAlumnoPorId(int id)
    {
        try
        {
            var alumno = await _alumnoDAO.obtenerPorIdAsync(id);
            if (alumno == null)
            {
                return NotFound();
            }
            return Ok(alumno);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Error al obtener el alumno: " + ex.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<alumno>> registrarAlumno([FromBody] alumno alumno)
    {
        try
        {
            if (alumno == null)
            {
                return BadRequest("El alumno no puede ser nulo.");
            }

            await _alumnoDAO.registrarAsync(alumno);
            return CreatedAtAction(nameof(obtenerAlumnoPorId), new { id = alumno.idAlumno }, alumno);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Error al registrar el alumno: " + ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> actualizarAlumno(int id, [FromBody] alumno alumno)
    {
        try
        {
            if (id != alumno.idAlumno)
            {
                return BadRequest("El ID del alumno no coincide.");
            }

            await _alumnoDAO.actualizarAsync(alumno);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Error al actualizar el alumno: " + ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> eliminarAlumno(int id)
    {
        try
        {
            await _alumnoDAO.eliminarAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Error al eliminar el alumno: " + ex.Message);
        }
    }
}