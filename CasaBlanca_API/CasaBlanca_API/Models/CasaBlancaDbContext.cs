using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CasaBlanca_API.Models;

public partial class CasaBlancaDbContext : DbContext
{
    public CasaBlancaDbContext()
    {
    }

    public CasaBlancaDbContext(DbContextOptions<CasaBlancaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<EstadoOcupacion> EstadoOcupacions { get; set; }

    public virtual DbSet<Estatus> Estatuses { get; set; }

    public virtual DbSet<Inmueble> Inmuebles { get; set; }

    public virtual DbSet<RolUser> RolUsers { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<ViewGetCasa> ViewGetCasas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("name=DefultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EstadoOcupacion>(entity =>
        {
            entity.ToTable("EstadoOcupacion");

            entity.Property(e => e.DateAdded)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EstadoInicialOcupacion).HasMaxLength(50);
            entity.Property(e => e.LastUpadded).HasColumnType("datetime");
        });

        modelBuilder.Entity<Estatus>(entity =>
        {
            entity.ToTable("Estatus");

            entity.Property(e => e.DateUpdated).HasColumnType("datetime");
            entity.Property(e => e.DatedAdded)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Estatus1)
                .HasMaxLength(50)
                .HasColumnName("Estatus");
        });

        modelBuilder.Entity<Inmueble>(entity =>
        {
            entity.ToTable("Inmueble");

            entity.Property(e => e.ApellidosOcupante).HasMaxLength(50);
            entity.Property(e => e.ApellidosTitular).HasMaxLength(50);
            entity.Property(e => e.CelularOcupante).HasMaxLength(50);
            entity.Property(e => e.CelularTitular).HasMaxLength(50);
            entity.Property(e => e.CuotaDeMantenimientoBase).HasColumnType("money");
            entity.Property(e => e.DateAdded)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EmailOcupante).HasMaxLength(50);
            entity.Property(e => e.EmailTitular).HasMaxLength(50);
            entity.Property(e => e.LastUpdated).HasColumnType("datetime");
            entity.Property(e => e.NombreOcupante).HasMaxLength(50);
            entity.Property(e => e.NombreTitular).HasMaxLength(50);
            entity.Property(e => e.NumeroCasa).HasMaxLength(50);
            entity.Property(e => e.Observaciones).HasColumnType("text");
            entity.Property(e => e.Password).HasMaxLength(250);
            entity.Property(e => e.Ubicacion).HasMaxLength(50);
            entity.Property(e => e.Usuario).HasMaxLength(50);

            entity.HasOne(d => d.EstadoOcupacionNavigation).WithMany(p => p.Inmuebles)
                .HasForeignKey(d => d.EstadoOcupacion)
                .HasConstraintName("FK_Inmueble_EstadoOcupacion");
        });

        modelBuilder.Entity<RolUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_RolsUser");

            entity.ToTable("RolUser");

            entity.Property(e => e.DateAdded)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DateUpdated).HasColumnType("datetime");
            entity.Property(e => e.Rol).HasMaxLength(50);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("Usuario");

            entity.Property(e => e.ApellidosUsuario).HasMaxLength(100);
            entity.Property(e => e.CelularUsuario).HasMaxLength(50);
            entity.Property(e => e.DateAdded)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DateUpdated).HasColumnType("datetime");
            entity.Property(e => e.EmailUsuario).HasMaxLength(250);
            entity.Property(e => e.NombreUsuario).HasMaxLength(50);
            entity.Property(e => e.Password)
                .HasMaxLength(250)
                .HasColumnName("password");

            entity.HasOne(d => d.IdEstatusNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdEstatus)
                .HasConstraintName("FK_Usuario_Estatus1");

            entity.HasOne(d => d.IdInmuebleNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdInmueble)
                .HasConstraintName("FK_Usuario_Inmueble1");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .HasConstraintName("FK_Usuario_RolUser");
        });

        modelBuilder.Entity<ViewGetCasa>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_GetCasas");

            entity.Property(e => e.ApellidosOcupante).HasMaxLength(50);
            entity.Property(e => e.ApellidosTitular).HasMaxLength(50);
            entity.Property(e => e.CelularOcupante).HasMaxLength(50);
            entity.Property(e => e.CelularTitular).HasMaxLength(50);
            entity.Property(e => e.CuotaDeMantenimientoBase).HasColumnType("money");
            entity.Property(e => e.DateAdded).HasColumnType("datetime");
            entity.Property(e => e.EmailOcupante).HasMaxLength(50);
            entity.Property(e => e.EmailTitular).HasMaxLength(50);
            entity.Property(e => e.EstadoInicialOcupacion).HasMaxLength(50);
            entity.Property(e => e.LastUpdated).HasColumnType("datetime");
            entity.Property(e => e.NombreOcupante).HasMaxLength(50);
            entity.Property(e => e.NombreTitular).HasMaxLength(50);
            entity.Property(e => e.NumeroCasa).HasMaxLength(50);
            entity.Property(e => e.Observaciones).HasColumnType("text");
            entity.Property(e => e.Ubicacion).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
