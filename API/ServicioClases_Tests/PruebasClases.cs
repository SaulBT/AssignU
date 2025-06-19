using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ServicioClases.Data;
using ServicioClases.Data.DAOs.Implementations;
using ServicioClases.Data.DTOs;
using ServicioClases.Exceptions;
using ServicioClases.Models;
using ServicioClases.Services.Implementations;

namespace ServicioClases_Tests;

public class PruebasClases
{
    private readonly ClasesDbContext _contexto;
    private readonly ClaseDAO _claseDAO;
    private readonly RegistroDAO _registroDAO;
    private readonly ClasesService _servicio;
    private HttpContext _httpContext;

    public PruebasClases()
    {
        var opciones = new DbContextOptionsBuilder<ClasesDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _contexto = new ClasesDbContext(opciones);

        _claseDAO = new ClaseDAO(_contexto);
        _registroDAO = new RegistroDAO(_contexto);
        _servicio = new ClasesService(_claseDAO, _registroDAO);
    }

    [Fact]
    public async Task CrearClaseAsync()
    {
        var claseNueva = new CrearClaseDTO { nombre = "Química Orgánica" };

        _httpContext = obtenerHttpContext(1);

        var resultado = await _servicio.crearClaseAsync(claseNueva, _httpContext);
        var claseCreada = await _claseDAO.obtenerClasePorIdAsync(resultado.IdClase);

        Assert.NotNull(resultado);
        Assert.Equal("Química Orgánica", claseCreada.Nombre);
    }

    [Fact]
    public async Task CrearClaseSinAutorizacionAsync()
    {
        var claseNueva = new CrearClaseDTO { nombre = "Geografía de México y del Mundo" };

        _httpContext = obtenerHttpContextInvalido();

        var resultado = await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _servicio.crearClaseAsync(claseNueva, _httpContext));
        Assert.Equal("Sin autorización para continuar.", resultado.Message);
    }

    [Fact]
    public async Task CrearClaseCamposVaciosNulosAsync()
    {
        var claseNueva = new CrearClaseDTO { nombre = "" };

        _httpContext = obtenerHttpContext(55);

        var resultado = await Assert.ThrowsAsync<ArgumentException>(
            () => _servicio.crearClaseAsync(claseNueva, _httpContext));
        Assert.Equal("Los datos para crear la clase son inválidos", resultado.Message);
    }

    [Fact]
    public async Task CrearClaseIdUsuarioCeroAsync()
    {
        var claseNueva = new CrearClaseDTO { nombre = "Ciencias de la Tierra" };


        _httpContext = obtenerHttpContextInvalido();

        var resultado = await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _servicio.crearClaseAsync(claseNueva, _httpContext));
        Assert.Equal("Sin autorización para continuar.", resultado.Message);
    }

    [Fact]
    public async Task EditarClaseAsync()
    {
        _contexto.Clase.Add(new Clase
        {
            IdClase = 3,
            Codigo = "J22LI8",
            Nombre = "Historia de México",
            IdDocente = 1
        });
        await _contexto.SaveChangesAsync();

        var clase = new ActualizarClaseDTO
        {
            idClase = 3,
            nombre = "Historia de México II"
        };

        _httpContext = obtenerHttpContext(1);

        var resultado = await _servicio.editarClaseAsync(clase, _httpContext);
        var claseActualizada = await _claseDAO.obtenerClasePorIdAsync(resultado.IdClase);

        Assert.NotNull(resultado);
        Assert.Equal(clase.nombre, claseActualizada.Nombre);
    }

    [Fact]
    public async Task EditarClaseSinAutorizacionAync()
    {
        var clase = new ActualizarClaseDTO
        {
            idClase = 3,
            nombre = "Historia de México II"
        };

        _httpContext = obtenerHttpContextInvalido();

        var resultado = await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _servicio.editarClaseAsync(clase, _httpContext));
        Assert.Equal("Sin autorización para continuar.", resultado.Message);
    }

    [Fact]
    public async Task EditarClaseNoExistenteAsync()
    {
        var clase = new ActualizarClaseDTO
        {
            idClase = 3,
            nombre = "Historia de México II"
        };

        _httpContext = obtenerHttpContext(1);

        var resultado = await Assert.ThrowsAsync<RecursoNoEncontradoException>(
            () => _servicio.editarClaseAsync(clase, _httpContext));
        Assert.Equal("No se encontró ninguna clase", resultado.Message);
    }

    [Fact]
    public async Task EditarClaseCamposVaciosAsync()
    {
        var clase = new ActualizarClaseDTO
        {
            idClase = 3,
            nombre = ""
        };

        _httpContext = obtenerHttpContext(1);

        var resultado = await Assert.ThrowsAsync<ArgumentException>(
            () => _servicio.editarClaseAsync(clase, _httpContext));
        Assert.Equal("Los datos para actualizar la clase son inválidos", resultado.Message);
    }

    [Fact]
    public async Task EditarClaseIdClaseCeroAsync()
    {
        var clase = new ActualizarClaseDTO
        {
            idClase = 0,
            nombre = "Historia de México II"
        };

        _httpContext = obtenerHttpContext(1);

        var resultado = await Assert.ThrowsAsync<ArgumentException>(
            () => _servicio.editarClaseAsync(clase, _httpContext));
        Assert.Equal("La id de la clase no es válida", resultado.Message);
    }

    [Fact]
    public async Task EditarClaseIdUsuarioCeroAsync()
    {
        var clase = new ActualizarClaseDTO
        {
            idClase = 3,
            nombre = "Historia de México II"
        };

        _httpContext = obtenerHttpContext(0);

        var resultado = await Assert.ThrowsAsync<ArgumentException>(
            () => _servicio.editarClaseAsync(clase, _httpContext));
        Assert.Equal("La id del usuario no es válida", resultado.Message);
    }

    [Fact]
    public async Task EliminarClaseConRegistroAsync()
    {
        _contexto.Clase.Add(new Clase
        {
            IdClase = 3,
            Codigo = "J22LI8",
            Nombre = "Historia de México",
            IdDocente = 1
        });
        _contexto.Registro.Add(new Registro
        {
            IdRegistro = 51,
            IdAlumno = 26,
            IdClase = 3,
            UltimoInicio = new DateTime()
        });
        await _contexto.SaveChangesAsync();

        _httpContext = obtenerHttpContext(1);

        await _servicio.eliminarClaseAsync(3, _httpContext);
        var claseEliminada = await _claseDAO.obtenerClasePorIdAsync(3);

        Assert.NotNull(claseEliminada);
        Assert.Equal(0, claseEliminada.IdDocente);
    }

    [Fact]
    public async Task EliminarClaseSinRegistroAsync()
    {
        _contexto.Clase.Add(new Clase
        {
            IdClase = 3,
            Codigo = "J22LI8",
            Nombre = "Historia de México",
            IdDocente = 1
        });
        await _contexto.SaveChangesAsync();

        _httpContext = obtenerHttpContext(1);

        await _servicio.eliminarClaseAsync(3, _httpContext);
        var claseEliminada = await _claseDAO.obtenerClasePorIdAsync(3);

        Assert.Null(claseEliminada);
    }

    [Fact]
    public async Task EliminarClaseSinAutorizacionAsync()
    {
        _httpContext = obtenerHttpContextInvalido();

        var resultado = await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _servicio.eliminarClaseAsync(109, _httpContext));
        Assert.Equal("Sin autorización para continuar.", resultado.Message);
    }

    [Fact]
    public async Task EliminarClaseIdUsuarioCeroAsync()
    {
        _httpContext = obtenerHttpContext(0);

        var resultado = await Assert.ThrowsAsync<ArgumentException>(
            () => _servicio.eliminarClaseAsync(2, _httpContext));
        Assert.Equal("La id del usuario no es válida", resultado.Message);
    }

    [Fact]
    public async Task EliminarClaseInexistenteAsync()
    {
        _httpContext = obtenerHttpContext(1);

        var resultado = await Assert.ThrowsAsync<RecursoNoEncontradoException>(
            () => _servicio.eliminarClaseAsync(3, _httpContext));
        Assert.Equal("No se encontró ninguna clase", resultado.Message);
    }

    [Fact]
    public async Task EliminarClaseIdClaseCeroAsync()
    {
        _httpContext = obtenerHttpContext(1);

        var resultado = await Assert.ThrowsAsync<ArgumentException>(
            () => _servicio.eliminarClaseAsync(0, _httpContext));
        Assert.Equal("La id de la clase no es válida", resultado.Message);
    }

    [Fact]
    public async Task ObtenerClasePorIdAsync()
    {
        _contexto.Clase.Add(new Clase
        {
            IdClase = 3,
            Codigo = "J22LI8",
            Nombre = "Historia de México",
            IdDocente = 1
        });
        await _contexto.SaveChangesAsync();

        _httpContext = obtenerHttpContext(1);

        var resultado = await _servicio.obtenerClasePorIdAsync(3);

        Assert.NotNull(resultado);
        Assert.Equal(3, resultado.IdClase);
    }

    [Fact]
    public async Task ObtenerClaseIdClaseCeroAsync()
    {
        _httpContext = obtenerHttpContext(1);

        var resultado = await Assert.ThrowsAsync<ArgumentException>(
            () => _servicio.obtenerClasePorIdAsync(0));
        Assert.Equal("La id de la clase no es válida", resultado.Message);
    }

    [Fact]
    public async Task ObtenerClaseInexistenteAsync()
    {
        _httpContext = obtenerHttpContext(1);

        var resultado = await Assert.ThrowsAsync<RecursoNoEncontradoException>(
            () => _servicio.obtenerClasePorIdAsync(3));
        Assert.Equal("No se encontró ninguna clase", resultado.Message);
    }

    [Fact]
    public async Task UnirseAUnaClaseAsync()
    {
        var codigo = "J22LI8";
        _contexto.Clase.Add(new Clase
        {
            IdClase = 3,
            Codigo = codigo,
            Nombre = "Historia de México",
            IdDocente = 1
        });
        _contexto.SaveChangesAsync();

        _httpContext = obtenerHttpContext(1);

        var resultado = await _servicio.unirseAClaseAsync(codigo, _httpContext);
        var claseExistente = await _claseDAO.obtenerClasePorCodigoAsync(codigo);

        Assert.NotNull(resultado);
        Assert.Equal(claseExistente.Codigo, resultado.Codigo);
    }

    [Fact]
    public async Task UnirseAUnaClaseSinAutorizacionAsync()
    {
        var codigo = "J22LI8";
        _httpContext = obtenerHttpContextInvalido();

        var resultado = await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _servicio.unirseAClaseAsync(codigo, _httpContext));
        Assert.Equal("Sin autorización para continuar.", resultado.Message);
    }

    [Fact]
    public async Task UnirseAUnaClaseIdUsuarioCeroAsync()
    {
        var codigo = "J22LI8";
        _httpContext = obtenerHttpContext(0);

        var resultado = await Assert.ThrowsAsync<ArgumentException>(
            () => _servicio.unirseAClaseAsync(codigo, _httpContext));
        Assert.Equal("La id del usuario no es válida", resultado.Message);
    }

    [Fact]
    public async Task UnirseAUnaClaseInexistenteAsyn()
    {
        var codigo = "J22LI8";
        _httpContext = obtenerHttpContext(1);

        var resultado = await Assert.ThrowsAsync<RecursoNoEncontradoException>(
            () => _servicio.unirseAClaseAsync(codigo, _httpContext));
        Assert.Equal("No se encontró ninguna clase", resultado.Message);
    }

    [Fact]
    public async Task UnirseAUnaClaseCamposVaciosNulosAsync()
    {
        var codigo = "";
        _httpContext = obtenerHttpContext(1);

        var resultado = await Assert.ThrowsAsync<ArgumentException>(
            () => _servicio.unirseAClaseAsync(codigo, _httpContext));
        Assert.Equal("El código de la clase es inválido", resultado.Message);
    }

    [Fact]
    public async Task UnirseAUnaClaseSinDocenteAsync()
    {
        var codigo = "J22LI8";
        _contexto.Clase.Add(new Clase
        {
            IdClase = 3,
            Codigo = "J22LI8",
            Nombre = "Historia de México",
            IdDocente = 0
        });
        await _contexto.SaveChangesAsync();

        _httpContext = obtenerHttpContext(1);

        var resultado = await Assert.ThrowsAsync<ClaseTerminadaException>(
            () => _servicio.unirseAClaseAsync(codigo, _httpContext));
        Assert.Equal("Esta clase ya no acepta a más alumnos", resultado.Message);
    }

    [Fact]
    public async Task AbandonarClaseConDocenteConRegistrosAsync()
    {
        _contexto.Clase.Add(new Clase
        {
            IdClase = 3,
            Codigo = "J22LI8",
            Nombre = "Historia de México",
            IdDocente = 1
        });
        _contexto.Registro.Add(new Registro
        {
            IdRegistro = 51,
            IdAlumno = 26,
            IdClase = 3,
            UltimoInicio = new DateTime()
        });
        _contexto.Registro.Add(new Registro
        {
            IdRegistro = 35,
            IdAlumno = 1,
            IdClase = 3,
            UltimoInicio = new DateTime()
        });
        await _contexto.SaveChangesAsync();

        _httpContext = obtenerHttpContext(1);

        await _servicio.salirDeClaseAsync(3, _httpContext);
        var registroEliminado = await _registroDAO.obtenerRegistroPorIdAlumnoYClaseAsync(1, 3);
        var clase = await _claseDAO.obtenerClasePorIdAsync(3);

        Assert.Null(registroEliminado);
        Assert.NotNull(clase);
    }

    [Fact]
    public async Task AbandonarClaseConDocenteSinRegistrosAsync()
    {
        _contexto.Clase.Add(new Clase
        {
            IdClase = 3,
            Codigo = "J22LI8",
            Nombre = "Historia de México",
            IdDocente = 1
        });
        _contexto.Registro.Add(new Registro
        {
            IdRegistro = 35,
            IdAlumno = 1,
            IdClase = 3,
            UltimoInicio = new DateTime()
        });
        await _contexto.SaveChangesAsync();

        _httpContext = obtenerHttpContext(1);

        await _servicio.salirDeClaseAsync(3, _httpContext);
        var registroEliminado = await _registroDAO.obtenerRegistroPorIdAlumnoYClaseAsync(1, 3);
        var clase = await _claseDAO.obtenerClasePorIdAsync(3);

        Assert.Null(registroEliminado);
        Assert.NotNull(clase);
    }

    [Fact]
    public async Task AbandonarClaseSinDocenteConRegistrosAsync()
    {
        _contexto.Clase.Add(new Clase
        {
            IdClase = 3,
            Codigo = "J22LI8",
            Nombre = "Historia de México",
            IdDocente = 0
        });
        _contexto.Registro.Add(new Registro
        {
            IdRegistro = 51,
            IdAlumno = 26,
            IdClase = 3,
            UltimoInicio = new DateTime()
        });
        _contexto.Registro.Add(new Registro
        {
            IdRegistro = 35,
            IdAlumno = 1,
            IdClase = 3,
            UltimoInicio = new DateTime()
        });
        await _contexto.SaveChangesAsync();

        _httpContext = obtenerHttpContext(1);

        await _servicio.salirDeClaseAsync(3, _httpContext);
        var registroEliminado = await _registroDAO.obtenerRegistroPorIdAlumnoYClaseAsync(1, 3);
        var clase = await _claseDAO.obtenerClasePorIdAsync(3);
        
        Assert.Null(registroEliminado);
        Assert.NotNull(clase);
    }

    [Fact]
    public async Task AbandonarClaseSinDocenteSinRegistrosAsync()
    {
        _contexto.Clase.Add(new Clase
        {
            IdClase = 3,
            Codigo = "J22LI8",
            Nombre = "Historia de México",
            IdDocente = 0
        });
        _contexto.Registro.Add(new Registro
        {
            IdRegistro = 35,
            IdAlumno = 1,
            IdClase = 3,
            UltimoInicio = new DateTime()
        });
        await _contexto.SaveChangesAsync();

        _httpContext = obtenerHttpContext(1);

        await _servicio.salirDeClaseAsync(3, _httpContext);
        var registroEliminado = await _registroDAO.obtenerRegistroPorIdAlumnoYClaseAsync(1, 3);
        var claseEliminada = await _claseDAO.obtenerClasePorIdAsync(3);

        Assert.Null(registroEliminado);
        Assert.Null(claseEliminada);
    }

    [Fact]
    public async Task AbandonarClaseInexistenteAsync()
    {
        _httpContext = obtenerHttpContext(1);

        var resultado = await Assert.ThrowsAsync<RecursoNoEncontradoException>(
            () => _servicio.salirDeClaseAsync(3, _httpContext));
        Assert.Equal("No se encontró ningún registro a la clase con id 3", resultado.Message);
    }

    [Fact]
    public async Task AbandonarSinAutorizacionAsync()
    {
        _httpContext = obtenerHttpContextInvalido();

        var resultado = await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _servicio.salirDeClaseAsync(3, _httpContext));
        Assert.Equal("Sin autorización para continuar.", resultado.Message);
    }

    [Fact]
    public async Task AbandonarClaseIdUsuarioCeroAsync()
    {
        _httpContext = obtenerHttpContext(0);

        var resultado = await Assert.ThrowsAsync<ArgumentException>(
            () => _servicio.salirDeClaseAsync(3, _httpContext));
        Assert.Equal("La id del usuario no es válida", resultado.Message);
    }

    [Fact]
    public async Task AbandonarClaseIdClaseCeroAsync()
    {
        _httpContext = obtenerHttpContext(1);

        var resultado = await Assert.ThrowsAsync<ArgumentException>(
            () => _servicio.salirDeClaseAsync(0, _httpContext));
        Assert.Equal("La id de la clase no es válida", resultado.Message);
    }

    [Fact]
    public async Task ObtenerClasesDocenteAsync()
    {
        _contexto.Clase.Add(new Clase
        {
            IdClase = 3,
            Codigo = "J22LI8",
            Nombre = "Historia de México",
            IdDocente = 1
        });
        _contexto.Clase.Add(new Clase
        {
            IdClase = 4,
            Codigo = "JNAO12",
            Nombre = "Ciencias Políticas",
            IdDocente = 1
        });
        _contexto.SaveChangesAsync();

        _httpContext = obtenerHttpContext(1);

        var clases = await _servicio.obtenerClasesDeDocenteAsync(_httpContext);

        Assert.NotNull(clases);
        Assert.Equal(1, clases[0].IdDocente);
        Assert.Equal(1, clases[1].IdDocente);
    }

    [Fact]
    public async Task ObtenerClasesDocenteSinAutorizacionAsync()
    {
        _httpContext = obtenerHttpContextInvalido();

        var resultado = await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _servicio.obtenerClasesDeDocenteAsync(_httpContext));
        Assert.Equal("Sin autorización para continuar.", resultado.Message);
    }

    [Fact]
    public async Task ObtenerClasesDocenteIdUsuarioCeroAsync()
    {
        _httpContext = obtenerHttpContext(0);

        var resultado = await Assert.ThrowsAsync<ArgumentException>(
            () => _servicio.obtenerClasesDeDocenteAsync(_httpContext));
        Assert.Equal("La id del usuario no es válida", resultado.Message);
    }

    [Fact]
    public async Task ObtenerClasesAlumnoAsync()
    {
        _contexto.Clase.Add(new Clase
        {
            IdClase = 3,
            Codigo = "J22LI8",
            Nombre = "Historia de México",
            IdDocente = 1
        });
        _contexto.Clase.Add(new Clase
        {
            IdClase = 10,
            Codigo = "7A624X",
            Nombre = "Metodologías de la Investigación",
            IdDocente = 89
        });
        _contexto.Registro.Add(new Registro
        {
            IdRegistro = 234,
            IdAlumno = 26,
            IdClase = 3,
            UltimoInicio = new DateTime()
        });
        _contexto.Registro.Add(new Registro
        {
            IdRegistro = 765,
            IdAlumno = 26,
            IdClase = 10,
            UltimoInicio = new DateTime()
        });
        await _contexto.SaveChangesAsync();

        _httpContext = obtenerHttpContext(26);

        var clases = await _servicio.obtenerClasesDeAlumnoAsync(_httpContext);

        Assert.NotNull(clases);
        Assert.Equal("Historia de México", clases[0].Nombre);
        Assert.Equal("Metodologías de la Investigación", clases[1].Nombre);
    }

    [Fact]
    public async Task ObtenerClasesAlumnoSinAutorizacionAsync()
    {
        _httpContext = obtenerHttpContextInvalido();

        var resultado = await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _servicio.obtenerClasesDeAlumnoAsync(_httpContext));
        Assert.Equal("Sin autorización para continuar.", resultado.Message);
    }

    [Fact]
    public async Task ObtenerClasesAlumnoIdUsuarioCeroAsync()
    {
        _httpContext = obtenerHttpContext(0);

        var resultado = await Assert.ThrowsAsync<ArgumentException>(
            () => _servicio.obtenerClasesDeAlumnoAsync(_httpContext));
        Assert.Equal("La id del usuario no es válida", resultado.Message);
    }

    [Fact]
    public async Task EnviarFechaVisualizacionAsync()
    {
        var fecha = new DateTime(2025, 6, 5, 12, 30, 00);
        _contexto.Clase.Add(new Clase
        {
            IdClase = 3,
            Codigo = "J22LI8",
            Nombre = "Historia de México",
            IdDocente = 1
        });
        _contexto.Registro.Add(new Registro
        {
            IdRegistro = 35,
            IdAlumno = 26,
            IdClase = 3,
            UltimoInicio = new DateTime()
        });
        await _contexto.SaveChangesAsync();

        _httpContext = obtenerHttpContext(26);

        await _servicio.enviarFechaVisualizacionAsync(3, fecha, _httpContext);
        var registro = await _registroDAO.obtenerRegistroPorIdAlumnoYClaseAsync(26, 3);

        Assert.Equal(fecha, registro.UltimoInicio);
    }

    [Fact]
    public async Task EnviarFechaVisualizacionSinAutorizacionAsync()
    {
        var fecha = new DateTime(2025, 6, 5, 12, 30, 00);
        _httpContext = obtenerHttpContextInvalido();

        var resultado = await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _servicio.enviarFechaVisualizacionAsync(3, fecha, _httpContext));
        Assert.Equal("Sin autorización para continuar.", resultado.Message);
    }

    [Fact]
    public async Task EnviarFechaVisualizacionIdUsuarioCeroAsync()
    {
        var fecha = new DateTime(2025, 6, 5, 12, 30, 00);
        _httpContext = obtenerHttpContext(0);

        var resultado = await Assert.ThrowsAsync<ArgumentException>(
            () => _servicio.enviarFechaVisualizacionAsync(3, fecha, _httpContext));
        Assert.Equal("La id del usuario no es válida", resultado.Message);
    }

    [Fact]
    public async Task EnviarFechaVisualizacionIdClaseCeroAsync()
    {
        var fecha = new DateTime(2025, 6, 5, 12, 30, 00);
        _httpContext = obtenerHttpContext(26);

        var resultado = await Assert.ThrowsAsync<ArgumentException>(
            () => _servicio.enviarFechaVisualizacionAsync(0, fecha, _httpContext));
        Assert.Equal("La id de la clase no es válida", resultado.Message);
    }

    [Fact]
    public async Task ObtenerRegistroAsync()
    {
        _contexto.Clase.Add(new Clase
        {
            IdClase = 3,
            Codigo = "J22LI8",
            Nombre = "Historia de México",
            IdDocente = 1
        });
        _contexto.Registro.Add(new Registro
        {
            IdRegistro = 35,
            IdAlumno = 26,
            IdClase = 3,
            UltimoInicio = new DateTime()
        });
        await _contexto.SaveChangesAsync();

        _httpContext = obtenerHttpContext(26);

        var registro = await _servicio.obtenerRegistroAlumno(26, 3, _httpContext);

        Assert.NotNull(registro);
        Assert.Equal(26, registro.IdAlumno);
        Assert.Equal(3, registro.IdClase);
    }

    [Fact]
    public async Task ObtenerRegistroSinAutorizacionAsync()
    {
        _httpContext = obtenerHttpContextInvalido();

        var resultado = await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _servicio.obtenerRegistroAlumno(26, 3, _httpContext));
        Assert.Equal("Sin autorización para continuar.", resultado.Message);
    }

    [Fact]
    public async Task ObtenerRegistroIdUsuarioCeroAsync()
    {
        _httpContext = obtenerHttpContext(0);

        var resultado = await Assert.ThrowsAsync<ArgumentException>(
            () => _servicio.obtenerRegistroAlumno(26, 3, _httpContext));
        Assert.Equal("La id del usuario no es válida", resultado.Message);
    }

    [Fact]
    public async Task ObtenerRegistroIdClaseCeroAsync()
    {
        _httpContext = obtenerHttpContext(26);

        var resultado = await Assert.ThrowsAsync<ArgumentException>(
            () => _servicio.obtenerRegistroAlumno(26, 0, _httpContext));
        Assert.Equal("La id de la clase no es válida", resultado.Message);
    }

    [Fact]
    public async Task ObtenerRegistroIdAlumnoCeroAsync()
    {
        _httpContext = obtenerHttpContext(26);

        var resultado = await Assert.ThrowsAsync<ArgumentException>(
            () => _servicio.obtenerRegistroAlumno(0, 3, _httpContext));
        Assert.Equal("La id del alumno no es válida", resultado.Message);
    }

    private HttpContext obtenerHttpContext(int idUsuario)
    {
        var claims = new List<Claim> { new Claim("idUsuario", idUsuario.ToString()) };
        var identity = new ClaimsIdentity(claims, "PruebaAuth");
        var principal = new ClaimsPrincipal(identity);
        return new DefaultHttpContext { User = principal };
    }

    private HttpContext obtenerHttpContextInvalido()
    {
        return new DefaultHttpContext { User = new ClaimsPrincipal() };
    }
}
