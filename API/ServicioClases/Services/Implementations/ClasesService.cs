using ServicioClases.Data.DAOs.Interfaces;
using ServicioClases.Data.DTOs;
using ServicioClases.Exceptions;
using ServicioClases.Models;
using ServicioClases.Services.Interfaces;

namespace ServicioClases.Services.Implementations;

public class ClasesService : IClasesService
{
    private readonly IClaseDAO _claseDAO;
    private readonly IRegistroDAO _registroDAO;

    public ClasesService(IClaseDAO claseDAO, IRegistroDAO registroDAO)
    {
        _claseDAO = claseDAO;
        _registroDAO = registroDAO;
    }

    public async Task<Clase> crearClaseAsync(CrearClaseDTO crearClaseDto, HttpContext httpContext)
    {
        verificarAutorizacion(httpContext);
        var idDocente = int.Parse(httpContext.User.FindFirst("idUsuario")!.Value);
        verificarIdUsuario(idDocente);
        verificarCamposCrearClase(crearClaseDto);
        var codigoClase = await asignarCodigoClaseAsync();
        var clase = await _claseDAO.crearClaseAsync(crearClaseDto, codigoClase);

        return clase;
    }

    public async Task<Clase> editarClase(ActualizarClaseDTO actualizarClaseDto, HttpContext httpContext)
    {
        verificarAutorizacion(httpContext);
        var idDocente = int.Parse(httpContext.User.FindFirst("idUsuario")!.Value);
        verificarIdUsuario(idDocente);
        verificarIdClase(actualizarClaseDto.idClase);
        verificarCamposActualizarClase(actualizarClaseDto);

        var claseExistente = await buscarClasePorId(actualizarClaseDto.idClase);
        claseExistente.Nombre = actualizarClaseDto.nombre;
        await _claseDAO.actualizarClaseAsync(claseExistente);

        return claseExistente;
    }

    public async Task eliminarClase(int idClase, HttpContext httpContext)
    {
        verificarAutorizacion(httpContext);
        var idDocente = int.Parse(httpContext.User.FindFirst("idUsuario")!.Value);
        verificarIdUsuario(idDocente);
        verificarIdClase(idClase);

        var tieneRegistros = await verificarRegistrosClase(idClase);
        if (!tieneRegistros)
        {
            await _claseDAO.eliminarClaseAsync(idClase);
        }
        else
        {
            await borrarDocenteDeClase(idClase);
        }
    }

    public async Task enviarFechaVisualizacion(int idClase, DateTime fechaVisualizacion, HttpContext httpContext)
    {
        verificarAutorizacion(httpContext);
        var idAlumno = int.Parse(httpContext.User.FindFirst("idUsuario")!.Value);
        verificarIdUsuario(idAlumno);

        var registro = await _registroDAO.obtenerRegistroPorIdAlumnoYClaseAsync(idAlumno, idClase);
        registro.UltimoInicio = fechaVisualizacion;
        await _registroDAO.actualizarRegistroAsync(registro);
    }

    public async Task<Clase?> obtenerClasePorId(int idClase, HttpContext httpContext)
    {
        verificarAutorizacion(httpContext);
        var idUsuario = int.Parse(httpContext.User.FindFirst("idUsuario")!.Value);
        verificarIdUsuario(idUsuario);
        verificarIdClase(idClase);

        var clase = await buscarClasePorId(idClase);
        return clase;
    }

    public async Task<List<Clase>?> obtenerClasesDeAlumno(HttpContext httpContext)
    {
        verificarAutorizacion(httpContext);
        var idAlumno = int.Parse(httpContext.User.FindFirst("idUsuario")!.Value);
        verificarIdUsuario(idAlumno);

        var registros = await _registroDAO.obtenerRegistrosPorAlumnoAsync(idAlumno);
        var clases = await _claseDAO.obtenerClasesDeAlumnoAsync(registros);

        return clases;
    }

    public Task<List<Clase>?> obtenerClasesDeDocente(HttpContext httpContext)
    {
        verificarAutorizacion(httpContext);
        var idDocente = int.Parse(httpContext.User.FindFirst("idUsuario")!.Value);
        verificarIdUsuario(idDocente);

        var clases = _claseDAO.obtenerClasesDeDocenteAsync(idDocente);

        return clases;
    }

    public async Task<Clase> unirseAClase(string codigoClase, HttpContext httpContext)
    {
        verificarAutorizacion(httpContext);
        var idAlumno = int.Parse(httpContext.User.FindFirst("idUsuario")!.Value);
        verificarIdUsuario(idAlumno);
        verificarCodigoClase(codigoClase);
        var claseExistente = await buscarClaseConCodigo(codigoClase);
        verificarClaseConDocente((int)(claseExistente?.IdDocente));

        var registro = new Registro
        {
            IdClase = claseExistente.IdClase,
            IdAlumno = idAlumno
        };

        await _registroDAO.crearRegistroAsync(registro);
        return claseExistente;
    }

    public async Task salirDeClase(int idClase, HttpContext httpContext)
    {
        verificarAutorizacion(httpContext);
        var idAlumno = int.Parse(httpContext.User.FindFirst("idUsuario")!.Value);
        verificarIdUsuario(idAlumno);
        verificarIdClase(idClase);
        var registro = await buscarRegistroPorIdAlumnoYClase(idAlumno, idClase);

        _registroDAO.eliminarRegistroAsync(registro.IdRegistro);
        var tieneRegistros = await verificarRegistrosClase(idClase);
        if (!tieneRegistros)
        {
            await _claseDAO.eliminarClaseAsync(idClase);
        }
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

    private void verificarCamposCrearClase(CrearClaseDTO crearClaseDto)
    {
        if (string.IsNullOrWhiteSpace(crearClaseDto.nombre))
        {
            throw new ArgumentException("Los datos para crear la clase son inválidos");
        }
    }

    private void verificarCamposActualizarClase(ActualizarClaseDTO actualizarClaseDto)
    {
        if (string.IsNullOrWhiteSpace(actualizarClaseDto.nombre))
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

            claseExistente = await buscarClaseConCodigo(codigo);
        } while (claseExistente != null);

        return codigo;
    }

    private async Task<Clase?> buscarClaseConCodigo(string codigoClase)
    {
        var clase = await _claseDAO.obtenerClasePorCodigoAsync(codigoClase);
        return clase;
    }

    private async Task<Clase?> buscarClasePorId(int idClase)
    {
        var clase = await _claseDAO.obtenerClasePorCodigoAsync(idClase.ToString());
        if (clase == null)
        {
            throw new RecursoNoEncontradoException("No se encontró ninguna clase");
        }
        return clase;
    }

    private async Task<bool> verificarRegistrosClase(int idClase)
    {
        var registros = await _registroDAO.obtenerRegistrosPorClaseAsync(idClase);
        if (registros.Count > 0)
        {
            return true;
        }
        return false;
    }

    private async Task borrarDocenteDeClase(int idClase)
    {
        var clase = await buscarClasePorId(idClase);
        clase.IdDocente = 0;
        await _claseDAO.actualizarClaseAsync(clase);
    }

    private async Task<Registro?> buscarRegistroPorIdAlumnoYClase(int idAlumno, int idClase)
    {
        var registro = await _registroDAO.obtenerRegistroPorIdAlumnoYClaseAsync(idAlumno, idClase);
        if (registro == null)
        {
            throw new RecursoNoEncontradoException($"No se encontró ningún registro a la clase con id {idClase}");
        }
        return registro;
    }
}