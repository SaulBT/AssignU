using Microsoft.EntityFrameworkCore;
using ServicioUsuarios.Entities;
using ServicioUsuarios.Services.Interfaces;
using ServicioUsuarios.Services.Implementation;
using ServicioUsuarios.Middlewares;
using ServicioUsuarios.DAOs;
using ServicioUsuarios.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ServicioUsuarios.DAOs.Interfaces;
using ServicioUsuarios.Middlewares.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<usuarios_bd_assignuContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddScoped<IAlumnoService, AlumnoService>();
builder.Services.AddScoped<IAlumnoDAO, AlumnoDAO>();
builder.Services.AddScoped<IDocenteService, DocenteService>();
builder.Services.AddScoped<IDocenteDAO, DocenteDAO>();
builder.Services.AddScoped<ICatalogosService, CatalogosService>();
builder.Services.AddScoped<IGradoEstudiosDAO, GradoEstudiosDAO>();
builder.Services.AddScoped<IGradoProfesionalDAO, GradoProfesionalDAO>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IGeneradorToken, GeneradorToken>();

builder.Services.AddAuthentication(options => {    
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;    
}).AddJwtBearer(options => {
    var config = builder.Configuration;
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    var llave = config["JWTSettings:Key"];
    options.TokenValidationParameters = new TokenValidationParameters() {
        ValidIssuer = config["JWTSettings:Issuer"],
        ValidAudience = config["JWTSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(llave)),        
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});
builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseManejoExcepciones();
app.UseAuthentication();
app.UseAuthorization();

//AlumnoService
app.MapPost("/alumno", async (RegistrarAlumnoDTO alumnoNuevoDto, IAlumnoService servicio) =>
{
    var alumno = await servicio.registrarAsync(alumnoNuevoDto);
    return Results.Created($"/alumno/{alumno.idAlumno}", alumno);
})
.WithName("RegistrarAlumno")
.WithOpenApi();

app.MapGet("/alumno/{id:int}", async (int id, IAlumnoService servicio) =>
{
    var alumno = await servicio.obtenerPorIdAsync(id);
    return Results.Ok(alumno);
})
.WithName("ObtenerAlumnoPorId")
.WithOpenApi();

app.MapPut("/alumnos", async (HttpContext context, ActualizarAlumnoDTO alumnoActualizadoDTO, IAlumnoService servicio) =>
{
    await servicio.actualizarAsync(context, alumnoActualizadoDTO);
    return Results.Ok();
})
.WithName("ActualizarAlumno")
.RequireAuthorization()
.WithOpenApi();

app.MapDelete("/alumnos", async (HttpContext context, IAlumnoService servicio) =>
{
    await servicio.eliminarAsync(context);
    return Results.Ok();
})
.WithName("EliminarAlumno")
.RequireAuthorization()
.WithOpenApi();

app.MapPut("/alumnos/cambiar-contrasenia", async (CambiarContraseniaDTO cambiarContraseniaDto, IAlumnoService servicio, HttpContext context) =>
{
    await servicio.cambiarContraseniaAsync(cambiarContraseniaDto, context);
    return Results.Ok();
})
.WithName("CambiarContraseniaAlumno")
.RequireAuthorization()
.WithOpenApi();

//DocenteService
app.MapPost("/docentes", async (RegistrarDocenteDTO docenteNuevoDto, IDocenteService servicio) =>
{
    var docente = await servicio.registrarAsync(docenteNuevoDto);
    return Results.Created($"/docentes/{docente.idDocente}", docente);
})
.WithName("RegistrarDocente")
.WithOpenApi();

app.MapGet("/docentes/{id:int}", async (int id, IDocenteService servicio) =>
{
    var docente = await servicio.obtenerPorIdAsync(id);
    return Results.Ok(docente);
})
.WithName("ObtenerDocentePorId")
.WithOpenApi();

app.MapPut("/docentes", async (HttpContext context, ActualizarDocenteDTO docenteActualizadoDTO, IDocenteService servicio) =>
{
    await servicio.actualizarAsync(context, docenteActualizadoDTO);
    return Results.Ok();
})
.WithName("ActualizarDocente")
.RequireAuthorization()
.WithOpenApi();

app.MapDelete("/docentes", async (HttpContext context, IDocenteService servicio) =>
{
    await servicio.eliminarAsync(context);
    return Results.Ok();
})
.WithName("EliminarDocente")
.RequireAuthorization()
.WithOpenApi();

app.MapPut("/docentes/cambiar-contrasenia", async (CambiarContraseniaDTO cambiarContraseniaDto, IDocenteService servicio, HttpContext context) =>
{
    await servicio.cambiarContraseniaAsync(cambiarContraseniaDto, context);
    return Results.Ok();
})
.WithName("CambiarContraseniaDocente")
.RequireAuthorization()
.WithOpenApi();

//CatalogosService
app.MapGet("/catalogos/grados-estudios", async (ICatalogosService servicio) =>
{
    var gradosEstudios = await servicio.obtenerGradosEstudiosAsync();
    return Results.Ok(gradosEstudios);
})
.WithName("ObtenerGradosEstudios")
.WithOpenApi();

app.MapGet("/catalogos/grados-profesionales", async (ICatalogosService servicio) =>
{
    var gradosProfesionales = await servicio.obtenerGradosProfesionalesAsync();
    return Results.Ok(gradosProfesionales);
})
.WithName("ObtenerGradosProfesionales")
.WithOpenApi();

app.MapGet("/catalogos/grado-estudio/{id:int}", async (int id, ICatalogosService servicio) =>
{
    var gradoEstudio = await servicio.obtenerGradoEstudioPorIdAsync(id);
    return Results.Ok(gradoEstudio);
})
.WithName("ObtenerGradoEstudioPorId")
.WithOpenApi();

app.MapGet("/catalogos/grado-profesional/{id:int}", async (int id, ICatalogosService servicio) =>
{
    var gradoProfesional = await servicio.obtenerGradoProfesionalPorIdAsync(id);
    return Results.Ok(gradoProfesional);
})
.WithName("ObtenerGradoProfesionalPorId")
.WithOpenApi();

// LoginService
app.MapPost("/login", async (IniciarSesionDTO usuarioDto, ILoginService servicio) =>
{
    var resultado = await servicio.IniciarSesion(usuarioDto);
    return Results.Ok(resultado);
})
.WithName("IniciarSesion")
.WithOpenApi();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
