using ServicioClases.Config;
using ServicioClases.Data.DAOs.Interfaces;
using ServicioClases.Data.DTOs;
using ServicioClases.Data.DTOs.RPC;
using ServicioClases.Models;
using ServicioClases.Services.Interfaces;
using ServicioClases.Validations;

namespace ServicioClases.Services.Implementations;

public class ServicioClase : IServicioClase
{
    private readonly IClaseDAO _claseDAO;
    private readonly IRegistroDAO _registroDAO;
    private readonly RpcClientRabbitMQ _rpcClient;
    private readonly ClaseValidaciones _validacion;
    private readonly ILogger<ServicioClase> _logger;

    public ServicioClase(IClaseDAO claseDAO, IRegistroDAO registroDAO, RpcClientRabbitMQ rpcClient, ClaseValidaciones validacion, ILogger<ServicioClase> logger)
    {
        _claseDAO = claseDAO;
        _registroDAO = registroDAO;
        _rpcClient = rpcClient;
        _validacion = validacion;
        _logger = logger;
    }

    public async Task<ClaseDTO> CrearClaseAsync(CrearClaseDTO crearClaseDto, HttpContext httpContext)
    {
        _logger.LogInformation("Creando una Clase");
        var claseDto = await _validacion.VerificarCreacionDeClaseAsync(httpContext, crearClaseDto);

        var clase = new Clase
        {
            Nombre = crearClaseDto.Nombre,
            Codigo = claseDto.CodigoClase,
            IdDocente = claseDto.IdDocente
        };
        var claseCreada = await _claseDAO.CrearClaseAsync(clase, claseDto.CodigoClase);
        claseDto.IdClase = claseCreada.IdClase;

        _logger.LogInformation($"Clase creada con id: {claseDto.IdClase}");
        return claseDto;
    }

    public async Task<ClaseDTO> EditarClaseAsync(int idClase, ActualizarClaseDTO actualizarClaseDto, HttpContext httpContext)
    {
        _logger.LogInformation("Editando una Clase");
        var claseActualizada = await _validacion.VerificarEdicionDeClaseAsync(idClase, actualizarClaseDto, httpContext);
        await _claseDAO.ActualizarClaseAsync(claseActualizada);

        var claseDto = new ClaseDTO
        {
            IdClase = claseActualizada.IdClase,
            NombreClase = claseActualizada.Nombre,
            CodigoClase = claseActualizada.Codigo,
            IdDocente = claseActualizada.IdDocente
        };
        _logger.LogInformation($"Clase editada con la id: {claseDto.IdClase}");
        return claseDto;
    }

    public async Task EliminarClaseAsync(int idClase, HttpContext httpContext)
    {
        _logger.LogInformation("Eliminando Clase");
        var clase = await _validacion.VerificarEliminarClaseAsync(idClase, httpContext);
        var tieneRegistros = await _validacion.VerificarRegistrosClaseAsync(idClase);
        if (!tieneRegistros)
        {
            await BorrarDatosDeClaseAsync(clase);
        }
        else
        {
            await _validacion.BorrarDocenteDeClaseAsync(idClase);
        }

        _logger.LogInformation($"Clase eliminada con la id {idClase}");
    }

    public async Task EnviarFechaVisualizacionAsync(int idClase, DateTime fechaVisualizacion, HttpContext httpContext)
    {
        _logger.LogInformation("Enviando actualización de última conexión a Clase");
        var idAlumno = _validacion.VerificarActualizacionUltimaConexion(idClase, httpContext);

        var registro = await _registroDAO.ObtenerRegistroPorIdAlumnoYClaseAsync(idAlumno, idClase);
        registro.UltimoInicio = fechaVisualizacion;
        await _registroDAO.ActualizarRegistroAsync(registro);
        _logger.LogInformation($"Última conexión a la Clase {registro.IdClase} del Alumno {registro.IdAlumno}");
    }

    public async Task<ClaseDTO?> ObtenerClasePorIdAsync(int idClase)
    {
        _logger.LogInformation("Buscando Clase por id");
        var clase = await _validacion.VerificarObtencionDeClaseAsync(idClase);

        var claseDto = new ClaseDTO
        {
            IdClase = clase.IdClase,
            NombreClase = clase.Nombre,
            CodigoClase = clase.Codigo,
            IdDocente = clase.IdDocente
        };
        _logger.LogInformation($"Clase con id {claseDto.IdClase} encontrada");
        return claseDto;
    }

    public async Task<List<Clase>?> ObtenerClasesDeAlumnoAsync(int idAlumno)
    {
        _logger.LogInformation("Obteniendo Clases de Alumno");
        _validacion.VerificarIdUsuario(idAlumno);

        var registros = await _registroDAO.ObtenerRegistrosPorAlumnoAsync(idAlumno);
        var clases = await _claseDAO.ObtenerClasesDeAlumnoAsync(registros);

        _logger.LogInformation($"Clases del Alumno con id {idAlumno} recuperadas");
        return clases;
    }

    public Task<List<Clase>?> ObtenerClasesDeDocenteAsync(int idDocente)
    {
        _logger.LogInformation("Obteniendo Clases de Docente");
        _validacion.VerificarIdUsuario(idDocente);

        var clases = _claseDAO.ObtenerClasesDeDocenteAsync(idDocente);

        _logger.LogInformation($"Clases del Docente con id {idDocente} recuperadas");
        return clases;
    }

    public async Task<Clase> UnirseAClaseAsync(string codigoClase, HttpContext httpContext)
    {
        _logger.LogInformation("Uniendose a una Clase");
        var claseExistente = await _validacion.VerificarUnirseAClaseAsync(codigoClase, httpContext);
        var idAlumno = int.Parse(httpContext.User.FindFirst("idUsuario")!.Value);

        var registro = new Registro
        {
            IdClase = claseExistente.IdClase,
            IdAlumno = idAlumno
        };

        await _registroDAO.CrearRegistroAsync(registro);
        _logger.LogInformation($"Alumno con id {idAlumno} registrado en la Clase con id {registro.IdClase}");
        return claseExistente;
    }

    public async Task SalirDeClaseAsync(int idAlumno, int idClase, HttpContext httpContext)
    {
        _logger.LogInformation("Saliendose de una Clase");
        var registro = await _validacion.VerificarSalirseClaseAsync(idAlumno, idClase, httpContext);
        _logger.LogInformation($"Se borró el registro del Alumno con id {idAlumno} en la Clase con id {idClase}");
        await _registroDAO.EliminarRegistroAsync(registro);

        var tieneRegistros = await _validacion.VerificarRegistrosClaseAsync(idClase);
        var clase = await _validacion.BuscarClasePorIdAsync(idClase);
        var estaTerminada = _validacion.VerificarClaseTerminada(clase.IdDocente);

        if (!tieneRegistros && estaTerminada)
        {
            await BorrarDatosDeClaseAsync(clase);
        }

    }

    public async Task BorrarDatosDeClaseAsync(Clase clase)
    {
        _logger.LogInformation("Borrando datos de una Clase");
        await _claseDAO.EliminarClaseAsync(clase);
        string mensajeEliminarTareas = crearMensajeRPC("eliminarTareasDeClase", new List<int>(), clase.IdClase);
        await enviarMensajeRPCAsync(mensajeEliminarTareas, "cola_tareas");
        _logger.LogInformation($"Se borraron los datos de la Clase con id {clase.IdClase}");
    }

    public async Task<Registro> ObtenerRegistroAlumno(int idAlumno, int idClase, HttpContext httpContext)
    {
        _logger.LogInformation("Buscando Registro de un Alumno");
        _validacion.VerificarObtencionDeRegistroAsync(idAlumno, idClase, httpContext);

        var registro = await _registroDAO.ObtenerRegistroPorIdAlumnoYClaseAsync(idAlumno, idClase);
        _logger.LogInformation($"Registro con id {registro.IdRegistro} recuperado");
        return registro;
    }

    public async Task<EstadisticasClaseDTO> ObtenerEstadisticasDeLaClase(int idClase)
    {
        _validacion.VerificarIdClase(idClase);

        var registros = await _registroDAO.ObtenerRegistrosPorClaseAsync(idClase);
        List<int> idAlumnos = generarListaIdAlumnos(registros);

        string mensajeJsonAlumnos = crearMensajeRPC("obtenerListaAlumnos", idAlumnos, 0);
        var alumnos = (await enviarMensajeRPCAsync(mensajeJsonAlumnos, "cola_usuarios")).Alumnos;

        string mensajeJsonTareas = crearMensajeRPC("obtenerTareasYRespuestasDeClase", new List<int>(), idClase);
        var resultadoTareasRespuestas = await enviarMensajeRPCAsync(mensajeJsonTareas, "cola_tareas");
        var tareas = resultadoTareasRespuestas.Tareas;
        var respuestas = resultadoTareasRespuestas.Respuestas;

        var listaAlumnos = generarListaAlumnos(alumnos, respuestas, registros);

        EstadisticasClaseDTO estadisticas = new EstadisticasClaseDTO
        {
            IdClase = idClase,
            Alumnos = listaAlumnos,
            Tareas = tareas
        };

        return estadisticas;
    }

    public async Task<RespuestaRPCDTO> ObtenerClasesTareasRespuestasDeAlumnoAsync(int idAlumno)
    {
        try
        {
            var registros = await _registroDAO.ObtenerRegistrosPorAlumnoAsync(idAlumno);
            var clases = await _claseDAO.ObtenerClasesDeAlumnoAsync(registros);

            var listaClases = await generarListaClasesAsync(clases, registros, idAlumno);

            return new RespuestaRPCDTO
            {
                Success = true,
                Clases = listaClases
            };
        }
        catch (Exception ex)
        {
            return DetectorExcepciones.detectarExcepcion(ex);
        }
        
    }

    private string crearMensajeRPC(string accion, List<int> idAlumnos, int idClase)
    {
        SolicitudRPCDTO solicitud = new SolicitudRPCDTO
        {
            Accion = accion,
            IdAlumnos = idAlumnos,
            IdClase = idClase
        };
        return System.Text.Json.JsonSerializer.Serialize(solicitud);
    }

    private List<int> generarListaIdAlumnos(List<Registro> registros)
    {
        List<int> idAlumnos = new List<int>();
        foreach (var registro in registros)
        {
            idAlumnos.Add(registro.IdAlumno);
        }

        return idAlumnos;
    }

    private List<AlumnoEstadisticasClaseDTO> generarListaAlumnos(List<AlumnoEstadisticasDTO> alumnos, List<RespuestasEstadisticaClaseDTO> respuestas, List<Registro> registros)
    {
        List<AlumnoEstadisticasClaseDTO> listaAlumnos = new List<AlumnoEstadisticasClaseDTO>();
        foreach (var alumno in alumnos)
        {
            var listaRespuestas = generarListaRespuestas(respuestas, alumno);
            var ultimaConexion = obtenerUltimaConexion(registros, alumno.IdAlumno);
            var alumnoDto = new AlumnoEstadisticasClaseDTO
            {
                IdAlumno = alumno.IdAlumno,
                NombreComeplto = alumno.NombreCompleto,
                Respuestas = listaRespuestas,
                UltimaConexion = ultimaConexion
            };
            listaAlumnos.Add(alumnoDto);
        }

        return listaAlumnos;
    }

    private List<RespuestasEstadisticaClaseDTO> generarListaRespuestas(List<RespuestasEstadisticaClaseDTO> respuestas, AlumnoEstadisticasDTO alumno)
    {
        List<RespuestasEstadisticaClaseDTO> listaRespuestas = new List<RespuestasEstadisticaClaseDTO>();
        foreach (var respuesta in respuestas)
        {
            if (respuesta.IdAlumno == alumno.IdAlumno)
            {
                listaRespuestas.Add(respuesta);
            }
        }

        return listaRespuestas;
    }

    private List<TareaEstadisticaPerfilDTO> generarListaTareas(List<TareaEstadisticasClaseDTO> tareas, List<RespuestasEstadisticaClaseDTO> respuestas, int idAlumno)
    {
        List<TareaEstadisticaPerfilDTO> listaTareas = new List<TareaEstadisticaPerfilDTO>();
        foreach (var tarea in tareas)
        {
            foreach (var respuesta in respuestas)
            {
                if (respuesta.IdTarea == tarea.IdTarea && respuesta.IdAlumno == idAlumno)
                {
                    var tareaEnClase = new TareaEstadisticaPerfilDTO
                    {
                        IdTarea = tarea.IdTarea,
                        Nombre = tarea.Nombre,
                        Calificacion = respuesta.Calificacion
                    };

                    listaTareas.Add(tareaEnClase);
                }
            }

        }

        return listaTareas;
    }

    private DateTime obtenerUltimaConexion(List<Registro> registros, int id)
    {
        DateTime ultimaConexion = new DateTime();
        foreach (var registro in registros)
        {
            if (registro.IdAlumno == id)
            {
                ultimaConexion = registro.UltimoInicio ?? default(DateTime);
            }
        }

        return ultimaConexion;
    }

    private async Task<List<ClaseEstadisticaPerfilDTO>> generarListaClasesAsync(List<Clase> clases, List<Registro> registros, int idAlumno)
    {
        List<ClaseEstadisticaPerfilDTO> listaClases = new List<ClaseEstadisticaPerfilDTO>();
        foreach (var clase in clases)
        {
            int idClase = clase.IdClase;
            var ultimaConexion = obtenerUltimaConexion(registros, idClase);

            var claseInscrita = new ClaseEstadisticaPerfilDTO
            {
                IdClase = idClase,
                Nombre = clase.Nombre,
                UltimaConexion = ultimaConexion
            };

            string mensajeJsonTareas = crearMensajeRPC("obtenerTareasYRespuestasDeClase", new List<int>(), idClase);
            var resultadoTareasRespuestas = await enviarMensajeRPCAsync(mensajeJsonTareas, "cola_tareas");
            var tareas = resultadoTareasRespuestas.Tareas;
            var respuestas = resultadoTareasRespuestas.Respuestas;

            var listaTareas = generarListaTareas(tareas, respuestas, idAlumno);

            claseInscrita.Tareas = listaTareas;
            listaClases.Add(claseInscrita);
        }

        return listaClases;
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
}