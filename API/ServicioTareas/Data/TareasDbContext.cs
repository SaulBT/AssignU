using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ServicioTareas.Models;

namespace ServicioTareas.Data;

public partial class TareasDbContext : DbContext
{
    public TareasDbContext(DbContextOptions<TareasDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<EstadoTareaAlumno> EstadoTareaAlumno { get; set; }

    public virtual DbSet<Tarea> Tarea { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<EstadoTareaAlumno>(entity =>
        {
            entity.HasKey(e => e.IdEstadoTarea).HasName("PRIMARY");

            entity.HasOne(d => d.IdTareaNavigation).WithMany(p => p.EstadoTareaAlumno)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Estado-Tarea");
        });

        modelBuilder.Entity<Tarea>(entity =>
        {
            entity.HasKey(e => e.IdTarea).HasName("PRIMARY");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
