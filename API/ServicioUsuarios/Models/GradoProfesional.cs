using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServicioUsuarios.Models;

[Table("grado_profesional")]
public partial class GradoProfesional
{
    [Key]
    [Column("idGradoProfesional")]
    public int IdGradoProfesional { get; set; }

    [Column("nombre")]
    [StringLength(45)]
    public string? Nombre { get; set; }

    [InverseProperty("IdGradoProfesionalNavigation")]
    public virtual ICollection<Docente> Docente { get; set; } = new List<Docente>();
}
