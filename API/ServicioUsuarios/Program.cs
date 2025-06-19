using Microsoft.EntityFrameworkCore;
using ServicioUsuarios.Services.Interfaces;
using ServicioUsuarios.Services.Implementation;
using ServicioUsuarios.Middlewares;
using ServicioUsuarios.Data.DAOs.Interfaces;
using ServicioUsuarios.Data.DAOs.Implementation;
using ServicioUsuarios.Data.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ServicioUsuarios.Config;
using ServicioUsuarios.BackgroundServices;
using ServicioUsuarios.Validations;
using ServicioUsuarios.Data.DTOs.Alumno;
using ServicioUsuarios.Data;
using ServicioUsuarios.Data.DTOs.Docente;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<UsuariosDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddScoped<IServicioAlumno, ServicioAlumno>();
builder.Services.AddScoped<IAlumnoDAO, AlumnoDAO>();
builder.Services.AddScoped<IServicioDocente, SerivicioDocente>();
builder.Services.AddScoped<IDocenteDAO, DocenteDAO>();
builder.Services.AddScoped<ServicioCatalogo, ServicioCatalogo>();
builder.Services.AddScoped<IGradoEstudiosDAO, GradoEstudiosDAO>();
builder.Services.AddScoped<IGradoProfesionalDAO, GradoProfesionalDAO>();
builder.Services.AddScoped<IServicioLogin, ServicioLogin>();
builder.Services.AddScoped<GeneradorToken>();
builder.Services.AddScoped<AlumnoValidaciones>();
builder.Services.AddScoped<DocenteValidaciones>();
builder.Services.AddScoped<LoginValidaciones>();

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
app.MapPost("/alumnos", async (RegistrarAlumnoDTO alumnoNuevoDto, IServicioAlumno servicio) =>
{
    await servicio.RegistrarAsync(alumnoNuevoDto);
    return Results.Created();
})
.WithName("Registrar Alumno")
.WithTags("Alumnos")
.WithSummary("Registrar un nuevo Alumno en el sistema")
.WithDescription(
    "Crea un Alumno con los datos: " +
    "\n - Nombre de completo." +
    "\n - Nombre usuario." +
    "\n - Correo electrónico." +
    "\n - Contraseña." +
    "\n - Id del último grado de estudios cursado.")
.Accepts<RegistrarAlumnoDTO>("application/json")
.Produces(201)
.Produces(400)
.Produces(409)
.WithOpenApi();

app.MapGet("/alumnos/{idAlumno}", async (int idAlumno, IServicioAlumno servicio) =>
{
    var alumno = await servicio.ObtenerAlumnoPorIdAsync(idAlumno);
    return Results.Ok(alumno);
})
.WithName("Obtener Alumno por id")
.WithTags("Alumnos")
.WithSummary("Obtiene un ALumno por medio de su id")
.WithDescription(
    "Obtiene un Alumno con los datos: " +
    "\n - Id del Alumno" +
    "\n - Nombre de completo." +
    "\n - Nombre usuario." +
    "\n - Correo electrónico." +
    "\n - Id del último grado de estudios cursado." +
    "\nCon la id del Alumno.")
.Produces<AlumnoDTO>(200)
.Produces(400)
.Produces(404)
.Produces(409)
.WithOpenApi();

app.MapPut("/alumnos/{idAlumno}", async (HttpContext context, int idAlumno, ActualizarAlumnoDTO alumnoActualizadoDTO, IServicioAlumno servicio) =>
{
    await servicio.ActualizarAsync(context, idAlumno, alumnoActualizadoDTO);
    return Results.Accepted();
})
.WithName("Actualizar Alumno")
.WithTags("Alumnos")
.WithSummary("Actualiza un Alumno con la id")
.WithDescription(
    "Actualiza un Alumno recibiendo lo siguientes datos:" +
    "\n - Id del Alumno" +
    "\n - Nombre completo" +
    "\n - Nombre de usuario" +
    "\n - Id del último grado de estudios" +
    "\nY devuelve los siguientes datos:" +
    "\n - Id del Alumno" +
    "\n - Nombre completo" +
    "\n - Nombre del usuario" +
    "\n - Correo electrónico" +
    "\n - Id del último grado de estudios")
.Accepts<ActualizarAlumnoDTO>("application/json")
.Produces(204)
.Produces(400)
.Produces(401)
.Produces(404)
.Produces(409)
.RequireAuthorization()
.WithOpenApi();

app.MapDelete("/alumnos/{idAlumno}", async (HttpContext context, int idAlumno, IServicioAlumno servicio) =>
{
    await servicio.EliminarAsync(context, idAlumno);
    return Results.Accepted();
})
.WithName("Eliminar Alumno")
.WithTags("Alumnos")
.WithSummary("Elimina un Alumno con la id")
.WithDescription("Elimina un Alumno con la id")
.Produces(204)
.Produces(400)
.Produces(401)
.Produces(404)
.RequireAuthorization()
.WithOpenApi();

app.MapPut("/alumnos/{idAlumno}/contrasenia", async (CambiarContraseniaDTO cambiarContraseniaDto, int idAlumno, IServicioAlumno servicio, HttpContext context) =>
{
    await servicio.CambiarContraseniaAsync(cambiarContraseniaDto, idAlumno, context);
    return Results.Accepted();
})
.WithName("Cambiar contrasenia de Alumno")
.WithTags("Alumnos")
.WithSummary("Cambia la contraseña de un Alumno")
.WithDescription("Cambia la contraseña de un Alumno")
.Accepts<CambiarContraseniaDTO>("application/json")
.Produces(204)
.Produces(400)
.Produces(401)
.Produces(409)
.RequireAuthorization()
.WithOpenApi();

app.MapGet("/alumnos/{idAlumno}/estadisticas", async (HttpContext context, int idAlumno, IServicioAlumno servicio) =>
{
    var estadisticas = await servicio.ObtenerEstadisticasPerfilAlumnoAsync(context, idAlumno);
    return Results.Ok(estadisticas);
})
.WithName("Obtener perfil de Alumno")
.WithTags("Alumnos")
.WithSummary("Obtener las estadísticas del Alumno")
.WithDescription(
    "Obtiene los siguientes datos de un Alumno:" +
    "\n - Datos de la cuenta del Alumno" +
    "\n - Clases del Alumno" +
    "\n - Tareas y calificación de cada Alumno")
.Produces<EstadisticasPerfilDTO>(200)
.Produces(400)
.Produces(401)
.Produces(404)
.Produces(409)
.RequireAuthorization()
.WithOpenApi();

//DocenteService
app.MapPost("/docentes", async (RegistrarDocenteDTO docenteNuevoDto, IServicioDocente servicio) =>
{
    await servicio.RegistrarAsync(docenteNuevoDto);
    return Results.Created();
})
.WithName("Registrar Docente")
.WithTags("Docentes")
.WithSummary("Registrar un nuevo Docente en el sistema")
.WithDescription(
    "Crea un Docente con los datos: " +
    "\n - Nombre completo." +
    "\n - Nombre usuario." +
    "\n - Correo electrónico." +
    "\n - Contraseña." +
    "\n - Id del último grado profesional obtenido.")
.Accepts<RegistrarDocenteDTO>("application/json")
.Produces(201)
.Produces(400)
.Produces(409)
.WithOpenApi();

app.MapGet("/docentes/{idDocente}", async (int idDocente, IServicioDocente servicio) =>
{
    var docente = await servicio.ObtenerDocentePorIdAsync(idDocente);
    return Results.Ok(docente);
})
.WithName("Obtener Docente por id")
.WithTags("Docentes")
.WithSummary("Obtiene un Docente por medio de su id")
.WithDescription(
    "Obtiene un Docente con los datos: " +
    "\n - Id del Docente" +
    "\n - Nombre completo." +
    "\n - Nombre usuario." +
    "\n - Correo electrónico." +
    "\n - Id del último grado de estudios cursado." +
    "\nCon la id del Docente.")
.Produces<DocenteDTO>(200)
.Produces(400)
.Produces(404)
.Produces(409)
.WithOpenApi();

app.MapPut("/docentes/{idDocente}", async (HttpContext context, int idDocente, ActualizarDocenteDTO docenteActualizadoDTO, IServicioDocente servicio) =>
{
    await servicio.ActualizarAsync(context, idDocente, docenteActualizadoDTO);
    return Results.Accepted();
})
.WithName("Actualizar Docente")
.WithTags("Docentes")
.WithSummary("Actualiza un Docente con la id")
.WithDescription(
    "Actualiza un Docente recibiendo lo siguientes datos:" +
    "\n - Id del Docente" +
    "\n - Nombre completo" +
    "\n - Nombre de usuario" +
    "\n - Id del último grado de estudios cursado." +
    "\nY devuelve los siguientes datos:" +
    "\n - Id del Docente" +
    "\n - Nombre completo" +
    "\n - Nombre del usuario" +
    "\n - Correo electrónico" +
    "\n - Id del último grado de estudios cursado.")
.Accepts<ActualizarDocenteDTO>("application/json")
.Produces(204)
.Produces(400)
.Produces(401)
.Produces(404)
.Produces(409)
.RequireAuthorization()
.WithOpenApi();

app.MapDelete("/docentes/{idDocente}", async (HttpContext context, int idDocente, IServicioDocente servicio) =>
{
    await servicio.EliminarAsync(context, idDocente);
    return Results.Accepted();
})
.WithName("EliminarDocente")
.WithTags("Docentes")
.WithSummary("Elimina un Docente con la id")
.WithDescription("Elimina un Docente con la id")
.Produces(204)
.Produces(400)
.Produces(401)
.Produces(404)
.RequireAuthorization()
.WithOpenApi();

app.MapPut("/docentes/{idDocente}/contrasenia", async (CambiarContraseniaDTO cambiarContraseniaDto, int idDocente, IServicioDocente servicio, HttpContext context) =>
{
    await servicio.CambiarContraseniaAsync(cambiarContraseniaDto, idDocente, context);
    return Results.Accepted();
})
.WithName("Cambiar contraseña de Docente")
.WithTags("Docentes")
.WithSummary("Cambia la contraseña de un Docente")
.WithDescription("Cambia la contraseña de un Docente")
.Accepts<CambiarContraseniaDTO>("application/json")
.Produces(204)
.Produces(400)
.Produces(401)
.Produces(409)
.RequireAuthorization()
.WithOpenApi();

//ServicioCatalogo
app.MapGet("/catalogos/grados-estudios", async (ServicioCatalogo servicio) =>
{
    var gradosEstudios = await servicio.ObtenerGradosEstudiosAsync();
    return Results.Ok(gradosEstudios);
})
.WithName("Obtener Grados de estudios")
.WithTags("Catalogos")
.WithSummary("Obtener grados de estudios")
.WithDescription(
    "Obtener grados de estudios:" +
    "\n - Primario" +
    "\n - Secundaria" +
    "\n - Bachillerato" +
    "\n - Universidad" +
    "\n - Maestría" +
    "\n - Doctorado" +
    "\n - Postdoctorado")
.Produces<List<GradoEstudiosDAO>>(200)
.Produces(404)
.WithOpenApi();

app.MapGet("/catalogos/grados-profesionales", async (ServicioCatalogo servicio) =>
{
    var gradosProfesionales = await servicio.ObtenerGradosProfesionalesAsync();
    return Results.Ok(gradosProfesionales);
})
.WithName("Obtener Grados Profesionales")
.WithTags("Catalogos")
.WithSummary("Obtener grados profesionales")
.WithDescription(
    "Obtener grados profesionales:" +
    "\n - Licenciatura" +
    "\n - Maestría" +
    "\n - Doctorado")
.Produces<List<GradoProfesionalDAO>>(200)
.Produces(404)
.WithOpenApi();

app.MapGet("/catalogos/grado-estudio/{idGradoEstudios}", async (int idGradoEstudios, ServicioCatalogo servicio) =>
{
    var gradoEstudio = await servicio.ObtenerGradoEstudioPorIdAsync(idGradoEstudios);
    return Results.Ok(gradoEstudio);
})
.WithName("Obtener un Grado de Estudio por id")
.WithTags("Catalogos")
.WithSummary("Obtener un Grado de Estudio")
.WithDescription("Obtener un Grado de Estudio")
.Produces<GradoEstudiosDAO>(200)
.Produces(404)
.WithOpenApi();

app.MapGet("/catalogos/grado-profesional/{idGradoProfesional}", async (int idGradoProfesional, ServicioCatalogo servicio) =>
{
    var gradoProfesional = await servicio.ObtenerGradoProfesionalPorIdAsync(idGradoProfesional);
    return Results.Ok(gradoProfesional);
})
.WithName("Obtener un Grado Profesional por id")
.WithTags("Catalogos")
.WithSummary("Obtener un grado profesional")
.WithDescription("Obtener un grado profesional")
.Produces<GradoProfesionalDAO>(200)
.Produces(404)
.WithOpenApi();

// LoginService
app.MapPost("/login", async (IniciarSesionDTO usuarioDto, IServicioLogin servicio) =>
{
    var resultado = await servicio.IniciarSesion(usuarioDto);
    return Results.Ok(resultado);
})
.WithName("Iniciar Sesion")
.WithTags("Login")
.WithSummary("Logearse como Alumno o Docente")
.WithDescription(
    "Logearse con los siguientes datos:" +
    "\n - Tipo de usuario" +
    "\n - Nombre de usuario o correo electrónico" +
    "\n - Contraseña")
.Produces<AlumnoDTO>(200)
.Produces<DocenteDTO>(200)
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
