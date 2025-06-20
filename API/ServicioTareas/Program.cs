using Microsoft.EntityFrameworkCore;
using ServicioTareas.Data;
using ServicioTareas.Services.Interfaces;
using ServicioTareas.Services.Implementations;
using ServicioTareas.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ServicioTareas.Data.DAOs.Interfaces;
using ServicioTareas.Data.DAOs.Implementations;
using ServicioTareas.Data.DTOs;
using ServicioTareas.Config;
using ServicioTareas.BackgroundServices;
using ServicioTareas.Validations;
using ServicioTareas.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<TareasDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddScoped<IServicioTarea, ServicioTarea>();
builder.Services.AddScoped<ITareaDAO, TareaDAO>();
builder.Services.AddScoped<TareasValidaciones>();

builder.Services.AddSingleton<RpcClientRabbitMQ>();
builder.Services.AddHostedService<RabbitMqInitializer>();

builder.Services.AddSingleton<RpcServerRabbitMQ>();
builder.Services.AddHostedService<InicializadorRpcServer>();

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
app.UseSwagger();
app.UseSwaggerUI();

//ServicioTarea
app.MapPost("/tareas", async (IServicioTarea servicio, CrearTareaDTO crearTareaDto, HttpContext httpContext) =>
{
    var tarea = await servicio.CrearTareaAsync(crearTareaDto, httpContext);
    return Results.Created($"/clase/{tarea.IdClase}/{tarea.IdTarea}", tarea);
})
.WithName("Crear Tarea")
.WithTags("Tareas")
.WithSummary("Crear una nueva Tarea en el sistema.")
.WithDescription(
    "Crear una nueva Tarea con los siguientes datos:" +
    "\n - Id de la Clase" +
    "\n - Nombre de la Clase" +
    "\n - Fecha límite" +
    "\n - Cuestionario"
)
.Accepts<CrearTareaDTO>("application/json")
.Produces<TareaDTO>(201)
.Produces(400)
.Produces(401)
.Produces(409)
.RequireAuthorization()
.WithOpenApi();

app.MapPut("/tareas/{idTarea}", async (IServicioTarea servicio, EditarTareaDTO editarTareaDTO, int idTarea, HttpContext httpContext) =>
{
    var tarea = await servicio.EditarTareaAsync(editarTareaDTO, idTarea, httpContext);
    return Results.Ok(tarea);
})
.WithName("Editar Tarea")
.WithTags("Tareas")
.WithSummary("Editar una Tarea.")
.WithDescription(
    "Editar una Tarea con los siguientes datos:" +
    "\n - Id de la Tarea" +
    "\n - Nombre de la Clase" +
    "\n - Fecha límite" +
    "\n - Cuestionario"
)
.Accepts<CrearTareaDTO>("application/json")
.Produces<TareaDTO>(200)
.Produces(400)
.Produces(401)
.Produces(404)
.Produces(409)
.RequireAuthorization()
.WithOpenApi();

app.MapDelete("/tareas/{idTarea}/", async (IServicioTarea servicio, int idTarea, HttpContext httpContext) =>
{
    await servicio.EliminarTareaAsync(idTarea, httpContext);
    return Results.Accepted();
})
.WithName("Eliminar Tarea")
.WithTags("Tareas")
.WithSummary("Eliminar una Tarea.")
.WithDescription("Eliminar una Tarea por medio de la id.")
.Produces(202)
.Produces(400)
.Produces(401)
.Produces(404)
.RequireAuthorization()
.WithOpenApi();

app.MapGet("/clases/{idClase}/tareas", async (IServicioTarea servicio, int idClase) =>
{
    var tareas = await servicio.ObtenerTareasDeClaseAsync(idClase);
    return Results.Ok(tareas);
})
.WithName("Obtener Tareas de Clase")
.WithTags("Tareas")
.WithSummary("Obtener todas las Tareas de una Clase.")
.WithDescription("Obtener todas las Tareas de una Clase por medio de la id de la Clase")
.Produces<List<Tarea>>(200)
.Produces(400)
.Produces(404)
.WithOpenApi();

app.MapGet("/tarea/{idTarea}/estadisticas/", async (IServicioTarea servicio, int idTarea) =>
{
    var estadisticas = await servicio.ObtenerEstadisticasTareaAsync(idTarea);
    return Results.Ok(estadisticas);
})
.WithName("Obtener estadisticas de Tarea")
.WithTags("Tareas")
.WithSummary("Obtener las estadísticas de una Tarea.")
.WithDescription("Se obtienen todas las estadísticas de una Tarea por medio de su id")
.Produces<EstadisticasTareaDTO>(200)
.Produces(400)
.Produces(404)
.WithOpenApi();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.Run();
