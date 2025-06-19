using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ServicioUsuarios.DAOs;
using ServicioUsuarios.DTOs;
using ServicioUsuarios.Entities;
using ServicioUsuarios.Exceptions;
using ServicioUsuarios.Services.Implementation;

namespace ServicioUsuarios.Tests;

public class PruebasAlumno
{
    private readonly usuarios_bd_assignuContext _contexto;
    private readonly AlumnoDAO _alumnoDAO;
    private readonly AlumnoService _servicio;
    private HttpContext _httpContext;

    public PruebasAlumno()
    {
        var opciones = new DbContextOptionsBuilder<usuarios_bd_assignuContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _contexto = new usuarios_bd_assignuContext(opciones);

        _alumnoDAO = new AlumnoDAO(_contexto);
        _servicio = new AlumnoService(_alumnoDAO);
    }

    [Fact]
    public async Task CrearUsuarioAlumnoAsync()
    {
        var alumno = new RegistrarAlumnoDTO
        {
            nombreCompleto = "Luis Alfonso Tejeda Castañeda",
            nombreUsuario = "luisfonsi",
            contrasenia = "hRBK2762",
            correoElectronico = "alfonso.unam.fei@gmail.com",
            idGradoEstudios = 4
        };

        var resultado = await _servicio.registrarAsync(alumno);
        var alumnoGuardado = await _alumnoDAO.obtenerPorNombreUsuarioAsync("luisfonsi");

        Assert.NotNull(resultado);
        Assert.Equal("luisfonsi", alumnoGuardado.nombreUsuario);
    }

    [Fact]
    public async Task CrearUsuarioAlumnoConNombreRepetidoAsync()
    {
        _contexto.alumnos.Add(new alumno
        {
            nombreCompleto = "Luis Daniel Tejeda Dorantes",
            nombreUsuario = "DanielTD_04",
            contrasenia = "iblNM3",
            correo = "tejedadaniel8oct@gmail.com",
            idGradoEstudios = 3
        });
        await _contexto.SaveChangesAsync();

        var alumno = new RegistrarAlumnoDTO
        {
            nombreCompleto = "Daniel Tendera Durán",
            nombreUsuario = "DanielTD_04",
            contrasenia = "mundoDeCaram310",
            correoElectronico = "zS25014787@uv.com.mx",
            idGradoEstudios = 4
        };

        var resultado = await Assert.ThrowsAsync<RecursoYaExistenteException>(
            () => _servicio.registrarAsync(alumno));
        Assert.Equal("El nombre de usuario 'DanielTD_04' ya está en uso por otro alumno.", resultado.Message);
    }

    [Fact]
    public async Task CrearUsuarioAlumnoConCorreoRepetidoAsync()
    {
        _contexto.alumnos.Add(new alumno
        {
            nombreCompleto = "Luis Daniel Tejeda Dorantes",
            nombreUsuario = "DanielTD_04",
            contrasenia = "iblNM3",
            correo = "tejedadaniel8oct@gmail.com",
            idGradoEstudios = 3
        });
        await _contexto.SaveChangesAsync();

        var alumno = new RegistrarAlumnoDTO
        {
            nombreCompleto = "Daniel Tejeda Barragán",
            nombreUsuario = "DanieliniCapuchini",
            contrasenia = "mementoMORI",
            correoElectronico = "tejedadaniel8oct@gmail.com",
            idGradoEstudios = 3
        };

        var resultado = await Assert.ThrowsAsync<RecursoYaExistenteException>(
            () => _servicio.registrarAsync(alumno));
        Assert.Equal("El correo 'tejedadaniel8oct@gmail.com' ya está en uso por otro alumno.", resultado.Message);
    }

    [Fact]
    public async Task CrearUsuarioAlumnoValoresNulosAsync()
    {
        var alumno = new RegistrarAlumnoDTO
        {
            nombreCompleto = "",
            nombreUsuario = "",
            contrasenia = "",
            correoElectronico = "",
            idGradoEstudios = 0
        };

        var resultado = await Assert.ThrowsAsync<ArgumentException>(
            () => _servicio.registrarAsync(alumno));
        Assert.Equal("Los parámetros de registro del alumno son inválidos.", resultado.Message);
    }

    [Fact]
    public async Task ActualizarAlumnoAync()
    {
        _contexto.alumnos.Add(new alumno
        {
            idAlumno = 10,
            nombreCompleto = "Luis Daniel Tejeda Dorantes",
            nombreUsuario = "DanielTD_04",
            contrasenia = "iblNM3",
            correo = "tejedadaniel8oct@gmail.com",
            idGradoEstudios = 3
        });
        await _contexto.SaveChangesAsync();

        var claims = new List<Claim>
        {
            new Claim("idUsuario", "10"),
        };

        var identity = new ClaimsIdentity(claims, "TestAuth");
        var principal = new ClaimsPrincipal(identity);
        _httpContext = new DefaultHttpContext { User = principal };

        var alumnoDto = new ActualizarAlumnoDTO
        {
            idAlumno = 10,
            nombreCompleto = "Luis Daniel Tejeda Dorántez",
            nombreUsuario = "DanielTD_04",
            idGradoEstudios = 4
        };
        await _servicio.actualizarAsync(_httpContext, alumnoDto);
        var alumnoActualizado = await _alumnoDAO.obtenerPorIdDtoAsync(10);

        Assert.Equal("Luis Daniel Tejeda Dorántez", alumnoActualizado.nombreCompleto);
        Assert.Equal(4, alumnoActualizado.idGradoEstudios);
    }

    [Fact]
    public async Task ActualizarAlumnoIdDiferenteAsync()
    {
        _contexto.alumnos.Add(new alumno
        {
            idAlumno = 10,
            nombreCompleto = "Luis Daniel Tejeda Dorantes",
            nombreUsuario = "DanielTD_04",
            contrasenia = "iblNM3",
            correo = "tejedadaniel8oct@gmail.com",
            idGradoEstudios = 3
        });
        await _contexto.SaveChangesAsync();

        var claims = new List<Claim>
        {
            new Claim("idUsuario", "4"),
        };

        var identity = new ClaimsIdentity(claims, "TestAuth");
        var principal = new ClaimsPrincipal(identity);
        _httpContext = new DefaultHttpContext { User = principal };

        var alumnoDto = new ActualizarAlumnoDTO
        {
            idAlumno = 10,
            nombreCompleto = "Luis Daniel Tejeda Dorántez",
            nombreUsuario = "DanielTD_04",
            idGradoEstudios = 4
        };

        var resultado = await Assert.ThrowsAsync<DiscordanciaDeIdException>(
            () => _servicio.actualizarAsync(_httpContext, alumnoDto));
        Assert.Equal("El ID del alumno en la URL no coincide con el ID del alumno en el cuerpo de la solicitud.", resultado.Message);
    }

    [Fact]
    public async Task ActualizarAlumnoNombreUsuarioRepetidoAsync()
    {
        _contexto.alumnos.Add(new alumno
        {
            idAlumno = 10,
            nombreCompleto = "Luis Daniel Tejeda Dorantes",
            nombreUsuario = "DanielTD_04",
            contrasenia = "iblNM3",
            correo = "tejedadaniel8oct@gmail.com",
            idGradoEstudios = 3
        });
        _contexto.alumnos.Add(new alumno
        {
            idAlumno = 8,
            nombreCompleto = "Guadalupe Cuellar Alonso",
            nombreUsuario = "GuadalupeCuellar",
            contrasenia = "sng123",
            correo = "ingbreton@gmail.com",
            idGradoEstudios = 2
        });
        await _contexto.SaveChangesAsync();

        var claims = new List<Claim>
        {
            new Claim("idUsuario", "8"),
        };

        var identity = new ClaimsIdentity(claims, "TestAuth");
        var principal = new ClaimsPrincipal(identity);
        _httpContext = new DefaultHttpContext { User = principal };

        var alumnoDto = new ActualizarAlumnoDTO
        {
            idAlumno = 8,
            nombreCompleto = "Guadalupe Cuellar Alonso",
            nombreUsuario = "DanielTD_04",
            idGradoEstudios = 2
        };

        var resultado = await Assert.ThrowsAsync<RecursoYaExistenteException>(
            () => _servicio.actualizarAsync(_httpContext, alumnoDto));
        Assert.Equal("El nombre de usuario 'DanielTD_04' ya está en uso por otro alumno.", resultado.Message);
    }

    [Fact]
    public async Task ActualizarAlumnoSinTokenAsync()
    {
        var alumnoDto = new ActualizarAlumnoDTO
        {
            idAlumno = 10,
            nombreCompleto = "Luis Daniel Tejeda Dorántez",
            nombreUsuario = "DanielTD_04",
            idGradoEstudios = 4
        };
        _httpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal() // sin identidad
        };

        var resultado = await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _servicio.actualizarAsync(_httpContext, alumnoDto));
    }

    [Fact]
    public async Task EliminarAlumnoAsync()
    {
        _contexto.alumnos.Add(new alumno
        {
            idAlumno = 10,
            nombreCompleto = "Luis Daniel Tejeda Dorantes",
            nombreUsuario = "DanielTD_04",
            contrasenia = "iblNM3",
            correo = "tejedadaniel8oct@gmail.com",
            idGradoEstudios = 3
        });
        await _contexto.SaveChangesAsync();

        var claims = new List<Claim>
        {
            new Claim("idUsuario", "10"),
        };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var principal = new ClaimsPrincipal(identity);
        _httpContext = new DefaultHttpContext { User = principal };

        await _servicio.eliminarAsync(_httpContext);
        var alumnoEliminado = await _alumnoDAO.obtenerPorNombreUsuarioAsync("DanielTD_04");
        Assert.Null(alumnoEliminado);
    }

    [Fact]
    public async Task EliminarAlumnoNoGuardadoAsync()
    {
        var claims = new List<Claim>
        {
            new Claim("idUsuario", "10"),
        };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var principal = new ClaimsPrincipal(identity);
        _httpContext = new DefaultHttpContext { User = principal };

        var resultado = await Assert.ThrowsAsync<RecursoNoEncontradoException>(
            () => _servicio.eliminarAsync(_httpContext));
        Assert.Equal("El alumno no existe.", resultado.Message);
    }

    [Fact]
    public async Task EliminarAlumnoSinTokenAsync()
    {
        _httpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal() // sin identidad
        };

        var resultado = await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _servicio.eliminarAsync(_httpContext));
    }

    [Fact]
    public async Task ObtenerAlumnoPorIdAsync()
    {
        _contexto.alumnos.Add(new alumno
        {
            idAlumno = 10,
            nombreCompleto = "Luis Daniel Tejeda Dorantes",
            nombreUsuario = "DanielTD_04",
            contrasenia = "iblNM3",
            correo = "tejedadaniel8oct@gmail.com",
            idGradoEstudios = 3
        });
        await _contexto.SaveChangesAsync();

        var alumno = await _servicio.obtenerPorIdAsync(10);
        Assert.NotNull(alumno);
        Assert.Equal("Luis Daniel Tejeda Dorantes", alumno.nombreCompleto);
    }

    [Fact]
    public async Task ObtenerAlumnoNoGuardadoAsync()
    {
        var resultado = await Assert.ThrowsAsync<RecursoNoEncontradoException>(
            () => _servicio.obtenerPorIdAsync(10));
        Assert.Equal("El alumno no existe.", resultado.Message);
    }

    [Fact]
    public async Task CambiarContraseniaAlumnoAsync()
    {
        _contexto.alumnos.Add(new alumno
        {
            idAlumno = 10,
            nombreCompleto = "Luis Daniel Tejeda Dorantes",
            nombreUsuario = "DanielTD_04",
            contrasenia = "iblNM3",
            correo = "tejedadaniel8oct@gmail.com",
            idGradoEstudios = 3
        });
        await _contexto.SaveChangesAsync();

        var claims = new List<Claim>
        {
            new Claim("idUsuario", "10"),
        };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var principal = new ClaimsPrincipal(identity);
        _httpContext = new DefaultHttpContext { User = principal };

        var cambioContraseniaDto = new CambiarContraseniaDTO
        {
            contraseniaActual = "iblNM3",
            contraseniaNueva = "xnmorfo1234@"
        };

        await _servicio.cambiarContraseniaAsync(cambioContraseniaDto, _httpContext);
        var alumnoActualizado = await _alumnoDAO.obtenerPorIdNormalAsync(10);

        Assert.NotNull(alumnoActualizado);
        Assert.Equal("xnmorfo1234@", alumnoActualizado.contrasenia);
    }

    [Fact]
    public async Task CambiarContraseniaAlumnoSinTokenAsync()
    {
        var cambioContraseniaDto = new CambiarContraseniaDTO
        {
            contraseniaActual = "iblNM3",
            contraseniaNueva = "xnmorfo1234@"
        };
        _httpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal() // sin identidad
        };

        var resultado = await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _servicio.cambiarContraseniaAsync(cambioContraseniaDto, _httpContext));
    }

    [Fact]
    public async Task CambiarContraseniaAlumnoConContraseniaIncorrectaAsync()
    {
        _contexto.alumnos.Add(new alumno
        {
            idAlumno = 10,
            nombreCompleto = "Luis Daniel Tejeda Dorantes",
            nombreUsuario = "DanielTD_04",
            contrasenia = "iblNM3",
            correo = "tejedadaniel8oct@gmail.com",
            idGradoEstudios = 3
        });
        await _contexto.SaveChangesAsync();

        var claims = new List<Claim>
        {
            new Claim("idUsuario", "10"),
        };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var principal = new ClaimsPrincipal(identity);
        _httpContext = new DefaultHttpContext { User = principal };

        var cambioContraseniaDto = new CambiarContraseniaDTO
        {
            contraseniaActual = "ib1NM3",
            contraseniaNueva = "xnmorfo1234@"
        };

        var resultado = await Assert.ThrowsAsync<ContraseniaDiferenteException>(
            () => _servicio.cambiarContraseniaAsync(cambioContraseniaDto, _httpContext));
        Assert.Equal("La contraseña actual es incorrecta.", resultado.Message);
    }
}
