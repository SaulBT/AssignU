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

builder.Services.AddScoped<IClasesService, ClasesService>();
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

//ClasesService
app.MapPost("/clase", async (IClasesService servicio, CrearClaseDTO crearClaseDto, HttpContext httpContext) =>
{
    var clase = await servicio.crearClaseAsync(crearClaseDto, httpContext);
    return Results.Created($"/clase/{clase.IdClase}", clase);
})
.WithName("CrearClase")
.RequireAuthorization()
.WithOpenApi();

app.MapPut("/clase", async (IClasesService servicio, ActualizarClaseDTO actualizarClaseDto, HttpContext httpContext) =>
{
    var clase = await servicio.editarClaseAsync(actualizarClaseDto, httpContext);
    return Results.Ok(clase);
})
.WithName("ActualizarClase")
.RequireAuthorization()
.WithOpenApi();

app.MapDelete("/clase/{id:int}", async (IClasesService servicio, int idClase, HttpContext httpContext) =>
{
    await servicio.eliminarClaseAsync(idClase, httpContext);
    return Results.Ok();
})
.WithName("EliminarClase")
.RequireAuthorization()
.WithOpenApi();

app.MapPut("/clase/fecha-visualizacion", async (IClasesService servicio, int idClase, DateTime fechaVisualizacion, HttpContext httpContext) =>
{
    await servicio.enviarFechaVisualizacionAsync(idClase, fechaVisualizacion, httpContext);
    return Results.Ok();
})
.WithName("EnviarFechaVisualizacion")
.RequireAuthorization()
.WithOpenApi();

app.MapGet("/clase/{id:int}", async (IClasesService servicio, int idClase) =>
{
    var clase = await servicio.obtenerClasePorIdAsync(idClase);
    return Results.Ok(clase);
})
.WithName("ObtenerClase")
.WithOpenApi();

app.MapGet("/alumno/{id:int}/clases", async (IClasesService servicio, HttpContext httpContext) =>
{
    var clases = await servicio.obtenerClasesDeAlumnoAsync(httpContext);
    return Results.Ok(clases);
})
.WithName("ObtenerClasesDeAlumno")
.RequireAuthorization()
.WithOpenApi();

app.MapGet("/docente/{id:int}/clases", async (IClasesService servicio, HttpContext httpContext) =>
{
    var clases = await servicio.obtenerClasesDeDocenteAsync(httpContext);
    return Results.Ok(clases);
})
.WithName("ObtenerClasesDeDocente")
.RequireAuthorization()
.WithOpenApi();

app.MapPost("/clase/unirme", async (IClasesService servicio, string codigoClase, HttpContext httpContext) =>
{
    var clase = await servicio.unirseAClaseAsync(codigoClase, httpContext);
    return Results.Created($"/clase/{clase.IdClase}", clase);
})
.WithName("UnirseAClase")
.RequireAuthorization()
.WithOpenApi();

app.MapDelete("/clase/{id:int}/salir", async (IClasesService servicio, int idClase, HttpContext httpContext) =>
{
    await servicio.salirDeClaseAsync(idClase, httpContext);
    return Results.Ok();
})
.WithName("SalirseClase")
.RequireAuthorization()
.WithOpenApi();

app.MapGet("/clase/alumnos/{id:int}", async (IClasesService servicio, int idAlumno, int idClase, HttpContext httpContext) =>
{
    var registro = await servicio.obtenerRegistroAlumno(idAlumno, idClase, httpContext);
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