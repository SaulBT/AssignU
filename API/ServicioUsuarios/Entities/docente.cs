using System;
using System.Collections.Generic;

namespace ServicioUsuarios.Entities;

public partial class docente
{
    public int idDocente { get; set; }

    public string? nombreCompleto { get; set; }

    public string? nombreUsuario { get; set; }

    public string? correo { get; set; }

    public string? contrasenia { get; set; }

    public int? idGradoProfesional { get; set; }

    public virtual grado_profesional? idGradoProfesionalNavigation { get; set; }
}
