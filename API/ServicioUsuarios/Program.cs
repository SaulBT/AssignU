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

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<usuarios_bd_assignuContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddScoped<IAlumnoService, AlumnoService>();
builder.Services.AddScoped<AlumnoDAO>();
builder.Services.AddScoped<IDocenteService, DocenteService>();
builder.Services.AddScoped<DocenteDAO>();
builder.Services.AddScoped<ICatalogosService, CatalogosService>();
builder.Services.AddScoped<GradoEstudiosDAO>();
builder.Services.AddScoped<GradoProfesionalDAO>();

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

//builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapControllers();
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

app.MapPut("/alumnos", async (int id, ActualizarAlumnoDTO alumnoActualizadoDTO, IAlumnoService servicio) =>
{
    await servicio.actualizarAsync(id, alumnoActualizadoDTO);
    return Results.Ok();
})
.WithName("ActualizarAlumno")
.WithOpenApi();

app.MapDelete("/alumnos", async (int id, IAlumnoService servicio) =>
{
     await servicio.eliminarAsync(id);
    return Results.Ok();
})
.WithName("EliminarAlumno")
.WithOpenApi();

//DocenteService
app.MapPost("/docente", async (RegistrarDocenteDTO docenteNuevoDto, IDocenteService servicio) =>
{
    var docente = await servicio.registrarAsync(docenteNuevoDto);
    return Results.Created($"/docente/{docente.idDocente}", docente);
})
.WithName("RegistrarDocente")
.WithOpenApi();

app.MapGet("/docente/{id:int}", async (int id, IDocenteService servicio) =>
{
    var docente = await servicio.obtenerPorIdAsync(id);
    return Results.Ok(docente);
})
.WithName("ObtenerDocentePorId")
.WithOpenApi();

app.MapPut("/docente", async (int id, ActualizarDocenteDTO docenteActualizadoDTO, IDocenteService servicio) =>
{
    await servicio.actualizarAsync(id, docenteActualizadoDTO);
    return Results.Ok();
})
.WithName("ActualizarDocente")
.WithOpenApi();

app.MapDelete("/docente", async (int id, IDocenteService servicio) =>
{
    await servicio.eliminarAsync(id);
    return Results.Ok();
})
.WithName("EliminarDocente")
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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
