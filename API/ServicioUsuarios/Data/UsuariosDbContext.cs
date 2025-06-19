using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ServicioUsuarios.Models;

namespace ServicioUsuarios.Data;

public partial class UsuariosDbContext : DbContext
{
    public UsuariosDbContext(DbContextOptions<UsuariosDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Alumno> Alumno { get; set; }

    public virtual DbSet<Docente> Docente { get; set; }

    public virtual DbSet<GradoEstudios> GradoEstudios { get; set; }

    public virtual DbSet<GradoProfesional> GradoProfesional { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Alumno>(entity =>
        {
            entity.HasKey(e => e.IdAlumno).HasName("PRIMARY");

            entity.HasOne(d => d.IdGradoEstudiosNavigation).WithMany(p => p.Alumno).HasConstraintName("alumno-grado");
        });

        modelBuilder.Entity<Docente>(entity =>
        {
            entity.HasKey(e => e.IdDocente).HasName("PRIMARY");

            entity.HasOne(d => d.IdGradoProfesionalNavigation).WithMany(p => p.Docente).HasConstraintName("docente-grado");
        });

        modelBuilder.Entity<GradoEstudios>(entity =>
        {
            entity.HasKey(e => e.IdGradoEstudios).HasName("PRIMARY");
        });

        modelBuilder.Entity<GradoProfesional>(entity =>
        {
            entity.HasKey(e => e.IdGradoProfesional).HasName("PRIMARY");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
