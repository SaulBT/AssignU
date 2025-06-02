namespace ServicioUsuarios.DTOs;

public class IniciarSesionDTO
{
    public string tipoUsuario { get; set; } // "alumno" o "docente"
    public string nombreUsuario { get; set; }
    public string contrasena { get; set; }
}