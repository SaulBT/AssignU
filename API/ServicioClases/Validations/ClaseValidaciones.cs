using ServicioClases.Data.DAOs.Interfaces;
using ServicioClases.Data.DTOs;
using ServicioClases.Exceptions;
using ServicioClases.Models;

namespace ServicioClases.Validations;

public class ClaseValidaciones
{
    private readonly IClaseDAO _claseDAO;
    private readonly IRegistroDAO _registroDAO;

    public ClaseValidaciones(IClaseDAO claseDAO, IRegistroDAO registroDAO)
    {
        _claseDAO = claseDAO;
        _registroDAO = registroDAO;
    }

    public async Task<ClaseDTO> VerificarCreacionDeClaseAsync(HttpContext httpContext, CrearClaseDTO crearClaseDto)
    {
        verificarAutorizacion(httpContext);
        var idDocente = int.Parse(httpContext.User.FindFirst("idUsuario")!.Value);
        VerificarIdUsuario(idDocente);
        verificarCamposCrearClase(crearClaseDto);
        var codigoClase = await asignarCodigoClaseAsync();

        return new ClaseDTO
        {
            NombreClase = crearClaseDto.Nombre,
            CodigoClase = codigoClase,
            IdDocente = idDocente
        };
    }

    public async Task<Clase> VerificarEdicionDeClaseAsync(int idClase, ActualizarClaseDTO actualizarClaseDto, HttpContext httpContext)
    {
        verificarAutorizacion(httpContext);
        var idDocente = int.Parse(httpContext.User.FindFirst("idUsuario")!.Value);
        VerificarIdUsuario(idDocente);
        VerificarIdClase(idClase);
        verificarCamposActualizarClase(actualizarClaseDto);
        var claseExistente = await BuscarClasePorIdAsync(idClase);
        claseExistente.Nombre = actualizarClaseDto.Nombre;

        return claseExistente;
    }

    public async Task<Clase> VerificarEliminarClaseAsync(int idClase, HttpContext httpContext)
    {
        verificarAutorizacion(httpContext);
        var idDocente = int.Parse(httpContext.User.FindFirst("idUsuario")!.Value);
        VerificarIdUsuario(idDocente);
        VerificarIdClase(idClase);
        return await BuscarClasePorIdAsync(idClase);
    }

    public int VerificarActualizacionUltimaConexion(int idClase, HttpContext httpContext)
    {
        verificarAutorizacion(httpContext);
        var idAlumno = int.Parse(httpContext.User.FindFirst("idUsuario")!.Value);
        VerificarIdUsuario(idAlumno);
        VerificarIdClase(idClase);

        return idAlumno;
    }

    public async Task<Clase> VerificarObtencionDeClaseAsync(int idClase)
    {
        VerificarIdClase(idClase);
        return await BuscarClasePorIdAsync(idClase);
    }

    public int VerificarObtencionClasesAsync(HttpContext httpContext)
    {
        verificarAutorizacion(httpContext);
        var idUsuario = int.Parse(httpContext.User.FindFirst("idUsuario")!.Value);
        VerificarIdUsuario(idUsuario);

        return idUsuario;
    }

    public async Task<Clase> VerificarUnirseAClaseAsync(string codigoClase, HttpContext httpContext)
    {
        verificarAutorizacion(httpContext);
        var idAlumno = int.Parse(httpContext.User.FindFirst("idUsuario")!.Value);
        VerificarIdUsuario(idAlumno);
        verificarCodigoClase(codigoClase);
        var claseExistente = await buscarClaseConCodigoAsync(codigoClase);
        verificarClaseConDocente((int)(claseExistente?.IdDocente));

        return claseExistente;
    }

    public async Task<Registro> VerificarSalirseClaseAsync(int idAlumno, int idClase, HttpContext httpContext)
    {
        verificarAutorizacion(httpContext);
        VerificarIdUsuario(idAlumno);
        VerificarIdClase(idClase);
        return await buscarRegistroPorIdAlumnoYClaseAsync(idAlumno, idClase);
    }

    public void VerificarObtencionDeRegistroAsync(int idAlumno, int idClase, HttpContext httpContext)
    {
        verificarAutorizacion(httpContext);
        var idUsuario = int.Parse(httpContext.User.FindFirst("idUsuario")!.Value);
        VerificarIdUsuario(idUsuario);
        verificarIdAlumno(idAlumno);
        VerificarIdClase(idClase);
    }

    public void VerificarIdUsuario(int idUsuario)
    {
        if (idUsuario <= 0)
        {
            throw new IdInvalidaException("La id del usuario no es válida: idUsuario " + idUsuario);
        }
    }

    public void VerificarIdClase(int idClase)
    {
        if (idClase <= 0)
        {
            throw new IdInvalidaException($"La id de la clase no es válida: idClase {idClase}");
        }
    }

    public async Task<Clase?> BuscarClasePorIdAsync(int idClase)
    {
        var clase = await _claseDAO.ObtenerClasePorIdAsync(idClase);
        if (clase == null)
        {
            throw new RecursoNoEncontradoException("No se encontró ninguna clase");
        }
        return clase;
    }

    public async Task<bool> VerificarRegistrosClaseAsync(int idClase)
    {
        var registros = await _registroDAO.ObtenerRegistrosPorClaseAsync(idClase);
        if (registros.Count > 0)
        {
            return true;
        }
        return false;
    }

    public async Task BorrarDocenteDeClaseAsync(int idClase)
    {
        var clase = await BuscarClasePorIdAsync(idClase);
        clase.IdDocente = 0;
        await _claseDAO.ActualizarClaseAsync(clase);
    }

    public bool VerificarClaseTerminada(int idDocente)
    {
        if (idDocente == 0)
        {
            return true;
        }
        return false;
    }

    private void verificarAutorizacion(HttpContext httpContext)
    {
        if (!httpContext.User.Identity?.IsAuthenticated ?? true)
        {
            throw new UnauthorizedAccessException("Sin autorización para continuar.");
        }
    }

    private void verificarIdAlumno(int idAlumno)
    {
        if (idAlumno <= 0)
        {
            throw new IdInvalidaException($"La id del alumno no es válida: idAlumno: {idAlumno}");
        }
    }

    private void verificarCamposCrearClase(CrearClaseDTO crearClaseDto)
    {
        if (string.IsNullOrWhiteSpace(crearClaseDto.Nombre))
        {
            throw new CampoObligatorioException("Los datos para crear la clase son inválidos: el nombre es nulo");
        }
    }

    private void verificarCamposActualizarClase(ActualizarClaseDTO actualizarClaseDto)
    {
        if (string.IsNullOrWhiteSpace(actualizarClaseDto.Nombre))
        {
            throw new CampoObligatorioException("Los datos para actualizar la clase son inválidos: el nombre es nulo");
        }
    }

    private void verificarCodigoClase(string codigoClase)
    {
        if (string.IsNullOrWhiteSpace(codigoClase) || codigoClase.Length != 6)
        {
            throw new ArgumentException("El código de la clase es inválido");
        }

        if (string.IsNullOrWhiteSpace(codigoClase))
        {
            throw new CampoObligatorioException("El código de la clase no es válido: el código es nulo");
        }
        else if (codigoClase.Length != 6)
        {
            throw new CodigoClaseInvalidoException($"El código de la clase no es válido: el código es menor a 6 dígitos: {codigoClase}");
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