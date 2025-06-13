using ServicioUsuarios.DAOs.Interfaces;
using ServicioUsuarios.DTOs;
using ServicioUsuarios.Entities;
using ServicioUsuarios.Exceptions;
using ServicioUsuarios.Services.Interfaces;

namespace ServicioUsuarios.Services.Implementation;

public class SerivicioDocente : IServicioDocente
{
    private readonly IDocenteDAO _docenteDAO;

    public SerivicioDocente(IDocenteDAO docenteDAO)
    {
        _docenteDAO = docenteDAO;
    }

    public async Task<DocenteDTO> ObtenerPorIdAsync(int id)
    {
        verificarIdValida(id);
        var docente = await _docenteDAO.ObtenerPorIdDtoAsync(id);
        verificarExistenciaDocente(docente);
        return docente;
    }

    public async Task<docente> RegistrarAsync(RegistrarDocenteDTO docenteDto)
    {
        verificarParametrosDocenteRegistro(docenteDto);
        await verificarDocenteNombreRegistroAsync(docenteDto.nombreUsuario);
        await verificarDocenteCorreoAsync(docenteDto.correoElectronico);

        var nuevoDocente = new docente
        {
            nombreCompleto = docenteDto.nombreCompleto,
            nombreUsuario = docenteDto.nombreUsuario,
            contrasenia = docenteDto.contrasenia,
            correo = docenteDto.correoElectronico,
            idGradoProfesional = docenteDto.idGradoProfesional
        };

        await _docenteDAO.AgregarDocenteAsync(nuevoDocente);

        return nuevoDocente;
    }

    public async Task ActualizarAsync(HttpContext contexto, ActualizarDocenteDTO docenteDto)
    {
        verificarAutorizacion(contexto);
        var idDocente = int.Parse(contexto.User.FindFirst("idUsuario")!.Value);
        verificarIgualdadId(idDocente, docenteDto.idDocente);
        verificarParametrosDocenteActualizacion(docenteDto);
        await verificarDocenteNombreActualizacionAsync(docenteDto.nombreUsuario, idDocente);

        var docente = await _docenteDAO.ObtenerPorIdNormalAsync(idDocente);
        docente.nombreCompleto = docenteDto.nombreCompleto;
        docente.nombreUsuario = docenteDto.nombreUsuario;
        docente.idGradoProfesional = docenteDto.idGradoProfesional;

        await _docenteDAO.ActualizarAsync(docente);
    }

    public async Task EliminarAsync(HttpContext contexto)
    {
        verificarAutorizacion(contexto);
        var idDocente = int.Parse(contexto.User.FindFirst("idUsuario")!.Value);
        verificarIdValida(idDocente);
        var docente = await _docenteDAO.ObtenerPorIdDtoAsync(idDocente);
        verificarExistenciaDocente(docente);
        await _docenteDAO.EliminarAsync(idDocente);
    }

    public async Task CambiarContraseniaAsync(CambiarContraseniaDTO cambiarContraseniaDTO, HttpContext context)
    {
        verificarParametrosCambiarContrasenia(cambiarContraseniaDTO);
        verificarAutorizacion(context);

        var idDocente = int.Parse(context.User.FindFirst("idUsuario")!.Value);
        var docente = await ObtenerDocentePorIdAsync(idDocente);
        verificarContraseniaActual(docente, cambiarContraseniaDTO.contraseniaActual);

        docente.contrasenia = cambiarContraseniaDTO.contraseniaNueva;
        await _docenteDAO.ActualizarAsync(docente);
    }

    private void verificarIdValida(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("El ID del docente debe ser mayor que cero.", nameof(id));
        }
    }

    private void verificarExistenciaDocente(DocenteDTO docente)
    {
        if (docente == null)
        {
            throw new RecursoNoEncontradoException("El docente no existe.");
        }
    }

    private void verificarParametrosDocenteRegistro(RegistrarDocenteDTO docenteDto)
    {
        if (docenteDto.nombreUsuario == "" ||
            docenteDto.nombreCompleto == "" ||
            docenteDto.contrasenia == "" ||
            docenteDto.correoElectronico == "" ||
            docenteDto.idGradoProfesional <= 0)
        {
            throw new ArgumentException("Los parámetros del docente son inválidos.");
        }
    }

    private void verificarParametrosDocenteActualizacion(ActualizarDocenteDTO docenteDto)
    {
        if (docenteDto.nombreUsuario == "" ||
            docenteDto.nombreCompleto == "" ||
            docenteDto.idGradoProfesional <= 0)
        {
            throw new ArgumentException("Los parámetros del docente son inválidos.");
        }
    }

    private void verificarParametrosCambiarContrasenia(CambiarContraseniaDTO cambiarContraseniaDto)
    {
        if (string.IsNullOrEmpty(cambiarContraseniaDto.contraseniaActual) ||
            string.IsNullOrEmpty(cambiarContraseniaDto.contraseniaNueva))
        {
            throw new ArgumentException("Los parámetros para cambiar la contraseña son inválidos.");
        }
    }

    private async Task verificarDocenteNombreRegistroAsync(string nombreUsuario)
    {
        var docenteExistente = await _docenteDAO.ObtenerPorNombreUsuarioAsync(nombreUsuario);
        if (docenteExistente != null)
        {
            throw new RecursoYaExistenteException($"El nombre de usuario '{nombreUsuario}' ya está en uso por otro docente.");
        }
    }

    private async Task verificarDocenteNombreActualizacionAsync(string nombreUsuario, int id)
    {
        var docenteExistente = await _docenteDAO.ObtenerPorNombreUsuarioEIdAsync(nombreUsuario, id);
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
        var docenteExistente = await _docenteDAO.ObtenerPorCorreoAsync(correo);
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

    private void verificarContraseniaActual(docente docente, string contraseniaActual)
    {
        if (docente.contrasenia != contraseniaActual)
        {
            throw new ContraseniaDiferenteException("La contraseña actual es incorrecta.");
        }
    }

    private async Task<docente> ObtenerDocentePorIdAsync(int id)
    {
        var alumno = await _docenteDAO.ObtenerPorIdNormalAsync(id);
        if (alumno == null)
        {
            throw new RecursoNoEncontradoException($"El docente con ID {id} no existe.");
        }
        return alumno;
    }
}