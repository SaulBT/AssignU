﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ServicioClases.Models;

namespace ServicioClases.Data;

public partial class ClasesDbContext : DbContext
{
    public ClasesDbContext(DbContextOptions<ClasesDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Clase> Clase { get; set; }

    public virtual DbSet<Registro> Registro { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        if (!optionsBuilder.IsConfigured){
            optionsBuilder.UseMySql("server=localhost;uid=clases_assignu;pwd=clase123;database=clases_bd_assignu", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.4.4-mysql"));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Clase>(entity =>
        {
            entity.HasKey(e => e.IdClase).HasName("PRIMARY");
        });

        modelBuilder.Entity<Registro>(entity =>
        {
            entity.HasKey(e => e.IdRegistro).HasName("PRIMARY");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
