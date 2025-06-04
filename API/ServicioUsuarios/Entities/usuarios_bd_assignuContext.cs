using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace ServicioUsuarios.Entities;

public partial class usuarios_bd_assignuContext : DbContext
{
    public usuarios_bd_assignuContext()
    {
    }

    public usuarios_bd_assignuContext(DbContextOptions<usuarios_bd_assignuContext> options)
        : base(options)
    {
    }

    public virtual DbSet<alumno> alumnos { get; set; }

    public virtual DbSet<docente> docentes { get; set; }

    public virtual DbSet<grado_estudio> grado_estudios { get; set; }

    public virtual DbSet<grado_profesional> grado_profesionals { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        if (!optionsBuilder.IsConfigured){
            optionsBuilder.UseMySql("server=localhost;uid=usuarios_assignu;pwd=usuario123;database=usuarios_bd_assignu", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.4.4-mysql"));
        }
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<alumno>(entity =>
        {
            entity.HasKey(e => e.idAlumno).HasName("PRIMARY");

            entity.ToTable("alumno");

            entity.HasIndex(e => e.idGradoEstudios, "alumno-grado_idx");

            entity.HasIndex(e => e.contrasenia, "contrasenia_UNIQUE").IsUnique();

            entity.HasIndex(e => e.correo, "correo_UNIQUE").IsUnique();

            entity.Property(e => e.idAlumno).ValueGeneratedNever();
            entity.Property(e => e.contrasenia).HasMaxLength(64);
            entity.Property(e => e.correo).HasMaxLength(45);
            entity.Property(e => e.nombreCompleto).HasMaxLength(135);
            entity.Property(e => e.nombreUsuario).HasMaxLength(45);

            entity.HasOne(d => d.idGradoEstudiosNavigation).WithMany(p => p.alumnos)
                .HasForeignKey(d => d.idGradoEstudios)
                .HasConstraintName("alumno-grado");
        });

        modelBuilder.Entity<docente>(entity =>
        {
            entity.HasKey(e => e.idDocente).HasName("PRIMARY");

            entity.ToTable("docente");

            entity.HasIndex(e => e.contrasenia, "contrasenia_UNIQUE").IsUnique();

            entity.HasIndex(e => e.correo, "correo_UNIQUE").IsUnique();

            entity.HasIndex(e => e.idGradoProfesional, "docente-grado_idx");

            entity.Property(e => e.contrasenia).HasMaxLength(64);
            entity.Property(e => e.correo).HasMaxLength(45);
            entity.Property(e => e.nombreCompleto).HasMaxLength(135);
            entity.Property(e => e.nombreUsuario).HasMaxLength(45);

            entity.HasOne(d => d.idGradoProfesionalNavigation).WithMany(p => p.docentes)
                .HasForeignKey(d => d.idGradoProfesional)
                .HasConstraintName("docente-grado");
        });

        modelBuilder.Entity<grado_estudio>(entity =>
        {
            entity.HasKey(e => e.idGradoEstudios).HasName("PRIMARY");

            entity.Property(e => e.nombre).HasMaxLength(45);
        });

        modelBuilder.Entity<grado_profesional>(entity =>
        {
            entity.HasKey(e => e.idGradoProfesional).HasName("PRIMARY");

            entity.ToTable("grado_profesional");

            entity.Property(e => e.nombre).HasMaxLength(45);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
