using System;
using System.Collections.Generic;

namespace ServicioUsuarios.Entities;

public partial class grado_profesional
{
    public int idGradoProfesional { get; set; }

    public string? nombre { get; set; }

    public virtual ICollection<docente> docentes { get; set; } = new List<docente>();
}
