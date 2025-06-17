using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServicioUsuarios.Models;

[Table("alumno")]
[Index("IdGradoEstudios", Name = "alumno-grado_idx")]
[Index("Correo", Name = "correo_UNIQUE", IsUnique = true)]
[Index("NombreUsuario", Name = "nombreUsuario_UNIQUE", IsUnique = true)]
public partial class Alumno
{
    [Key]
    [Column("idAlumno")]
    public int IdAlumno { get; set; }

    [Column("nombreCompleto")]
    [StringLength(135)]
    public string? NombreCompleto { get; set; }

    [Column("nombreUsuario")]
    [StringLength(45)]
    public string? NombreUsuario { get; set; }

    [Column("correo")]
    [StringLength(45)]
    public string? Correo { get; set; }

    [Column("contrasenia")]
    [StringLength(64)]
    public string? Contrasenia { get; set; }

    [Column("idGradoEstudios")]
    public int? IdGradoEstudios { get; set; }

    [ForeignKey("IdGradoEstudios")]
    [InverseProperty("Alumno")]
    public virtual GradoEstudios? IdGradoEstudiosNavigation { get; set; }
}
