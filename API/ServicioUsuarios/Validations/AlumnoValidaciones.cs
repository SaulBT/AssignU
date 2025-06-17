using ServicioUsuarios.Data.DAOs.Interfaces;
using ServicioUsuarios.Data.DTOs;
using ServicioUsuarios.Data.DTOs.Alumno;
using ServicioUsuarios.Data.DTOs.RPC;
using ServicioUsuarios.Models;
using ServicioUsuarios.Exceptions;

namespace ServicioUsuarios.Validations;

public class AlumnoValidaciones
{
    private readonly IAlumnoDAO _alumnoDAO;

    public AlumnoValidaciones(IAlumnoDAO alumnoDAO)
    {
        _alumnoDAO = alumnoDAO;
    }

    public async Task VerificarRegistroDeAlumnoAsync(RegistrarAlumnoDTO registrarAlumnoDto)
    {
        verificarParametrosAlumnoRegistro(registrarAlumnoDto);
        await verificarAlumnoNombreRegistroAsync(registrarAlumnoDto.NombreUsuario);
        await verificarAlumnoCorreoAsync(registrarAlumnoDto.CorreoElectronico);
    }

    public async Task<Alumno> VerificarActualizacionDeAlumnoAsync(HttpContext httpContext, ActualizarAlumnoDTO alumnoDto)
    {
        VerificarAutorizacion(httpContext);
        var idAlumno = int.Parse(httpContext.User.FindFirst("idUsuario")!.Value);
        verificarIdUsuario(idAlumno);
        verificarIgualdadId(idAlumno, alumnoDto.IdAlumno);
        verificarParametrosAlumnoActualizacion(alumnoDto);
        await verificarAlumnoNombreActualizacionAsync(alumnoDto.NombreUsuario, idAlumno);
        return await VerificarExistenciaAlumno(idAlumno);
    }

    public async Task<Alumno> VerificarEliminarAlumnoAsync(HttpContext httpContext)
    {
        VerificarAutorizacion(httpContext);
        var idAlumno = int.Parse(httpContext.User.FindFirst("idUsuario")!.Value);
        verificarIdUsuario(idAlumno);

        return await VerificarExistenciaAlumno(idAlumno);
    }

    public async Task<Alumno> VerificarObtencionDeAlumnoAsync(int idAlumno)
    {
        verificarIdUsuario(idAlumno);
        return await VerificarExistenciaAlumno(idAlumno);
    }

    public async Task<Alumno> VerificarCambioContraseniaAsync(CambiarContraseniaDTO cambiarContraseniaDto, HttpContext httpContext)
    {
        verificarParametrosCambiarContrasenia(cambiarContraseniaDto);
        VerificarAutorizacion(httpContext);
        var idAlumno = int.Parse(httpContext.User.FindFirst("idUsuario")!.Value);
        verificarIdUsuario(idAlumno);
        var alumno = await VerificarExistenciaAlumno(idAlumno);
        verificarContraseniaActual(alumno, cambiarContraseniaDto.ContraseniaActual);

        return alumno;
    }

    public async Task<Alumno> VerificarExistenciaAlumno(int idAlumno)
    {
        var alumnoObtenido = await _alumnoDAO.ObtenerAlumnoPorIdAsync(idAlumno);
        if (alumnoObtenido == null)
        {
            throw new RecursoNoEncontradoException($"El Alumno con la id {idAlumno} no existe.");
        }
        return alumnoObtenido;
    }

    public RespuestaRPCDTO VerificarListaIdAlumnos(List<int> idAlumnos)
    {
        var resultado = new RespuestaRPCDTO();

        resultado = verificarDatosListaIdAlumnos(idAlumnos);
        resultado = verificarIdsDeListaAlumnos(idAlumnos);

        return resultado;
    }

    public void VerificarAutorizacion(HttpContext context)
    {
        if (!context.User.Identity?.IsAuthenticated ?? true)
        {
            throw new UnauthorizedAccessException("El usuario no está autenticado.");
        }
    }

    private void verificarIdUsuario(int idUsuario)
    {
        if (idUsuario <= 0)
        {
            throw new IdInvalidaException($"La id del usuario no es válida: id {idUsuario}");
        }
    }

    private void verificarParametrosAlumnoRegistro(RegistrarAlumnoDTO alumnoDto)
    {
        if (string.IsNullOrEmpty(alumnoDto.NombreCompleto))
        {
            throw new CampoObligatorioException("Los parámetros de registro del alumno son inválidos: El nombre completo es nulo");
        }
        else if (string.IsNullOrEmpty(alumnoDto.NombreUsuario))
        {
            throw new CampoObligatorioException("Los parámetros de registro del alumno son inválidos: El nombre usuario es nulo");
        }
        else if (string.IsNullOrEmpty(alumnoDto.Contrasenia))
        {
            throw new CampoObligatorioException("Los parámetros de registro del alumno son inválidos: La contraseña es nula");
        }
        else if (string.IsNullOrEmpty(alumnoDto.CorreoElectronico))
        {
            throw new CampoObligatorioException("Los parámetros de registro del alumno son inválidos: El correo electrónico es nulo");
        }
        else if (alumnoDto.IdGradoEstudios <= 0)
        {
            throw new IdInvalidaException($"Los parámetros de registro del alumno son inválidos: Id del grado de estudios: {alumnoDto.IdGradoEstudios}");
        }
    }

    private void verificarParametrosAlumnoActualizacion(ActualizarAlumnoDTO alumnoDto)
    {
        if (string.IsNullOrEmpty(alumnoDto.NombreCompleto))
        {
            throw new CampoObligatorioException("Los parámetros de actualización del alumno son inválidos: El nombre completo es nulo");
        }
        else if (string.IsNullOrEmpty(alumnoDto.NombreUsuario))
        {
            throw new CampoObligatorioException("Los parámetros de actualización del alumno son inválidos: El nombre usuario es nulo");
        }
        else if (alumnoDto.IdGradoEstudios <= 0)
        {
            throw new IdInvalidaException($"Los parámetros de actualización del alumno son inválidos: Id del grado de estudios: {alumnoDto.IdGradoEstudios}");
        }
    }

    private void verificarParametrosCambiarContrasenia(CambiarContraseniaDTO cambiarContraseniaDto)
    {
        if (string.IsNullOrEmpty(cambiarContraseniaDto.ContraseniaNueva))
        {
            throw new CampoObligatorioException("Los parámetros de cambio de contraseña son inválidos: La contraseña nueva es nula");
        }
        else if (string.IsNullOrEmpty(cambiarContraseniaDto.ContraseniaActual))
        {
            throw new CampoObligatorioException("Los parámetros de cambio de contraseña son inválidos: La contraseña actual es nula");
        }
    }

    private RespuestaRPCDTO verificarDatosListaIdAlumnos(List<int> idAlumnos)
    {
        var resultado = new RespuestaRPCDTO
        {
            Success = true
        };

        if (idAlumnos == null)
        {
            resultado.Success = false;
            resultado.Error = new ErrorDTO
            {
                Tipo = "CampoObligatorioException",
                Mensaje = "La lista de idAlumnos es nula"
            };
        }

        return resultado;
    }

    private RespuestaRPCDTO verificarIdsDeListaAlumnos(List<int> idAlumnos)
    {
        try
        {
            foreach (int idAlumno in idAlumnos)
            {
                verificarIdUsuario(idAlumno);
            }

            return new RespuestaRPCDTO
            {
                Success = true
            };
        }
        catch (Exception ex)
        {
            return DetectorExcepciones.detectarExcepcion(ex);
        }
    }

    private async Task verificarAlumnoNombreRegistroAsync(string nombreUsuario)
    {
        var alumnoExistente = await _alumnoDAO.ObtenerPorNombreUsuarioAsync(nombreUsuario);
        if (alumnoExistente != null)
        {
            throw new RecursoYaExistenteException($"El nombre de usuario '{nombreUsuario}' ya está en uso por otro Alumno.");
        }
    }

    private async Task verificarAlumnoNombreActualizacionAsync(string nombreUsuario, int id)
    {
        var alumnoExistente = await _alumnoDAO.ObtenerPorNombreUsuarioEIdAsync(nombreUsuario, id);
        if (alumnoExistente != null)
        {
            throw new RecursoYaExistenteException($"El nombre de usuario '{nombreUsuario}' ya está en uso por otro alumno.");
        }
    }

    private void verificarIgualdadId(int id, int idAlumno)
    {
        if (id != idAlumno)
        {
            throw new DiscordanciaDeIdException("El ID del alumno en la URL no coincide con el ID del alumno en el cuerpo de la solicitud.");
        }
    }

    private async Task verificarAlumnoCorreoAsync(string correo)
    {
        var alumnoExistente = await _alumnoDAO.ObtenerPorCorreoAsync(correo);
        if (alumnoExistente != null)
        {
            throw new RecursoYaExistenteException($"El correo '{correo}' ya está en uso por otro alumno.");
        }
    }

    private void verificarContraseniaActual(Alumno alumno, string contraseniaActual)
    {
        if (alumno.Contrasenia != contraseniaActual)
        {
            throw new ContraseniaDiferenteException("La contraseña actual es incorrecta.");
        }
    }
}