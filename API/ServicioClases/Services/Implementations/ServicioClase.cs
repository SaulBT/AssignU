using ServicioClases.Config;
using ServicioClases.Data.DAOs.Interfaces;
using ServicioClases.Data.DTOs;
using ServicioClases.Exceptions;
using ServicioClases.Models;
using ServicioClases.Services.Interfaces;
using ServicioClases.Validations;

namespace ServicioClases.Services.Implementations;

public class ServicioClase : IServicioClase
{
    private readonly IClaseDAO _claseDAO;
    private readonly IRegistroDAO _registroDAO;
    private readonly RpcClientRabbitMQ _rpcClient;

    public ServicioClase(IClaseDAO claseDAO, IRegistroDAO registroDAO, RpcClientRabbitMQ rpcClient)
    {
        _claseDAO = claseDAO;
        _registroDAO = registroDAO;
        _rpcClient = rpcClient;
    }

    public async Task<Clase> CrearClaseAsync(CrearClaseDTO crearClaseDto, HttpContext httpContext)
    {
        verificarAutorizacion(httpContext);
        var idDocente = int.Parse(httpContext.User.FindFirst("idUsuario")!.Value);
        verificarIdUsuario(idDocente);
        verificarCamposCrearClase(crearClaseDto);
        var codigoClase = await asignarCodigoClaseAsync();

        var clase = new Clase
        {
            Nombre = crearClaseDto.Nombre,
            Codigo = codigoClase,
            IdDocente = idDocente
        };
        var claseCreada = await _claseDAO.CrearClaseAsync(clase, codigoClase);

        return claseCreada;
    }

    public async Task<Clase> EditarClaseAsync(ActualizarClaseDTO actualizarClaseDto, HttpContext httpContext)
    {
        verificarAutorizacion(httpContext);
        var idDocente = int.Parse(httpContext.User.FindFirst("idUsuario")!.Value);
        verificarIdUsuario(idDocente);
        verificarIdClase(actualizarClaseDto.IdClase);
        verificarCamposActualizarClase(actualizarClaseDto);

        var claseExistente = await buscarClasePorIdAsync(actualizarClaseDto.IdClase);
        claseExistente.Nombre = actualizarClaseDto.Nombre;
        await _claseDAO.ActualizarClaseAsync(claseExistente);

        return claseExistente;
    }

    public async Task EliminarClaseAsync(int idClase, HttpContext httpContext)
    {
        verificarAutorizacion(httpContext);
        var idDocente = int.Parse(httpContext.User.FindFirst("idUsuario")!.Value);
        verificarIdUsuario(idDocente);
        verificarIdClase(idClase);

        var tieneRegistros = await verificarRegistrosClaseAsync(idClase);
        if (!tieneRegistros)
        {
            var clase = await buscarClasePorIdAsync(idClase);
            await _claseDAO.EliminarClaseAsync(clase);
        }
        else
        {
            await borrarDocenteDeClaseAsync(idClase);
        }
    }

    public async Task EnviarFechaVisualizacionAsync(int idClase, DateTime fechaVisualizacion, HttpContext httpContext)
    {
        verificarAutorizacion(httpContext);
        var idAlumno = int.Parse(httpContext.User.FindFirst("idUsuario")!.Value);
        verificarIdUsuario(idAlumno);
        verificarIdClase(idClase);

        var registro = await _registroDAO.ObtenerRegistroPorIdAlumnoYClaseAsync(idAlumno, idClase);
        registro.UltimoInicio = fechaVisualizacion;
        await _registroDAO.ActualizarRegistroAsync(registro);
    }

    public async Task<Clase?> ObtenerClasePorIdAsync(int idClase)
    {
        verificarIdClase(idClase);

        var clase = await buscarClasePorIdAsync(idClase);
        return clase;
    }

    public async Task<List<Clase>?> ObtenerClasesDeAlumnoAsync(HttpContext httpContext)
    {
        verificarAutorizacion(httpContext);
        var idAlumno = int.Parse(httpContext.User.FindFirst("idUsuario")!.Value);
        verificarIdUsuario(idAlumno);

        var registros = await _registroDAO.ObtenerRegistrosPorAlumnoAsync(idAlumno);
        var clases = await _claseDAO.ObtenerClasesDeAlumnoAsync(registros);

        return clases;
    }

    public Task<List<Clase>?> ObtenerClasesDeDocenteAsync(HttpContext httpContext)
    {
        verificarAutorizacion(httpContext);
        var idDocente = int.Parse(httpContext.User.FindFirst("idUsuario")!.Value);
        verificarIdUsuario(idDocente);

        var clases = _claseDAO.ObtenerClasesDeDocenteAsync(idDocente);

        return clases;
    }

    public async Task<Clase> UnirseAClaseAsync(string codigoClase, HttpContext httpContext)
    {
        verificarAutorizacion(httpContext);
        var idAlumno = int.Parse(httpContext.User.FindFirst("idUsuario")!.Value);
        verificarIdUsuario(idAlumno);
        verificarCodigoClase(codigoClase);
        var claseExistente = await buscarClaseConCodigoAsync(codigoClase);
        verificarClaseConDocente((int)(claseExistente?.IdDocente));

        var registro = new Registro
        {
            IdClase = claseExistente.IdClase,
            IdAlumno = idAlumno
        };

        await _registroDAO.CrearRegistroAsync(registro);
        return claseExistente;
    }

    public async Task SalirDeClaseAsync(int idClase, HttpContext httpContext)
    {
        
        verificarAutorizacion(httpContext);
        var idAlumno = int.Parse(httpContext.User.FindFirst("idUsuario")!.Value);
        verificarIdUsuario(idAlumno);
        verificarIdClase(idClase);
        Console.WriteLine("Bucsa el registro.");
        var registro = await buscarRegistroPorIdAlumnoYClaseAsync(idAlumno, idClase);

        Console.WriteLine("Lo encuentra. Id: " + registro.IdRegistro);
        Console.WriteLine("Lo elimina.");
        await _registroDAO.EliminarRegistroAsync(registro);
        Console.WriteLine("Verifica si la clase tiene otro registro.");
        var tieneRegistros = await verificarRegistrosClaseAsync(idClase);
        Console.WriteLine("¿Tiene registros? " + tieneRegistros);
        Console.WriteLine("Busca la clase.");
        var clase = await buscarClasePorIdAsync(idClase);
        Console.WriteLine("La encuentra. Id: " + clase.IdClase);
        Console.WriteLine("Verifica si el docente es 0.");
        
        var estaTerminada = verificarClaseTerminada(clase.IdDocente);
        Console.WriteLine("¿El docente es 0? " + estaTerminada);
        
        if (!tieneRegistros && estaTerminada)
        {
            Console.WriteLine("Borra la clase.");
            await _claseDAO.EliminarClaseAsync(clase);
        }
        
    }

    public async Task<Registro> ObtenerRegistroAlumno(int idAlumno, int idClase, HttpContext httpContext)
    {
        verificarAutorizacion(httpContext);
        var idUsuario = int.Parse(httpContext.User.FindFirst("idUsuario")!.Value);
        verificarIdUsuario(idUsuario);
        verificarIdAlumno(idAlumno);
        verificarIdClase(idClase);

        var registro = await _registroDAO.ObtenerRegistroPorIdAlumnoYClaseAsync(idAlumno, idClase);
        return registro;
    }

    public async Task<EstadisticasClaseDTO> ObtenerEstadisticasDeLaClase(int idClase)
    {
        verificarIdClase(idClase);
        //Obtener todos los registros de la clase.
        var registros = await _registroDAO.ObtenerRegistrosPorClaseAsync(idClase);
        List<int> idAlumnos = new List<int>();
        foreach (var registro in registros)
        {
            idAlumnos.Add(registro.IdAlumno);
        }
        //Pedir la idAlumno y nombre completo de cada alumno inscrito al servicio de alumnos
        string mensajeJsonAlumnos = crearMensajeRPC("obtenerAlumnosDeClase", idAlumnos, 0);
        var alumnos = (await enviarMensajeRPCAsync(mensajeJsonAlumnos, "cola_clases_usuarios")).Alumnos;
        //Pedir todas las tareas y sus respuestas al servicio de tareas.
        string mensajeJsonTareas = crearMensajeRPC("obtenerTareasYRespuestasDeClase", new List<int>(), idClase);
        var resultadoTareasRespuestas = await enviarMensajeRPCAsync(mensajeJsonTareas, "cola_clases_tareas");
        var tareas = resultadoTareasRespuestas.Tareas;
        var respuestas = resultadoTareasRespuestas.Respuestas;
        //Pedir los resultados de todas las tareas de cada alumno inscrito en la clase al servicio de tareas.
        //Crear el DTO y enviarlo
        List<AlumnoEstadisticasClaseDTO> listaAlumnos = new List<AlumnoEstadisticasClaseDTO>();
        foreach (var alumno in alumnos) {
            List<RespuestasEstadisticaClaseDTO> listaRespuestas = new List<RespuestasEstadisticaClaseDTO>();
            foreach (var respuesta in respuestas) {
                Console.WriteLine("idAlumno en alumno = " + alumno.IdAlumno);
                Console.WriteLine("idAlumno en respuesta = " + respuesta.IdAlumno);
                if (respuesta.IdAlumno == alumno.IdAlumno)
                {
                    listaRespuestas.Add(respuesta);
                }
            }
            
            DateTime ultimaConexion = new DateTime();
            foreach (var registro in registros) {
                if (registro.IdAlumno == alumno.IdAlumno)
                {
                    ultimaConexion = registro.UltimoInicio ?? default(DateTime);
                }
            }

            var alumnoDto = new AlumnoEstadisticasClaseDTO
            {
                IdAlumno = alumno.IdAlumno,
                NombreComeplto = alumno.NombreCompleto,
                Respuestas = listaRespuestas,
                UltimaConexion = ultimaConexion
            };
            listaAlumnos.Add(alumnoDto);
        }
        
        EstadisticasClaseDTO estadisticas = new EstadisticasClaseDTO
        {
            IdClase = idClase,
            Alumnos = listaAlumnos,
            Tareas = tareas
        };

        return estadisticas;
    }

    private void verificarAutorizacion(HttpContext httpContext)
    {
        if (!httpContext.User.Identity?.IsAuthenticated ?? true)
        {
            throw new UnauthorizedAccessException("Sin autorización para continuar.");
        }
    }

    private void verificarIdUsuario(int idUsuario)
    {
        if (idUsuario <= 0)
        {
            throw new ArgumentException("La id del usuario no es válida");
        }
    }

    private void verificarIdClase(int idClase)
    {
        if (idClase <= 0)
        {
            throw new ArgumentException("La id de la clase no es válida");
        }
    }

    private void verificarIdAlumno(int idAlumno)
    {
        if (idAlumno <= 0)
        {
            throw new ArgumentException("La id del alumno no es válida");
        }
    }

    private void verificarCamposCrearClase(CrearClaseDTO crearClaseDto)
    {
        if (string.IsNullOrWhiteSpace(crearClaseDto.Nombre))
        {
            throw new ArgumentException("Los datos para crear la clase son inválidos");
        }
    }

    private void verificarCamposActualizarClase(ActualizarClaseDTO actualizarClaseDto)
    {
        if (string.IsNullOrWhiteSpace(actualizarClaseDto.Nombre))
        {
            throw new ArgumentException("Los datos para actualizar la clase son inválidos");
        }
    }

    private void verificarCodigoClase(string codigoClase)
    {
        if (string.IsNullOrWhiteSpace(codigoClase) || codigoClase.Length != 6)
        {
            throw new ArgumentException("El código de la clase es inválido");
        }
    }

    private void verificarClaseConDocente(int idDocente)
    {
        if (idDocente == 0)
        {
            throw new ClaseTerminadaException("Esta clase ya no acepta a más alumnos");
        }
    }

    private bool verificarClaseTerminada(int idDocente)
    {
        if (idDocente == 0)
        {
            return true;
        }
        return false;
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

    private async Task<string> asignarCodigoClaseAsync()
    {
        var claseExistente = new Clase();
        var codigo = string.Empty;
        do
        {
            var caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            codigo = new string(Enumerable.Range(0, 6)
                .Select(_ => caracteres[random.Next(caracteres.Length)])
                .ToArray());

            claseExistente = await buscarClaseConCodigoVerificacionAsync(codigo);
        } while (claseExistente != null);

        return codigo;
    }

    private async Task<Clase?> buscarClaseConCodigoVerificacionAsync(string codigoClase)
    {
        return await _claseDAO.ObtenerClasePorCodigoAsync(codigoClase);
    }

    private async Task<Clase?> buscarClaseConCodigoAsync(string codigoClase)
    {
        var clase = await _claseDAO.ObtenerClasePorCodigoAsync(codigoClase);
        if (clase == null)
        {
            throw new RecursoNoEncontradoException("No se encontró ninguna clase");
        }
        return clase;
    }

    private async Task<Clase?> buscarClasePorIdAsync(int idClase)
    {
        var clase = await _claseDAO.ObtenerClasePorIdAsync(idClase);
        if (clase == null)
        {
            throw new RecursoNoEncontradoException("No se encontró ninguna clase");
        }
        return clase;
    }

    private async Task<bool> verificarRegistrosClaseAsync(int idClase)
    {
        var registros = await _registroDAO.ObtenerRegistrosPorClaseAsync(idClase);
        if (registros.Count > 0)
        {
            return true;
        }
        return false;
    }

    private async Task borrarDocenteDeClaseAsync(int idClase)
    {
        var clase = await buscarClasePorIdAsync(idClase);
        clase.IdDocente = 0;
        await _claseDAO.ActualizarClaseAsync(clase);
    }

    private async Task<Registro?> buscarRegistroPorIdAlumnoYClaseAsync(int idAlumno, int idClase)
    {
        var registro = await _registroDAO.ObtenerRegistroPorIdAlumnoYClaseAsync(idAlumno, idClase);
        if (registro == null)
        {
            throw new RecursoNoEncontradoException($"No se encontró ningún registro a la clase con id {idClase}");
        }
        return registro;
    }
}