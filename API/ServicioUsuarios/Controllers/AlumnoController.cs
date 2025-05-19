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
    public async Task<ActionResult<alumno>> registrarAlumno([FromBody] RegistrarAlumnoDTO alumnoDto)
    {
        try
        {
            if (alumnoDto == null)
            {
                return BadRequest("El alumno no puede ser nulo.");
            }

            var alumno = new alumno
            {
                nombreCompleto = alumnoDto.nombreCompleto,
                nombreUsuario = alumnoDto.nombreUsuario,
                contrasenia = alumnoDto.contrasenia,
                correo = alumnoDto.correo,
                idGradoEstudios = alumnoDto.idGradoEstudios
            };

            await _alumnoDAO.registrarAsync(alumno);
            return CreatedAtAction(nameof(obtenerAlumnoPorId), new { id = alumno.idAlumno }, alumno);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Error al registrar el alumno: " + ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> actualizarAlumno(int id, [FromBody] ActualizarAlumnoDTO alumnoDto)
    {
        try
        {
            if (id != alumnoDto.idAlumno)
            {
                return BadRequest("El ID del alumno no coincide.");
            }

            var alumno = new alumno
            {
                idAlumno = id,
                nombreCompleto = alumnoDto.nombreCompleto,
                nombreUsuario = alumnoDto.nombreUsuario,
                idGradoEstudios = alumnoDto.idGradoEstudios
            };

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