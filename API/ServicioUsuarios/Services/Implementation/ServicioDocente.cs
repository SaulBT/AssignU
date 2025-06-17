using ServicioUsuarios.Data.DAOs.Interfaces;
using ServicioUsuarios.Data.DTOs;
using ServicioUsuarios.Models;
using ServicioUsuarios.Services.Interfaces;
using ServicioUsuarios.Data.DTOs.Docente;
using ServicioUsuarios.Validations;

namespace ServicioUsuarios.Services.Implementation;

public class SerivicioDocente : IServicioDocente
{
    private readonly IDocenteDAO _docenteDAO;
    private readonly DocenteValidaciones _validaciones;
    private readonly ILogger _logger;

    public SerivicioDocente(IDocenteDAO docenteDAO, DocenteValidaciones validaciones, ILogger logger)
    {
        _docenteDAO = docenteDAO;
        _validaciones = validaciones;
        _logger = logger;
    }

    public async Task<DocenteDTO> RegistrarAsync(RegistrarDocenteDTO registrarDocenteDto)
    {
        _logger.LogInformation("Registrando a Docente");
        await _validaciones.VerificarRegistroDocenteAsync(registrarDocenteDto);

        var nuevoDocente = new Docente
        {
            NombreCompleto = registrarDocenteDto.NombreCompleto,
            NombreUsuario = registrarDocenteDto.NombreUsuario,
            Contrasenia = registrarDocenteDto.Contrasenia,
            Correo = registrarDocenteDto.CorreoElectronico,
            IdGradoProfesional = registrarDocenteDto.IdGradoProfesional
        };
        await _docenteDAO.AgregarDocenteAsync(nuevoDocente);

        var retornoDocente = new DocenteDTO
        {
            IdDocente = nuevoDocente.IdDocente,
            NombreCompleto = nuevoDocente.NombreCompleto,
            NombreUsuario = nuevoDocente.NombreUsuario,
            Correo = nuevoDocente.Correo,
            IdGradoProfesional = (int)nuevoDocente.IdGradoProfesional
        };

        _logger.LogInformation($"Se registró al Docente con la id asignada {retornoDocente.IdDocente}");
        return retornoDocente;
    }

    public async Task<DocenteDTO> ActualizarAsync(HttpContext httpContext, ActualizarDocenteDTO actualizarDocenteDto)
    {
        _logger.LogInformation("Se actualiza un Docente");
        var docente = await _validaciones.VerificarActualizacionDeDocenteAsync(httpContext, actualizarDocenteDto);

        docente.NombreCompleto = actualizarDocenteDto.NombreCompleto;
        docente.NombreUsuario = actualizarDocenteDto.NombreUsuario;
        docente.IdGradoProfesional = actualizarDocenteDto.IdGradoProfesional;
        await _docenteDAO.ActualizarAsync(docente);

        var retornoDocente = new DocenteDTO
        {
            IdDocente = docente.IdDocente,
            NombreCompleto = docente.NombreCompleto,
            NombreUsuario = docente.NombreUsuario,
            Correo = docente.Correo,
            IdGradoProfesional = (int)docente.IdGradoProfesional
        };
        _logger.LogInformation($"Se actualizó al Docente con la id {retornoDocente.IdDocente}");
        return retornoDocente;
    }

    public async Task EliminarAsync(HttpContext httpContexto)
    {
        _logger.LogInformation("Eliminando a un Docente");
        var docente = await _validaciones.VerificarEliminacionDeDocenteAsync(httpContexto);
        await _docenteDAO.EliminarAsync(docente);
        _logger.LogInformation($"Se eliminó al Docente con la id {docente.IdDocente}");
    }

    public async Task<DocenteDTO> ObtenerDocentePorIdAsync(int idDocente)
    {
        _logger.LogInformation("Buscando a un Docente");
        var docente = await _validaciones.VerificarObtencionDeDocenteAsync(idDocente);

        var retornoDocente = new DocenteDTO
        {
            IdDocente = docente.IdDocente,
            NombreCompleto = docente.NombreCompleto,
            NombreUsuario = docente.NombreUsuario,
            Correo = docente.Correo,
            IdGradoProfesional = (int)docente.IdGradoProfesional
        };
        _logger.LogInformation($"Docente encontrado con la id: {retornoDocente.IdDocente}");
        return retornoDocente;
    }

    public async Task CambiarContraseniaAsync(CambiarContraseniaDTO cambiarContraseniaDto, HttpContext httpContext)
    {
        _logger.LogInformation("Cambiando la contraseña de un Docente");
        var docente = await _validaciones.VerificarCambioDeContraseniaAsync(cambiarContraseniaDto, httpContext);

        docente.Contrasenia = cambiarContraseniaDto.ContraseniaNueva;
        _logger.LogInformation($"Contraseña de un Docente cambiada con la id {docente.IdDocente}");
        await _docenteDAO.ActualizarAsync(docente);
    }
}