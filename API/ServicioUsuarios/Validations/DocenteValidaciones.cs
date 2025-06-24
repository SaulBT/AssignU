using ServicioUsuarios.Data.DAOs.Interfaces;
using ServicioUsuarios.Data.DTOs;
using ServicioUsuarios.Data.DTOs.Docente;
using ServicioUsuarios.Exceptions;
using ServicioUsuarios.Models;

namespace ServicioUsuarios.Validations;

public class DocenteValidaciones
{
    private readonly IDocenteDAO _docenteDAO;

    public DocenteValidaciones(IDocenteDAO docenteDAO)
    {
        _docenteDAO = docenteDAO;
    }

    public async Task VerificarRegistroDocenteAsync(RegistrarDocenteDTO registrarDocenteDto)
    {
        verificarParametrosDocenteRegistro(registrarDocenteDto);
        await verificarDocenteNombreRegistroAsync(registrarDocenteDto.NombreUsuario);
        await verificarDocenteCorreoAsync(registrarDocenteDto.CorreoElectronico);
    }

    public async Task<Docente> VerificarActualizacionDeDocenteAsync(HttpContext httpContext, int idDocente, ActualizarDocenteDTO docenteDto)
    {
        verificarAutorizacion(httpContext);
        var idDocenteContexto = int.Parse(httpContext.User.FindFirst("idUsuario")!.Value);
        verificarIgualdadId(idDocente, idDocenteContexto);
        verificarParametrosDocenteActualizacion(docenteDto);
        await verificarDocenteNombreActualizacionAsync(docenteDto.NombreUsuario, idDocente);

        return await verificarExistenciaDocenteAsync(idDocente);
    }

    public async Task<Docente> VerificarEliminacionDeDocenteAsync(HttpContext httpContext, int idDocente)
    {
        verificarAutorizacion(httpContext);
        var idDocenteContexto = int.Parse(httpContext.User.FindFirst("idUsuario")!.Value);
        verificarIdValida(idDocente);
        verificarIdValida(idDocenteContexto);
        verificarIgualdadId(idDocente, idDocenteContexto);
        return await verificarExistenciaDocenteAsync(idDocente);
    }

    public async Task<Docente> VerificarObtencionDeDocenteAsync(int idDocente)
    {
        verificarIdValida(idDocente);
        return await verificarExistenciaDocenteAsync(idDocente);
    }

    public async Task<Docente> VerificarCambioDeContraseniaAsync(CambiarContraseniaDTO cambiarContraseniaDTO, int idDocente, HttpContext httpContext)
    {
        verificarParametrosCambiarContrasenia(cambiarContraseniaDTO);
        verificarAutorizacion(httpContext);
        var idDocenteContexto = int.Parse(httpContext.User.FindFirst("idUsuario")!.Value);
        verificarIdValida(idDocenteContexto);
        verificarIdValida(idDocente);
        verificarIgualdadId(idDocente, idDocenteContexto);
        var docente = await verificarExistenciaDocenteAsync(idDocente);
        verificarContraseniaActual(docente, cambiarContraseniaDTO.ContraseniaActual);

        return docente;
    }
    
    private void verificarIdValida(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("El ID del docente debe ser mayor que cero.", nameof(id));
        }
    }

    private void verificarParametrosDocenteRegistro(RegistrarDocenteDTO docenteDto)
    {
        if (string.IsNullOrEmpty(docenteDto.NombreCompleto))
        {
            throw new CampoObligatorioException("Los parámetros de registro del docente son inválidos: El nombre completo es nulo");
        }
        else if (string.IsNullOrEmpty(docenteDto.NombreUsuario))
        {
            throw new CampoObligatorioException("Los parámetros de registro del docente son inválidos: El nombre usuario es nulo");
        }
        else if (string.IsNullOrEmpty(docenteDto.Contrasenia))
        {
            throw new CampoObligatorioException("Los parámetros de registro del docente son inválidos: La contraseña es nula");
        }
        else if (string.IsNullOrEmpty(docenteDto.CorreoElectronico))
        {
            throw new CampoObligatorioException("Los parámetros de registro del docente son inválidos: El correo electrónico es nulo");
        }
        else if (docenteDto.IdGradoProfesional <= 0)
        {
            throw new IdInvalidaException($"Los parámetros de registro del docente son inválidos: Id del grado de estudios: {docenteDto.IdGradoProfesional}");
        }
    }

    private void verificarParametrosDocenteActualizacion(ActualizarDocenteDTO docenteDto)
    {
        if (docenteDto.NombreUsuario == "" ||
            docenteDto.NombreCompleto == "" ||
            docenteDto.IdGradoProfesional <= 0)
        {
            throw new ArgumentException("Los parámetros del docente son inválidos.");
        }
    }

    private void verificarParametrosCambiarContrasenia(CambiarContraseniaDTO cambiarContraseniaDto)
    {
        if (string.IsNullOrEmpty(cambiarContraseniaDto.ContraseniaActual) ||
            string.IsNullOrEmpty(cambiarContraseniaDto.ContraseniaNueva))
        {
            throw new ArgumentException("Los parámetros para cambiar la contraseña son inválidos.");
        }
    }

    private async Task<Docente> verificarExistenciaDocenteAsync(int idDocente)
    {
        var docente = await _docenteDAO.ObtenerDocentePorIdAsync(idDocente);
        if (docente == null)
        {
            throw new RecursoNoEncontradoException("El docente no existe.");
        }

        return docente;
    }

    private async Task verificarDocenteNombreRegistroAsync(string nombreUsuario)
    {
        var docenteExistente = await _docenteDAO.ObtenerDocentePorNombreUsuarioAsync(nombreUsuario);
        if (docenteExistente != null)
        {
            throw new RecursoYaExistenteException($"El nombre de usuario '{nombreUsuario}' ya está en uso por otro docente.");
        }
    }

    private async Task verificarDocenteNombreActualizacionAsync(string nombreUsuario, int id)
    {
        var docenteExistente = await _docenteDAO.ObtenerDocentePorNombreUsuarioEIdAsync(nombreUsuario, id);
        if (docenteExistente != null)
        {
            throw new RecursoYaExistenteException($"El nombre de usuario '{nombreUsuario}' ya está en uso por otro docente.");
        }
    }

    private void verificarIgualdadId(int id, int idDocente)
    {
        if (id != idDocente)
        {
            throw new DiscordanciaDeIdException("El ID del docente no coincide con el ID proporcionado.");
        }
    }

    private async Task verificarDocenteCorreoAsync(string correo)
    {
        var docenteExistente = await _docenteDAO.ObtenerDocentePorCorreoAsync(correo);
        if (docenteExistente != null)
        {
            throw new RecursoYaExistenteException($"El correo '{correo}' ya está en uso por otro docente.");
        }
    }

    private void verificarAutorizacion(HttpContext context)
    {
        if (!context.User.Identity?.IsAuthenticated ?? true)
        {
            throw new UnauthorizedAccessException("El usuario no está autenticado.");
        }
    }

    private void verificarContraseniaActual(Docente docente, string contraseniaActual)
    {
        if (docente.Contrasenia != contraseniaActual)
        {
            throw new ContraseniaDiferenteException("La contraseña actual es incorrecta.");
        }
    }
}