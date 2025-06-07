using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServicioTareas.Models;

[Table("tarea")]
public partial class Tarea
{
    [Key]
    [Column("idTarea")]
    public int IdTarea { get; set; }

    [Column("idClase")]
    public int IdClase { get; set; }

    [Column("nombre")]
    [StringLength(45)]
    public string Nombre { get; set; } = null!;

    [Column("idArchivo")]
    public int IdArchivo { get; set; }

    [Column("fechaLimite", TypeName = "datetime")]
    public DateTime FechaLimite { get; set; }

    [Column("estado")]
    [StringLength(45)]
    public string Estado { get; set; } = null!;

    [InverseProperty("IdTareaNavigation")]
    public virtual ICollection<EstadoTareaAlumno> EstadoTareaAlumno { get; set; } = new List<EstadoTareaAlumno>();
}
