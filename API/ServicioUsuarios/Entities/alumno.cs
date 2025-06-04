using System;
using System.Collections.Generic;

namespace ServicioUsuarios.Entities;

public partial class alumno
{
    public int idAlumno { get; set; }

    public string? nombreCompleto { get; set; }

    public string? nombreUsuario { get; set; }

    public string? correo { get; set; }

    public string? contrasenia { get; set; }

    public int? idGradoEstudios { get; set; }

    public virtual grado_estudio? idGradoEstudiosNavigation { get; set; }
}
