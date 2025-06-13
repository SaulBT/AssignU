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
using ServicioUsuarios.DAOs.Implementation;
using ServicioUsuarios.Config;
using ServicioUsuarios.BackgroundServices;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<usuarios_bd_assignuContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddScoped<IServicioAlumno, ServicioAlumno>();
builder.Services.AddScoped<IAlumnoDAO, AlumnoDAO>();
builder.Services.AddScoped<IServicioDocente, SerivicioDocente>();
builder.Services.AddScoped<IDocenteDAO, DocenteDAO>();
builder.Services.AddScoped<ServicioCatalogo, ServicioCatalogo>();
builder.Services.AddScoped<IGradoEstudiosDAO, GradoEstudiosDAO>();
builder.Services.AddScoped<IGradoProfesionalDAO, GradoProfesionalDAO>();
builder.Services.AddScoped<IServicioLogin, LoginService>();
builder.Services.AddScoped<GeneradorToken>();

builder.Services.AddSingleton<RpcServerRabbitMQ>();
builder.Services.AddHostedService<InicializadorRpcServer>();

builder.Services.AddSingleton<RpcClientRabbitMQ>();
builder.Services.AddHostedService<RabbitMqInitializer>();

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

//ServicioAlumno
app.MapPost("/alumno", async (RegistrarAlumnoDTO alumnoNuevoDto, IServicioAlumno servicio) =>
{
    var alumno = await servicio.RegistrarAsync(alumnoNuevoDto);
    return Results.Created($"/alumno/{alumno.idAlumno}", alumno);
})
.WithName("RegistrarAlumno")
.WithOpenApi();

app.MapGet("/alumno/{id:int}", async (int id, IServicioAlumno servicio) =>
{
    var alumno = await servicio.ObtenerPorIdAsync(id);
    return Results.Ok(alumno);
})
.WithName("ObtenerAlumnoPorId")
.WithOpenApi();

app.MapPut("/alumnos", async (HttpContext context, ActualizarAlumnoDTO alumnoActualizadoDTO, IServicioAlumno servicio) =>
{
    await servicio.ActualizarAsync(context, alumnoActualizadoDTO);
    return Results.Ok();
})
.WithName("ActualizarAlumno")
.RequireAuthorization()
.WithOpenApi();

app.MapDelete("/alumnos", async (HttpContext context, IServicioAlumno servicio) =>
{
    await servicio.EliminarAsync(context);
    return Results.Ok();
})
.WithName("EliminarAlumno")
.RequireAuthorization()
.WithOpenApi();

app.MapPut("/alumnos/cambiar-contrasenia", async (CambiarContraseniaDTO cambiarContraseniaDto, IServicioAlumno servicio, HttpContext context) =>
{
    await servicio.CambiarContraseniaAsync(cambiarContraseniaDto, context);
    return Results.Ok();
})
.WithName("CambiarContraseniaAlumno")
.RequireAuthorization()
.WithOpenApi();

app.MapGet("/alumno/{id:int}/estadisticas", async (HttpContext context, IServicioAlumno servicio) =>
{
    var estadisticas = await servicio.ObtenerEstadisticasPerfilAlumnoAsync(context);
    return Results.Ok(estadisticas);
})
.WithName("ObtenerPerfilAlumno")
.RequireAuthorization()
.WithOpenApi();

//DocenteService
app.MapPost("/docentes", async (RegistrarDocenteDTO docenteNuevoDto, IServicioDocente servicio) =>
{
    var docente = await servicio.RegistrarAsync(docenteNuevoDto);
    return Results.Created($"/docentes/{docente.idDocente}", docente);
})
.WithName("RegistrarDocente")
.WithOpenApi();

app.MapGet("/docentes/{id:int}", async (int id, IServicioDocente servicio) =>
{
    var docente = await servicio.ObtenerPorIdAsync(id);
    return Results.Ok(docente);
})
.WithName("ObtenerDocentePorId")
.WithOpenApi();

app.MapPut("/docentes", async (HttpContext context, ActualizarDocenteDTO docenteActualizadoDTO, IServicioDocente servicio) =>
{
    await servicio.ActualizarAsync(context, docenteActualizadoDTO);
    return Results.Ok();
})
.WithName("ActualizarDocente")
.RequireAuthorization()
.WithOpenApi();

app.MapDelete("/docentes", async (HttpContext context, IServicioDocente servicio) =>
{
    await servicio.EliminarAsync(context);
    return Results.Ok();
})
.WithName("EliminarDocente")
.RequireAuthorization()
.WithOpenApi();

app.MapPut("/docentes/cambiar-contrasenia", async (CambiarContraseniaDTO cambiarContraseniaDto, IServicioDocente servicio, HttpContext context) =>
{
    await servicio.CambiarContraseniaAsync(cambiarContraseniaDto, context);
    return Results.Ok();
})
.WithName("CambiarContraseniaDocente")
.RequireAuthorization()
.WithOpenApi();

//ServicioCatalogo
app.MapGet("/catalogos/grados-estudios", async (ServicioCatalogo servicio) =>
{
    var gradosEstudios = await servicio.ObtenerGradosEstudiosAsync();
    return Results.Ok(gradosEstudios);
})
.WithName("ObtenerGradosEstudios")
.WithOpenApi();

app.MapGet("/catalogos/grados-profesionales", async (ServicioCatalogo servicio) =>
{
    var gradosProfesionales = await servicio.ObtenerGradosProfesionalesAsync();
    return Results.Ok(gradosProfesionales);
})
.WithName("ObtenerGradosProfesionales")
.WithOpenApi();

app.MapGet("/catalogos/grado-estudio/{id:int}", async (int id, ServicioCatalogo servicio) =>
{
    var gradoEstudio = await servicio.ObtenerGradoEstudioPorIdAsync(id);
    return Results.Ok(gradoEstudio);
})
.WithName("ObtenerGradoEstudioPorId")
.WithOpenApi();

app.MapGet("/catalogos/grado-profesional/{id:int}", async (int id, ServicioCatalogo servicio) =>
{
    var gradoProfesional = await servicio.ObtenerGradoProfesionalPorIdAsync(id);
    return Results.Ok(gradoProfesional);
})
.WithName("ObtenerGradoProfesionalPorId")
.WithOpenApi();

// LoginService
app.MapPost("/login", async (IniciarSesionDTO usuarioDto, IServicioLogin servicio) =>
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
