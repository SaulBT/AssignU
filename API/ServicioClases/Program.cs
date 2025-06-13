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

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ClasesDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddScoped<IServicioClase, ServicioClase>();
builder.Services.AddScoped<IClaseDAO, ClaseDAO>();
builder.Services.AddScoped<IRegistroDAO, RegistroDAO>();

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
app.MapPost("/clase", async (IServicioClase servicio, CrearClaseDTO crearClaseDto, HttpContext httpContext) =>
{
    var clase = await servicio.CrearClaseAsync(crearClaseDto, httpContext);
    return Results.Created($"/clase/{clase.IdClase}", clase);
})
.WithName("CrearClase")
.RequireAuthorization()
.WithOpenApi();

app.MapPut("/clase", async (IServicioClase servicio, ActualizarClaseDTO actualizarClaseDto, HttpContext httpContext) =>
{
    var clase = await servicio.EditarClaseAsync(actualizarClaseDto, httpContext);
    return Results.Ok(clase);
})
.WithName("ActualizarClase")
.RequireAuthorization()
.WithOpenApi();

app.MapDelete("/clase/{id:int}", async (IServicioClase servicio, int idClase, HttpContext httpContext) =>
{
    await servicio.EliminarClaseAsync(idClase, httpContext);
    return Results.Ok();
})
.WithName("EliminarClase")
.RequireAuthorization()
.WithOpenApi();

app.MapPut("/clase/fecha-visualizacion", async (IServicioClase servicio, int idClase, DateTime fechaVisualizacion, HttpContext httpContext) =>
{
    await servicio.EnviarFechaVisualizacionAsync(idClase, fechaVisualizacion, httpContext);
    return Results.Ok();
})
.WithName("EnviarFechaVisualizacion")
.RequireAuthorization()
.WithOpenApi();

app.MapGet("/clase/{id:int}", async (IServicioClase servicio, int idClase) =>
{
    var clase = await servicio.ObtenerClasePorIdAsync(idClase);
    return Results.Ok(clase);
})
.WithName("ObtenerClase")
.WithOpenApi();

app.MapGet("/alumno/{id:int}/clases", async (IServicioClase servicio, HttpContext httpContext) =>
{
    var clases = await servicio.ObtenerClasesDeAlumnoAsync(httpContext);
    return Results.Ok(clases);
})
.WithName("ObtenerClasesDeAlumno")
.RequireAuthorization()
.WithOpenApi();

app.MapGet("/docente/{id:int}/clases", async (IServicioClase servicio, HttpContext httpContext) =>
{
    var clases = await servicio.ObtenerClasesDeDocenteAsync(httpContext);
    return Results.Ok(clases);
})
.WithName("ObtenerClasesDeDocente")
.RequireAuthorization()
.WithOpenApi();

app.MapPost("/clase/unirme", async (IServicioClase servicio, string codigoClase, HttpContext httpContext) =>
{
    var clase = await servicio.UnirseAClaseAsync(codigoClase, httpContext);
    return Results.Created($"/clase/{clase.IdClase}", clase);
})
.WithName("UnirseAClase")
.RequireAuthorization()
.WithOpenApi();

app.MapDelete("/clase/{id:int}/salir", async (IServicioClase servicio, int idClase, HttpContext httpContext) =>
{
    await servicio.SalirDeClaseAsync(idClase, httpContext);
    return Results.Ok();
})
.WithName("SalirseClase")
.RequireAuthorization()
.WithOpenApi();

app.MapGet("/clase/alumnos/{id:int}", async (IServicioClase servicio, int idAlumno, int idClase, HttpContext httpContext) =>
{
    var registro = await servicio.ObtenerRegistroAlumno(idAlumno, idClase, httpContext);
    return Results.Ok(registro);
})
.WithName("ObtenerRegistroDeAlumno")
.RequireAuthorization()
.WithOpenApi();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();