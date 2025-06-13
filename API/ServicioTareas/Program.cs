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
using RabbitMQ.Client;
using ServicioTareas.Services;
using ServicioTareas.Config;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<TareasDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddScoped<IServicioTarea, ServicioTarea>();
builder.Services.AddScoped<ITareaDAO, TareaDAO>();
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

//ServicioTarea
app.MapPost("/clase/{codigo}", async (IServicioTarea servicio, CrearTareaDTO crearTareaDto, HttpContext httpContext) =>
{
    var tarea = await servicio.CrearTareaAsync(crearTareaDto, httpContext);
    return Results.Created($"/clase/{tarea.IdClase}/{tarea.IdTarea}", tarea);
})
.WithName("CrearTarea")
.RequireAuthorization()
.WithOpenApi();

app.MapPut("/clase/{codigo}/{idTarea:int}", async (IServicioTarea servicio, EditarTareaDTO editarTareaDTO, HttpContext httpContext) =>
{
    var tarea = await servicio.EditarTareaAsync(editarTareaDTO, httpContext);
    return Results.Ok(tarea);
})
.WithName("EditarTarea")
.RequireAuthorization()
.WithOpenApi();

app.MapDelete("/clase/{codigo}/", async (IServicioTarea servicio, int idTarea, HttpContext httpContext) =>
{
    await servicio.EliminarTareaAsync(idTarea, httpContext);
    return Results.Ok();
})
.WithName("EliminarTarea")
.RequireAuthorization()
.WithOpenApi();

app.MapGet("/clase/{codigo}/tareas", async (IServicioTarea servicio, int idClase) =>
{
    var tareas = await servicio.ObtenerTareasDeClaseAsync(idClase);
    return Results.Ok(tareas);
})
.WithName("ObtenerTareasDeClase")
.WithOpenApi();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.Run();
