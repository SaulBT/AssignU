using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using ServicioUsuarios.DAOs;
using ServicioUsuarios.DAOs.Interfaces;
using ServicioUsuarios.DTOs;
using ServicioUsuarios.Entities;
using ServicioUsuarios.Exceptions;
using ServicioUsuarios.Services.Implementation;

namespace ServicioUsuarios.Tests;

public class PruebasDocente
{
    private readonly usuarios_bd_assignuContext _contexto;
    private readonly DocenteDAO _docenteDAO;
    private readonly DocenteService _servicio;
    private HttpContext _httpContext;

    public PruebasDocente()
    {
        var opciones = new DbContextOptionsBuilder<usuarios_bd_assignuContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _contexto = new usuarios_bd_assignuContext(opciones);

        _docenteDAO = new DocenteDAO(_contexto);
        _servicio = new DocenteService(_docenteDAO);
    }

    [Fact]
    public async Task CrearUsuarioDocenteAsync()
    {
        var docente = new RegistrarDocenteDTO
        {
            nombreCompleto = "Salomé de la Paz Torres Pérez",
            nombreUsuario = "salocelotl",
            contrasenia = "moisal77",
            correoElectronico = "lafloryelcanto@gmail.com",
            idGradoProfesional = 3
        };

        var resultado = await _servicio.registrarAsync(docente);
        var alumnoGuardado = await _docenteDAO.obtenerPorNombreUsuarioAsync("salocelotl");

        Assert.NotNull(resultado);
        Assert.Equal("salocelotl", alumnoGuardado.nombreUsuario);
    }

    [Fact]
    public async Task CrearUsuarioDocenteConNombreUsuarioRepetido()
    {
        _contexto.Add(new docente
        {
            nombreCompleto = "Claudia Trujillo Espinoza",
            nombreUsuario = "clautruji99",
            contrasenia = "stardust",
            correo = "ctrujilloesp@uv.mx",
            idGradoProfesional = 2
        });
        await _contexto.SaveChangesAsync();

        var docente = new RegistrarDocenteDTO
        {
            nombreCompleto = "Claudia Truman Ochoa",
            nombreUsuario = "clautruji99",
            contrasenia = "RUBIEL_123",
            correoElectronico = "coruscant@yahoo.com",
            idGradoProfesional = 2
        };

        var resultado = await Assert.ThrowsAsync<RecursoYaExistenteException>(
            () => _servicio.registrarAsync(docente));
        Assert.Equal("El nombre de usuario 'clautruji99' ya está en uso por otro docente.", resultado.Message);
    }

    [Fact]
    public async Task CrearUsuarioDocenteConCorreoRepetido()
    {
        _contexto.Add(new docente
        {
            nombreCompleto = "Claudia Trujillo Espinoza",
            nombreUsuario = "clautruji99",
            contrasenia = "stardust",
            correo = "ctrujilloesp@uv.mx",
            idGradoProfesional = 2
        });
        await _contexto.SaveChangesAsync();

        var docente = new RegistrarDocenteDTO
        {
            nombreCompleto = "Claudia Truman Ochoa",
            nombreUsuario = "florYcanto",
            contrasenia = "RUBIEL_123",
            correoElectronico = "ctrujilloesp@uv.mx",
            idGradoProfesional = 2
        };

        var resultado = await Assert.ThrowsAsync<RecursoYaExistenteException>(
            () => _servicio.registrarAsync(docente));
        Assert.Equal("El correo 'ctrujilloesp@uv.mx' ya está en uso por otro docente.", resultado.Message);
    }

    [Fact]
    public async Task CrearUsuarioDocenteValoresNulos()
    {
        var docente = new RegistrarDocenteDTO
        {
            nombreCompleto = null,
            nombreUsuario = null,
            contrasenia = null,
            correoElectronico = null,
            idGradoProfesional = 0
        };

        var resultado = await Assert.ThrowsAsync<ArgumentException>(
            () => _servicio.registrarAsync(docente));
        Assert.Equal("Los parámetros del docente son inválidos.", resultado.Message);
    }

    [Fact]
    public async Task ActualizarDocenteAsync()
    {
        _contexto.Add(new docente
        {
            idDocente = 10,
            nombreCompleto = "Claudia Trujillo Espinoza",
            nombreUsuario = "clautruji99",
            contrasenia = "stardust",
            correo = "ctrujilloesp@uv.mx",
            idGradoProfesional = 2
        });
        await _contexto.SaveChangesAsync();

        var claims = new List<Claim>
        {
            new Claim("idUsuario", "10")
        };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var principal = new ClaimsPrincipal(identity);
        _httpContext = new DefaultHttpContext { User = principal };

        var docente = new ActualizarDocenteDTO
        {
            idDocente = 10,
            nombreCompleto = "Claudia Trujillo Méndez",
            nombreUsuario = "clautruji99",
            idGradoProfesional = 3
        };
        await _servicio.actualizarAsync(_httpContext, docente);
        var docenteActualizado = await _docenteDAO.obtenerPorIdNormalAsync(10);

        Assert.Equal("Claudia Trujillo Méndez", docenteActualizado.nombreCompleto);
        Assert.Equal("clautruji99", docenteActualizado.nombreUsuario);
    }

    [Fact]
    public async Task AcutalizarDocenteIdDiferenteAsync()
    {
        _contexto.Add(new docente
        {
            idDocente = 10,
            nombreCompleto = "Claudia Trujillo Espinoza",
            nombreUsuario = "clautruji99",
            contrasenia = "stardust",
            correo = "ctrujilloesp@uv.mx",
            idGradoProfesional = 2
        });
        await _contexto.SaveChangesAsync();

        var claims = new List<Claim>
        {
            new Claim("idUsuario", "1")
        };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var principal = new ClaimsPrincipal(identity);
        _httpContext = new DefaultHttpContext { User = principal };

        var docente = new ActualizarDocenteDTO
        {
            idDocente = 10,
            nombreCompleto = "Claudia Trujillo Méndez",
            nombreUsuario = "clautruji99",
            idGradoProfesional = 3
        };

        var resultado = await Assert.ThrowsAsync<DiscordanciaDeIdException>(
            () => _servicio.actualizarAsync(_httpContext, docente));
        Assert.Equal("El ID del docente no coincide con el ID proporcionado.", resultado.Message);
    }

    [Fact]
    public async Task ActualizarDocenteConNombreUsuarioRepetidoAsync()
    {
        _contexto.Add(new docente
        {
            idDocente = 10,
            nombreCompleto = "Claudia Trujillo Espinoza",
            nombreUsuario = "clautruji99",
            contrasenia = "stardust",
            correo = "ctrujilloesp@uv.mx",
            idGradoProfesional = 2
        });
        _contexto.Add(new docente
        {
            idDocente = 17,
            nombreCompleto = "Claudia Truman Ochoa",
            nombreUsuario = "clautruji99",
            contrasenia = "MOMOS_BOT_123",
            correo = "cltochoa@uv.mx",
            idGradoProfesional = 1
        });
        await _contexto.SaveChangesAsync();

        var claims = new List<Claim>
        {
            new Claim("idUsuario", "17")
        };

        var identity = new ClaimsIdentity(claims, "TestAuth");
        var principal = new ClaimsPrincipal(identity);
        _httpContext = new DefaultHttpContext { User = principal };

        var docenteDto = new ActualizarDocenteDTO
        {
            idDocente = 17,
            nombreCompleto = "Claudia Truman Ochoa",
            nombreUsuario = "clautruji99",
            idGradoProfesional = 1
        };

        var resultado = await Assert.ThrowsAsync<RecursoYaExistenteException>(
            () => _servicio.actualizarAsync(_httpContext, docenteDto));
        Assert.Equal("El nombre de usuario 'clautruji99' ya está en uso por otro docente.", resultado.Message);
    }

    [Fact]
    public async Task ActualizarDocenteSinToken()
    {
        var docenteDto = new ActualizarDocenteDTO
        {
            idDocente = 10,
            nombreCompleto = "Claudia Trujillo Méndez",
            nombreUsuario = "clautruji99",
            idGradoProfesional = 3
        };
        _httpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity())
        };

        var resultado = await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _servicio.actualizarAsync(_httpContext, docenteDto));
    }

    [Fact]
    public async Task EliminarDocenteAsync()
    {
        _contexto.Add(new docente
        {
            idDocente = 10,
            nombreCompleto = "Claudia Trujillo Espinoza",
            nombreUsuario = "clautruji99",
            contrasenia = "stardust",
            correo = "ctrujilloesp@uv.mx",
            idGradoProfesional = 2
        });
        await _contexto.SaveChangesAsync();

        var claims = new List<Claim>
        {
            new Claim("idUsuario", "10")
        };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var principal = new ClaimsPrincipal(identity);
        _httpContext = new DefaultHttpContext { User = principal };

        await _servicio.eliminarAsync(_httpContext);
        var docenteEliminado = await _docenteDAO.obtenerPorNombreUsuarioAsync("clautruji99");
        Assert.Null(docenteEliminado);
    }

    [Fact]
    public async Task EliminarAlumnoNoGuardadoAsync()
    {
        var claims = new List<Claim>
        {
            new Claim("idUsuario", "10")
        };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var principal = new ClaimsPrincipal(identity);
        _httpContext = new DefaultHttpContext { User = principal };

        var resultado = await Assert.ThrowsAsync<RecursoNoEncontradoException>(
            () => _servicio.eliminarAsync(_httpContext));
        Assert.Equal("El docente no existe.", resultado.Message);
    }

    [Fact]
    public async Task EliminarDocenteSinTokenAsync()
    {
        _httpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity())
        };

        var resultado = await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _servicio.eliminarAsync(_httpContext));
    }

    [Fact]
    public async Task ObtenerDocentePorIdAsync()
    {
        _contexto.Add(new docente
        {
            idDocente = 10,
            nombreCompleto = "Claudia Trujillo Espinoza",
            nombreUsuario = "clautruji99",
            contrasenia = "stardust",
            correo = "ctrujilloesp@uv.mx",
            idGradoProfesional = 2
        });
        await _contexto.SaveChangesAsync();

        var alumno = await _servicio.obtenerPorIdAsync(10);
        Assert.NotNull(alumno);
        Assert.Equal("Claudia Trujillo Espinoza", alumno.nombreCompleto);
    }

    [Fact]
    public async Task ObtenerDocenteNoGuardadoAsync()
    {
        var resultado = await Assert.ThrowsAsync<RecursoNoEncontradoException>(
            () => _servicio.obtenerPorIdAsync(10));
        Assert.Equal("El docente no existe.", resultado.Message);
    }

    [Fact]
    public async Task CambiarContraseniaDocenteAsync()
    {
        _contexto.Add(new docente
        {
            idDocente = 10,
            nombreCompleto = "Claudia Trujillo Espinoza",
            nombreUsuario = "clautruji99",
            contrasenia = "stardust",
            correo = "ctrujilloesp@uv.mx",
            idGradoProfesional = 2
        });
        await _contexto.SaveChangesAsync();

        var claims = new List<Claim>
        {
            new Claim("idUsuario", "10")
        };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var principal = new ClaimsPrincipal(identity);
        _httpContext = new DefaultHttpContext { User = principal };

        var cambioContrasenia = new CambiarContraseniaDTO
        {
            contraseniaActual = "stardust",
            contraseniaNueva = "5qe8fn29"
        };

        await _servicio.cambiarContraseniaAsync(cambioContrasenia, _httpContext);
        var docenteActualizado = await _docenteDAO.obtenerPorIdNormalAsync(10);

        Assert.NotNull(docenteActualizado);
        Assert.Equal("5qe8fn29", docenteActualizado.contrasenia);
    }

    [Fact]
    public async Task CambiarContraseniaDocenteSinTokenAsync()
    {
        var cambioContrasenia = new CambiarContraseniaDTO
        {
            contraseniaActual = "stardust",
            contraseniaNueva = "5qe8fn29"
        };
        _httpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal() // sin identidad
        };

        var resultado = await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _servicio.cambiarContraseniaAsync(cambioContrasenia, _httpContext));
    }

    [Fact]
    public async Task CambiarContraseniaDocenteConContraseniaIncorrectaAsync()
    {
        _contexto.Add(new docente
        {
            idDocente = 10,
            nombreCompleto = "Claudia Trujillo Espinoza",
            nombreUsuario = "clautruji99",
            contrasenia = "stardust",
            correo = "ctrujilloesp@uv.mx",
            idGradoProfesional = 2
        });
        await _contexto.SaveChangesAsync();

        var claims = new List<Claim>
        {
            new Claim("idUsuario", "10")
        };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var principal = new ClaimsPrincipal(identity);
        _httpContext = new DefaultHttpContext { User = principal };

        var cambioContrasenia = new CambiarContraseniaDTO
        {
            contraseniaActual = "#09235LJ1",
            contraseniaNueva = "5qe8fn29"
        };

        var resultado = await Assert.ThrowsAsync<ContraseniaDiferenteException>(
            () => _servicio.cambiarContraseniaAsync(cambioContrasenia, _httpContext));
        Assert.Equal("La contraseña actual es incorrecta.", resultado.Message);
    }
}