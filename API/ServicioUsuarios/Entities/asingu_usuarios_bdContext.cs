using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ServicioUsuarios.Entities;

public partial class asingu_usuarios_bdContext : DbContext
{
    public asingu_usuarios_bdContext()
    {
    }

    public asingu_usuarios_bdContext(DbContextOptions<asingu_usuarios_bdContext> options)
        : base(options)
    {
    }

    public virtual DbSet<alumno> alumnos { get; set; }

    public virtual DbSet<docente> docentes { get; set; }

    public virtual DbSet<grado_estudio> grado_estudios { get; set; }

    public virtual DbSet<grado_profesional> grado_profesionals { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL("server=localhost;uid=asingu_usuarios;pwd=usuario123;database=asingu_usuarios_bd");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<alumno>(entity =>
        {
            entity.HasKey(e => e.idAlumno).HasName("PRIMARY");

            entity.ToTable("alumno");

            entity.HasIndex(e => e.contrasenia, "contrasenia_UNIQUE").IsUnique();

            entity.HasIndex(e => e.correo, "correo_UNIQUE").IsUnique();

            entity.HasIndex(e => e.idGradoEstudios, "idGradoEstudios_idx");

            entity.Property(e => e.contrasenia).HasMaxLength(64);
            entity.Property(e => e.correo).HasMaxLength(45);
            entity.Property(e => e.nombreCompleto).HasMaxLength(135);
            entity.Property(e => e.nombreUsuario).HasMaxLength(45);

            entity.HasOne(d => d.idGradoEstudiosNavigation).WithMany(p => p.alumnos)
                .HasForeignKey(d => d.idGradoEstudios)
                .HasConstraintName("idGradoEstudios");
        });

        modelBuilder.Entity<docente>(entity =>
        {
            entity.HasKey(e => e.idDocente).HasName("PRIMARY");

            entity.ToTable("docente");

            entity.HasIndex(e => e.contrasenia, "contrasenia_UNIQUE").IsUnique();

            entity.HasIndex(e => e.correo, "correo_UNIQUE").IsUnique();

            entity.HasIndex(e => e.idGradoProfesional, "idGradoProfesional_idx");

            entity.Property(e => e.contrasenia).HasMaxLength(64);
            entity.Property(e => e.correo).HasMaxLength(45);
            entity.Property(e => e.nombreCompleto).HasMaxLength(135);
            entity.Property(e => e.nombreUsuario).HasMaxLength(45);

            entity.HasOne(d => d.idGradoProfesionalNavigation).WithMany(p => p.docentes)
                .HasForeignKey(d => d.idGradoProfesional)
                .HasConstraintName("idGradoProfesional");
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
