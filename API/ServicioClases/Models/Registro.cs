using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServicioClases.Models;

[Table("registro")]
public partial class Registro
{
    [Key]
    [Column("idRegistro")]
    public int IdRegistro { get; set; }

    [Column("idAlumno")]
    public int IdAlumno { get; set; }

    [Column("idClase")]
    public int IdClase { get; set; }

    [Column("ultimoInicio", TypeName = "datetime")]
    public DateTime? UltimoInicio { get; set; }
}
