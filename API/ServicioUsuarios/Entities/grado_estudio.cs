using System;
using System.Collections.Generic;

namespace ServicioUsuarios.Entities;

public partial class grado_estudio
{
    public int idGradoEstudios { get; set; }

    public string? nombre { get; set; }

    public virtual ICollection<alumno> alumnos { get; set; } = new List<alumno>();
}
