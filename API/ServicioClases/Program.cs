using Microsoft.EntityFrameworkCore;
using ServicioClases.Data;
using ServicioClases.Services.Interfaces;
using ServicioClases.Services.Implementations;
using ServicioClases.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ServicioClases.Data.DAOs.Interfaces;
using ServicioClases.Data.DAOs.Implementations;
using ServicioClases.Data.DTOs;
using ServicioClases.Config;
using ServicioClases.BackgroundService;
using ServicioUsuarios.BackgroundServices;
using ServicioClases.Models;
using ServicioClases.Validations;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ClasesDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddScoped<IServicioClase, ServicioClase>();
builder.Services.AddScoped<IClaseDAO, ClaseDAO>();
builder.Services.AddScoped<IRegistroDAO, RegistroDAO>();
builder.Services.AddScoped<ClaseValidaciones>();

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

//ServicioClase
app.MapPost("/clases", async (IServicioClase servicio, CrearClaseDTO crearClaseDto, HttpContext httpContext) =>
{
    var clase = await servicio.CrearClaseAsync(crearClaseDto, httpContext);
    return Results.Created($"/clase/{clase.CodigoClase}", clase);
})
.WithName("Crear Clase")
.WithTags("Clases")
.WithSummary("Crear una nueva Clase en el sistema")
.WithDescription(
    "Crea una Clase con los datos: " +
    "\n - Nombre de la Clase." +
    "\n - Id del Docente obtenido del JWT.")
.Accepts<CrearClaseDTO>("application/json")
.Produces<ClaseDTO>(201)
.Produces(400)
.Produces(401)
.RequireAuthorization()
.WithOpenApi();

app.MapPut("/clases/{codigo-clase}", async (IServicioClase servicio, ActualizarClaseDTO actualizarClaseDto, HttpContext httpContext) =>
{
    var clase = await servicio.EditarClaseAsync(actualizarClaseDto, httpContext);
    return Results.Ok(clase);
})
.WithName("Actualizar Clase")
.WithTags("Clases")
.WithSummary("Actualizar una Clase")
.WithDescription(
    "Actualiza una Clase con los datos:" +
    "\n - Id de la Clase" +
    "\n - Nombre de la Clase" +
    "\n - Id del Docente obtenido del JWT.")
.Accepts<ActualizarClaseDTO>("application/json")
.Produces<ClaseDTO>(200)
.Produces(400)
.Produces(401)
.Produces(404)
.RequireAuthorization()
.WithOpenApi();

app.MapDelete("/clases/{codigo-clase}", async (IServicioClase servicio, int idClase, HttpContext httpContext) =>
{
    await servicio.EliminarClaseAsync(idClase, httpContext);
    return Results.Ok(202);
})
.WithName("Eliminar Clase")
.WithTags("Clases")
.WithSummary("Eliminar una Clase")
.WithDescription("Eliminar una Clase con su id")
.Produces(202)
.Produces(400)
.Produces(401)
.Produces(404)
.RequireAuthorization()
.WithOpenApi();

app.MapPut("/clases/{codigo-clase}/ultima-conexion", async (IServicioClase servicio, int idClase, DateTime fechaVisualizacion, HttpContext httpContext) =>
{
    await servicio.EnviarFechaVisualizacionAsync(idClase, fechaVisualizacion, httpContext);
    return Results.Ok(202);
})
.WithName("Enviar fecha ultima conexion")
.WithTags("Clases")
.WithSummary("Actualizar la última conexión")
.WithDescription(
    "Se actualiza la última conexión a una Clase con los siguientes datos:" +
    "\n - Id de la Clase" +
    "\n - Fecha de última conexión" +
    "\n - Id del Docente obtenido del JWT")
.Produces(202)
.Produces(400)
.Produces(401)
.Produces(404)
.RequireAuthorization()
.WithOpenApi();

app.MapGet("/clases/{codigo-clase}", async (IServicioClase servicio, int idClase) =>
{
    var clase = await servicio.ObtenerClasePorIdAsync(idClase);
    return Results.Ok(clase);
})
.WithName("Obtener Clase")
.WithTags("Clases")
.WithSummary("Obtener una Clase")
.WithDescription("Se obtiene una Clase con su id")
.Produces<ClaseDTO>(200)
.Produces(400)
.Produces(404)
.WithOpenApi();

app.MapGet("/alumnos/{nombre-usuario}/clases", async (IServicioClase servicio, HttpContext httpContext) =>
{
    var clases = await servicio.ObtenerClasesDeAlumnoAsync(httpContext);
    return Results.Ok(clases);
})
.WithName("Obtener Clases de Alumno")
.WithTags("Clases")
.WithSummary("Obtener las Clases de un Alumno")
.WithDescription("Se obtiene todas las Clases a las que un Alumno está inscrito por medio de su id obtenida del JWT")
.Produces<List<Clase>>(200)
.Produces(400)
.Produces(401)
.Produces(404)
.RequireAuthorization()
.WithOpenApi();

app.MapGet("/docentes/{nombre-usuario}/clases", async (IServicioClase servicio, HttpContext httpContext) =>
{
    var clases = await servicio.ObtenerClasesDeDocenteAsync(httpContext);
    return Results.Ok(clases);
})
.WithName("Obtener Clases de Docente")
.WithTags("Clases")
.WithSummary("Obtener las Clases de un Docente")
.WithDescription("Se obtiene todas las Clases en las que un Docente enseña por medio de su id obtenida del JWT")
.Produces<List<Clase>>(200)
.Produces(400)
.Produces(401)
.Produces(404)
.RequireAuthorization()
.WithOpenApi();

app.MapPost("/clases/{codigo-clase}/unirse", async (IServicioClase servicio, string codigoClase, HttpContext httpContext) =>
{
    var clase = await servicio.UnirseAClaseAsync(codigoClase, httpContext);
    return Results.Created($"/clase/{clase.IdClase}", clase);
})
.WithName("Unirse a Clase")
.WithTags("Clases")
.WithSummary("Unirse a una Clase")
.WithDescription(
    "Un Alumno se une a una Clase con los siguientes datos:" +
    "\n - Id del Alumno obtenido por el JWT" +
    "\n - Código de la Clase")
.Produces(202)
.Produces(400)
.Produces(401)
.Produces(404)
.Produces(409)
.RequireAuthorization()
.WithOpenApi();

app.MapDelete("/alumnos/{nombre-usuario}/clases/{codigo-clase}/salir", async (IServicioClase servicio, int idClase, HttpContext httpContext) =>
{
    await servicio.SalirDeClaseAsync(idClase, httpContext);
    return Results.Ok();
})
.WithName("Salirse de Clase")
.WithTags("Clases")
.WithSummary("Salirse de una Clase")
.WithDescription(
    "Un Alumno se sale de una Clase con los siguientes datos:" +
    "\n - Id del Alumno obtenido por el JWT" +
    "\n - Id de la Clase")
.Produces(202)
.Produces(400)
.Produces(401)
.Produces(404)
.RequireAuthorization()
.WithOpenApi();

app.MapGet("/alumnos/{nombre-usuario}/clases/{codigo-clase}", async (IServicioClase servicio, int idAlumno, int idClase, HttpContext httpContext) =>
{
    var registro = await servicio.ObtenerRegistroAlumno(idAlumno, idClase, httpContext);
    return Results.Ok(registro);
})
.WithName("Obtener Registro de Alumno")
.WithTags("Clases")
.WithSummary("Obtener el Registro de un Alumno en una Clase")
.WithDescription(
    "Obtener el Registro de un Aalumno en una Clase con los siguientes datos:" +
    "\n - Id del Alumno" +
    "\n - Id de la Clase")
.Produces<Registro>(200)
.Produces(400)
.Produces(401)
.Produces(404)
.RequireAuthorization()
.WithOpenApi();

app.MapGet("/clases/{codigo-clase}/estadisticas", async (IServicioClase servicio, int idClase) =>
{
    var estadísticas = await servicio.ObtenerEstadisticasDeLaClase(idClase);
    return Results.Ok(estadísticas);
})
.WithName("ObtenerEstadistica")
.WithTags("Clases")
.WithSummary("Obtener las estadísticas de una Clase")
.WithDescription("Se obtiene las estadísticas de una Clases por medio de su id")
.Produces<EstadisticasClaseDTO>(200)
.Produces(400)
.Produces(404)
.WithOpenApi();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();