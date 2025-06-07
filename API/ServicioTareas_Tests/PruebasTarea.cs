using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ServicioTareas.Data;
using ServicioTareas.Data.DAOs.Implementations;
using ServicioTareas.Data.DAOs.Interfaces;
using ServicioTareas.Data.DTOs;
using ServicioTareas.Exceptions;
using ServicioTareas.Models;
using ServicioTareas.Services.Implementations;
using ServicioTareas.Services.Interfaces;

namespace ServicioTareas_Tests;

public class PruebasTarea
{
    private readonly TareasDbContext _contexto;
    private readonly ITareaDAO _tareaDAO;
    private readonly ITareasServices _servicio;
    private HttpContext _httpContext;

    public PruebasTarea()
    {
        var opciones = new DbContextOptionsBuilder<TareasDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _contexto = new TareasDbContext(opciones);

        _tareaDAO = new TareaDAO(_contexto);
        _servicio = new TareasService(_tareaDAO);
    }

    [Fact]
    public async Task CrearTareaAsync()
    {
        _httpContext = obtenerHttpContext(45, "docente");
        var tarea = new CrearTareaDTO
        {
            idClase = 267,
            nombre = "Examen diagnóstico",
            fechaLimite = new DateTime(2025, 6, 6, 16, 59, 59)
        };

        var resultado = await _servicio.crearTareaAsync(tarea, _httpContext);
        Assert.NotNull(resultado);
        Assert.Equal(tarea.nombre, resultado.Nombre);
    }

    [Fact]
    public async Task CrearTareaSinAutorizacionAsync()
    {
        _httpContext = obtenerHttpContext(-1, "docente");
        var tarea = new CrearTareaDTO
        {
            idClase = 267,
            nombre = "Examen diagnóstico",
            fechaLimite = new DateTime(2025, 6, 6, 16, 59, 59)
        };

        var resultado = await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _servicio.crearTareaAsync(tarea, _httpContext));
        Assert.Equal("Sin autorización para continuar", resultado.Message);
    }

    [Fact]
    public async Task CrearTareaCamposInvalidosAsync()
    {
        _httpContext = obtenerHttpContext(45, "docente");
        var tarea = new CrearTareaDTO
        {
            idClase = 0,
            nombre = "",
            fechaLimite = new DateTime()
        };

        var resultado = await Assert.ThrowsAsync<ArgumentException>(
            () => _servicio.crearTareaAsync(tarea, _httpContext));
        Assert.Equal("Hay campos inválidos", resultado.Message);
    }

    [Fact]
    public async Task CrearTareaNombreRepetidoAsync()
    {
        _contexto.Tarea.Add(new Tarea
        {
            IdClase = 40,
            Nombre = "Actividad 1 - Métodos Ágiles",
            IdArchivo = 23,
            FechaLimite = new DateTime(2025, 5, 23, 23, 59, 59),
            Estado = "terminada"
        });
        await _contexto.SaveChangesAsync();

        _httpContext = obtenerHttpContext(45, "docente");
        var tarea = new CrearTareaDTO
        {
            idClase = 40,
            nombre = "Actividad 1 - Métodos Ágiles",
            fechaLimite = new DateTime()
        };

        var resultado = await Assert.ThrowsAsync<RecursoYaExistenteException>(
            () => _servicio.crearTareaAsync(tarea, _httpContext));
        Assert.Equal("Nombre repetido", resultado.Message);
    }

    [Fact]
    public async Task EditarTareaAsync()
    {
        _contexto.Tarea.Add(new Tarea
        {
            IdTarea = 1003,
            IdClase = 67,
            Nombre = "Ejercicios de lógica 2",
            IdArchivo = 3095,
            FechaLimite = new DateTime(2025, 6, 22, 23, 59, 59),
            Estado = "activa"
        });
        await _contexto.SaveChangesAsync();

        _httpContext = obtenerHttpContext(45, "docente");
        var tarea = new EditarTareaDTO
        {
            idTarea = 1003,
            nombre = "Ejercicios de Lógica - EXTENDIDO",
            idArchivo = 3095,
            fechaLimite = new DateTime(2025, 6, 23, 23, 59, 59)
        };

        var resultado = await _servicio.editarTareaAsync(tarea, _httpContext);
        Assert.NotNull(resultado);
        Assert.Equal(tarea.nombre, resultado.Nombre);
    }

    [Fact]
    public async Task EditarTareaSinAutorizacionAsync()
    {
        _httpContext = obtenerHttpContext(45, "");
        var tarea = new EditarTareaDTO
        {
            idTarea = 1003,
            nombre = "Ejercicios de Lógica - EXTENDIDO",
            idArchivo = 3095,
            fechaLimite = new DateTime(2025, 6, 23, 23, 59, 59)
        };
        var resultado = await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _servicio.editarTareaAsync(tarea, _httpContext));
        Assert.Equal("Sin autorización para continuar", resultado.Message);
    }

    [Fact]
    public async Task EditarTareaCamposInvalidosAsync()
    {
        _httpContext = obtenerHttpContext(45, "docente");
        var tarea = new EditarTareaDTO
        {
            idTarea = 0,
            nombre = "",
            idArchivo = 3095,
            fechaLimite = new DateTime()
        };
        var resultado = await Assert.ThrowsAsync<ArgumentException>(
            () => _servicio.editarTareaAsync(tarea, _httpContext));
        Assert.Equal("Hay campos inválidos", resultado.Message);
    }

    [Fact]
    public async Task EditarTareaInexistenteAsync()
    {
        _httpContext = obtenerHttpContext(45, "docente");
        var tarea = new EditarTareaDTO
        {
            idTarea = 404,
            nombre = "Ciclo del agua",
            idArchivo = 8095,
            fechaLimite = new DateTime(2025, 6, 23, 23, 59, 59)
        };
        var resultado = await Assert.ThrowsAsync<RecursoNoEncontradoException>(
            () => _servicio.editarTareaAsync(tarea, _httpContext));
        Assert.Equal("No se encontró una tarea con la id 404", resultado.Message);
    }

    [Fact]
    public async Task EditarTareaNombreRepetidoAsync()
    {
        _contexto.Tarea.Add(new Tarea
        {
            IdTarea = 734,
            IdClase = 40,
            Nombre = "Actividad 1 - Métodos Ágiles",
            IdArchivo = 0,
            FechaLimite = new DateTime(2025, 6, 6, 16, 59, 59),
            Estado = "terminada"
        });
        _contexto.Tarea.Add(new Tarea
        {
            IdTarea = 736,
            IdClase = 40,
            Nombre = "Actividad 2 - Estándares",
            IdArchivo = 0,
            FechaLimite = new DateTime(2025, 6, 6, 16, 59, 59),
            Estado = "terminada"
        });
        await _contexto.SaveChangesAsync();

        _httpContext = obtenerHttpContext(45, "docente");
        var tarea = new EditarTareaDTO
        {
            idTarea = 736,
            nombre = "Actividad 1 - Métodos Ágiles",
            idArchivo = 0,
            fechaLimite = new DateTime(2025, 6, 20, 16, 59, 59)
        };
        var resultado = await Assert.ThrowsAsync<RecursoYaExistenteException>(
            () => _servicio.editarTareaAsync(tarea, _httpContext));
        Assert.Equal("Nombre repetido", resultado.Message);
    }

    [Fact]
    public async Task EliminarTareaAsync()
    {
        _contexto.Tarea.Add(new Tarea
        {
            IdTarea = 734,
            IdClase = 40,
            Nombre = "Actividad 1 - Métodos Ágiles",
            IdArchivo = 851,
            FechaLimite = new DateTime(2025, 6, 22, 23, 59, 59),
            Estado = "activa"
        });
        await _contexto.SaveChangesAsync();

        _httpContext = obtenerHttpContext(45, "docente");

        await _servicio.eliminarTareaAsync(734, _httpContext);
        var tareaEliminada = await _tareaDAO.obtenerTareaPorIdAsync(734);
        Assert.Null(tareaEliminada);
    }

    [Fact]
    public async Task EliminarTareaSinAutorizacionAsync()
    {
        _httpContext = obtenerHttpContext(0, "");

        var resultado = await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _servicio.eliminarTareaAsync(734, _httpContext));
        Assert.Equal("Sin autorización para continuar", resultado.Message);
    }

    [Fact]
    public async Task EliminarTareaIdTareaCeroAsync()
    {
        _httpContext = obtenerHttpContext(45, "docente");

        var resultado = await Assert.ThrowsAsync<ArgumentException>(
            () => _servicio.eliminarTareaAsync(0, _httpContext));
        Assert.Equal("La id de la tarea es inválida", resultado.Message);
    }

    [Fact]
    public async Task EliminarTareaInexistenteAsync()
    {
        _httpContext = obtenerHttpContext(45, "docente");

        var resultado = await Assert.ThrowsAsync<RecursoNoEncontradoException>(
            () => _servicio.eliminarTareaAsync(404, _httpContext));
        Assert.Equal("No se encontró una tarea con la id 404", resultado.Message);
    }

    [Fact]
    public async Task ObtenerTareasAsync()
    {
        _contexto.Tarea.Add(new Tarea
        {
            IdTarea = 734,
            IdClase = 40,
            Nombre = "Actividad 1 - Métodos Ágiles",
            IdArchivo = 851,
            FechaLimite = new DateTime(2025, 6, 22, 23, 59, 59),
            Estado = "activa"
        });
        _contexto.Tarea.Add(new Tarea
        {
            IdTarea = 736,
            IdClase = 40,
            Nombre = "Actividad 2 - Estándares",
            IdArchivo = 853,
            FechaLimite = new DateTime(2025, 6, 22, 23, 59, 59),
            Estado = "activa"
        });
        await _contexto.SaveChangesAsync();

        var resultado = await _servicio.obtenerTareasDeClaseAsync(40);
        Assert.Equal(734, resultado[0].IdTarea);
        Assert.Equal(736, resultado[1].IdTarea);
    }

    [Fact]
    public async Task ObtenerTareaIdClaseInvalidaAsync()
    {
        var resultado = await Assert.ThrowsAsync<ArgumentException>(
            () => _servicio.obtenerTareasDeClaseAsync(0));
        Assert.Equal("La id de la clase es inválida", resultado.Message);
    }

    private HttpContext obtenerHttpContext(int idUsuario, string rol)
    {
        var claims = new List<Claim>();
        if (idUsuario != -1)
        {
            claims.Add(new Claim("idUsuario", idUsuario.ToString()));
        }
        claims.Add(new Claim(ClaimTypes.Role, rol));
        var identity = new ClaimsIdentity(claims, "PruebaAuth");
        var principal = new ClaimsPrincipal(identity);
        return new DefaultHttpContext { User = principal };
    }
}
