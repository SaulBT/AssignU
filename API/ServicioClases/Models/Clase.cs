using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServicioClases.Models;

[Table("clase")]
[Index("Codigo", Name = "codigo_UNIQUE", IsUnique = true)]
public partial class Clase
{
    [Key]
    [Column("idClase")]
    public int IdClase { get; set; }

    [Column("codigo")]
    [StringLength(45)]
    public string Codigo { get; set; } = null!;

    [Column("nombre")]
    [StringLength(45)]
    public string? Nombre { get; set; }

    [Column("idDocente")]
    public int IdDocente { get; set; }
}
