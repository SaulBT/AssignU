using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServicioUsuarios.Models;

[Table("grado_estudios")]
public partial class GradoEstudios
{
    [Key]
    [Column("idGradoEstudios")]
    public int IdGradoEstudios { get; set; }

    [Column("nombre")]
    [StringLength(45)]
    public string? Nombre { get; set; }

    [InverseProperty("IdGradoEstudiosNavigation")]
    public virtual ICollection<Alumno> Alumno { get; set; } = new List<Alumno>();
}
