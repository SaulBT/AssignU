using ServicioUsuarios.Models;
using ServicioUsuarios.Services.Interfaces;
using ServicioUsuarios.Data.DTOs.Alumno;
using ServicioUsuarios.Data.DAOs.Interfaces;
using ServicioUsuarios.Config;
using ServicioUsuarios.Validations;
using ServicioUsuarios.Data.DTOs;
using ServicioUsuarios.Data.DTOs.RPC;

namespace ServicioUsuarios.Services.Implementation;

public class ServicioAlumno : IServicioAlumno
{
    private readonly IAlumnoDAO _alumnoDAO;
    private readonly RpcClientRabbitMQ _rpcClient;
    private readonly ILogger<ServicioAlumno> _logger;
    private readonly AlumnoValidaciones _validaciones;

    public ServicioAlumno(IAlumnoDAO alumnoDAO, RpcClientRabbitMQ rpcClient, ILogger<ServicioAlumno> logger, AlumnoValidaciones validaciones)
    {
        _alumnoDAO = alumnoDAO;
        _rpcClient = rpcClient;
        _logger = logger;
        _validaciones = validaciones;
    }

    public async Task<AlumnoDTO> RegistrarAsync(RegistrarAlumnoDTO registrarAlumnoDto)
    {
        _logger.LogInformation("Registrando a Alumno");
        await _validaciones.VerificarRegistroDeAlumnoAsync(registrarAlumnoDto);

        var alumnoNuevo = new Alumno
        {
            NombreCompleto = registrarAlumnoDto.NombreCompleto,
            NombreUsuario = registrarAlumnoDto.NombreUsuario,
            Contrasenia = registrarAlumnoDto.Contrasenia,
            Correo = registrarAlumnoDto.CorreoElectronico,
            IdGradoEstudios = registrarAlumnoDto.IdGradoEstudios
        };
        await _alumnoDAO.AgregarAlumnoAsync(alumnoNuevo);

        var retornoAlumno = new AlumnoDTO
        {
            NombreCompleto = registrarAlumnoDto.NombreCompleto,
            NombreUsuario = registrarAlumnoDto.NombreUsuario,
            Correo = registrarAlumnoDto.CorreoElectronico,
            IdGradoEstudios = registrarAlumnoDto.IdGradoEstudios
        };

        _logger.LogInformation($"Alumno registrado con éxito, id asignada: {retornoAlumno.IdAlumno}");
        return retornoAlumno;
    }

    public async Task<AlumnoDTO> ActualizarAsync(HttpContext httpContext, int idAlumno, ActualizarAlumnoDTO actualizarAlumnoDto)
    {
        _logger.LogInformation("Actualizando a alumno");
        var alumno = await _validaciones.VerificarActualizacionDeAlumnoAsync(httpContext, idAlumno, actualizarAlumnoDto);

        alumno.NombreCompleto = actualizarAlumnoDto.NombreCompleto;
        alumno.NombreUsuario = actualizarAlumnoDto.NombreUsuario;
        alumno.IdGradoEstudios = actualizarAlumnoDto.IdGradoEstudios;
        await _alumnoDAO.ActualizarAsync(alumno);
        var retornoAlumno = new AlumnoDTO
        {
            IdAlumno = alumno.IdAlumno,
            NombreCompleto = actualizarAlumnoDto.NombreCompleto,
            NombreUsuario = actualizarAlumnoDto.NombreUsuario,
            Correo = alumno.Correo,
            IdGradoEstudios = actualizarAlumnoDto.IdGradoEstudios
        };
        _logger.LogInformation($"Alumno actualizado con la id {retornoAlumno.IdAlumno}");
        return retornoAlumno;
    }

    public async Task EliminarAsync(HttpContext httpContext, int idAlumno)
    {
        _logger.LogInformation("Eliminando a Alumno");
        var alumno = await _validaciones.VerificarEliminarAlumnoAsync(httpContext, idAlumno);

        await _alumnoDAO.EliminarAsync(alumno);
        _logger.LogInformation($"Alumno eliminado con la id {alumno.IdAlumno}");
    }

    public async Task<AlumnoDTO?> ObtenerAlumnoPorIdAsync(int idAlumno)
    {
        _logger.LogInformation("Buscando a un Alumno");
        var alumnoObtenido = await _validaciones.VerificarObtencionDeAlumnoAsync(idAlumno);

        var retornoAlumno = new AlumnoDTO
        {
            IdAlumno = alumnoObtenido.IdAlumno,
            NombreCompleto = alumnoObtenido.NombreCompleto,
            NombreUsuario = alumnoObtenido.NombreUsuario,
            Correo = alumnoObtenido.Correo,
            IdGradoEstudios = (int)alumnoObtenido.IdGradoEstudios
        };

        _logger.LogInformation("Alumno encontrado");
        return retornoAlumno;
    }

    public async Task CambiarContraseniaAsync(CambiarContraseniaDTO cambiarContraseniaDto, int idAlumno, HttpContext httpContext)
    {
        _logger.LogInformation("Cambiando contraseña de Alumno");
        var alumno = await _validaciones.VerificarCambioContraseniaAsync(cambiarContraseniaDto, idAlumno, httpContext);

        alumno.Contrasenia = cambiarContraseniaDto.ContraseniaNueva;
        await _alumnoDAO.ActualizarAsync(alumno);
        _logger.LogInformation($"Contraseña cambiada para el Alumno con la id {alumno.IdAlumno}");
    }

    public async Task<RespuestaRPCDTO> ObtenerListaAlumnosAsync(List<int> idAlumnos)
    {
        _logger.LogInformation("Buscando alumnos de lista con ids");
        var respuesta = _validaciones.VerificarListaIdAlumnos(idAlumnos);
        var listaAlumnos = await generarListaDeAlumnosAsync(idAlumnos);

        respuesta.Alumnos = listaAlumnos;
        _logger.LogInformation("Se obtuvieron los datos de los Alumnos");
        return respuesta;
    }

    public async Task<EstadisticasPerfilDTO> ObtenerEstadisticasPerfilAlumnoAsync(HttpContext httpContext, int idAlumno)
    {
        _logger.LogInformation("Recopilando datos para estadísticas del perfil de Alumno");
        _validaciones.VerificarObtencionDeEstadisticasDeAlumno(httpContext, idAlumno);

        var datosPerfil = await ObtenerAlumnoPorIdAsync(idAlumno);
        
        string mensajeJson = crearMensajeRPC("obtenerClasesTareasYRespuesta", idAlumno);
        var resultadoClasesTareasRespuesta = await enviarMensajeRPCAsync(mensajeJson, "cola_clases");

        var estadistica = new EstadisticasPerfilDTO
        {
            IdAlumno = idAlumno,
            NombreUsuario = datosPerfil.NombreUsuario,
            NombreCompleto = datosPerfil.NombreCompleto,
            Correo = datosPerfil.Correo,
            idGradoEstudios = (int)datosPerfil.IdGradoEstudios,
            Clases = resultadoClasesTareasRespuesta.Clases
        };

        _logger.LogInformation($"Estadísticas del Alumno con id {idAlumno} generadas");
        return estadistica;
    }

    /*
    //Métodos privados
    */

    private string crearMensajeRPC(string accion, int idAlumno)
    {
        SolicitudRPCDTO solicitud = new SolicitudRPCDTO
        {
            Accion = accion,
            IdAlumno = idAlumno
        };
        return System.Text.Json.JsonSerializer.Serialize(solicitud);
    }

    private async Task<RespuestaRPCDTO> enviarMensajeRPCAsync(string mensajeJson, string cola)
    {
        string respuestaJson = await _rpcClient.CallAsync(cola, mensajeJson);
        var respuesta = System.Text.Json.JsonSerializer.Deserialize<RespuestaRPCDTO>(respuestaJson);

        if (respuesta == null)
        {
            throw new Exception("No hay respuesta");
        }
        else if (!respuesta.Success)
        {
            if (respuesta.Error == null)
            {
                throw new InvalidOperationException("No hay error");
            }
            throw LanzarExcepciones.lanzarExcepcion(respuesta.Error.Tipo, respuesta.Error.Mensaje);
        }

        return respuesta;
    }
    
    private async Task<List<AlumnoEstadisticasDTO>> generarListaDeAlumnosAsync(List<int> idAlumnos)
    {
        List<AlumnoEstadisticasDTO> listaAlumnos = new List<AlumnoEstadisticasDTO>();
        foreach (int idAlumno in idAlumnos)
        {
            Alumno alumno = await _validaciones.VerificarExistenciaAlumno(idAlumno);
            var alumnoInscrito = new AlumnoEstadisticasDTO
            {
                IdAlumno = alumno.IdAlumno,
                NombreCompleto = alumno.NombreCompleto,
            };
            listaAlumnos.Add(alumnoInscrito);
        }

        return listaAlumnos;
    } 
}