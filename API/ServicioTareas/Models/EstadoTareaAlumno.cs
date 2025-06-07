using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServicioTareas.Models;

[Table("estado_tarea_alumno")]
[Index("IdTarea", Name = "Estado-Tarea_idx")]
public partial class EstadoTareaAlumno
{
    [Key]
    [Column("idEstadoTarea")]
    public int IdEstadoTarea { get; set; }

    [Column("idAlumno")]
    public int IdAlumno { get; set; }

    [Column("idTarea")]
    public int IdTarea { get; set; }

    [Column("estado")]
    [StringLength(45)]
    public string Estado { get; set; } = null!;

    [Column("calificacion")]
    public float? Calificacion { get; set; }

    [ForeignKey("IdTarea")]
    [InverseProperty("EstadoTareaAlumno")]
    public virtual Tarea IdTareaNavigation { get; set; } = null!;
}
