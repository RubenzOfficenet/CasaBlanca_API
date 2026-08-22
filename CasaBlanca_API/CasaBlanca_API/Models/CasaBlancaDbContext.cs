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

    public virtual DbSet<Concepto> Conceptos { get; set; }

    public virtual DbSet<ConceptoEgresoEvento> ConceptoEgresoEventos { get; set; }

    public virtual DbSet<Egreso> Egresos { get; set; }

    public virtual DbSet<EgresoEvento> EgresoEventos { get; set; }

    public virtual DbSet<EstadoOcupacion> EstadoOcupacions { get; set; }

    public virtual DbSet<Estatus> Estatuses { get; set; }

    public virtual DbSet<EstatusEvento> EstatusEventos { get; set; }

    public virtual DbSet<EventosIngreso> EventosIngresos { get; set; }

    public virtual DbSet<Ingreso> Ingresos { get; set; }

    public virtual DbSet<Inmueble> Inmuebles { get; set; }

    public virtual DbSet<RolUser> RolUsers { get; set; }

    public virtual DbSet<Ubicacion> Ubicacions { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<ViewGetCasa> ViewGetCasas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("name=DefultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Concepto>(entity =>
        {
            entity.ToTable("Concepto");

            entity.Property(e => e.Concepto1)
                .HasMaxLength(50)
                .HasColumnName("Concepto");
            entity.Property(e => e.DateAdded)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DateUpdated).HasColumnType("datetime");
            entity.Property(e => e.TipoConcepto).HasMaxLength(50);
        });

        modelBuilder.Entity<ConceptoEgresoEvento>(entity =>
        {
            entity.ToTable("ConceptoEgresoEvento");

            entity.Property(e => e.ConceptoEgreso).HasMaxLength(150);
            entity.Property(e => e.FechaAdd)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LastUpdated).HasColumnType("datetime");
        });

        modelBuilder.Entity<Egreso>(entity =>
        {
            entity.Property(e => e.Beneficiario).HasMaxLength(150);
            entity.Property(e => e.Concepto).HasMaxLength(150);
            entity.Property(e => e.DateAdded)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaEgreso).HasColumnType("datetime");
            entity.Property(e => e.LastUpdate).HasColumnType("datetime");
            entity.Property(e => e.Monto).HasColumnType("money");
            entity.Property(e => e.Observaciones).HasColumnType("text");
        });

        modelBuilder.Entity<EgresoEvento>(entity =>
        {
            entity.ToTable("EgresoEvento");

            entity.Property(e => e.AddDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaEgreso).HasColumnType("datetime");
            entity.Property(e => e.LastUpdated).HasColumnType("datetime");
            entity.Property(e => e.MontoEgreso).HasColumnType("money");
            entity.Property(e => e.Observaciones).HasColumnType("text");

            entity.HasOne(d => d.IdConceptoEgresoEventoNavigation).WithMany(p => p.EgresoEventos)
                .HasForeignKey(d => d.IdConceptoEgresoEvento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EgresoEvento_ConceptoEgresoEvento");
        });

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

        modelBuilder.Entity<EstatusEvento>(entity =>
        {
            entity.ToTable("EstatusEvento");

            entity.Property(e => e.DateAdded)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EstatusEvento1)
                .HasMaxLength(50)
                .HasColumnName("EstatusEvento");
            entity.Property(e => e.LastUpdated).HasColumnType("datetime");
        });

        modelBuilder.Entity<EventosIngreso>(entity =>
        {
            entity.ToTable("EventosIngreso");

            entity.Property(e => e.Apartado).HasColumnType("money");
            entity.Property(e => e.DateAdded)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DepositoGarantia).HasColumnType("money");
            entity.Property(e => e.FechaEvento).HasColumnType("datetime");
            entity.Property(e => e.FechaPago).HasColumnType("datetime");
            entity.Property(e => e.LastUpdate).HasColumnType("datetime");
            entity.Property(e => e.LimpiezaDomingo).HasColumnType("money");
            entity.Property(e => e.Liquida).HasColumnType("money");
            entity.Property(e => e.Luz).HasColumnType("money");
            entity.Property(e => e.RentaInmobiliario).HasColumnType("money");

            entity.HasOne(d => d.IdEstatusEventoNavigation).WithMany(p => p.EventosIngresos)
                .HasForeignKey(d => d.IdEstatusEvento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EventosIngreso_EstatusEvento");

            entity.HasOne(d => d.IdInmuebleNavigation).WithMany(p => p.EventosIngresos)
                .HasForeignKey(d => d.IdInmueble)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EventosIngreso_Inmueble");
        });

        modelBuilder.Entity<Ingreso>(entity =>
        {
            entity.Property(e => e.Borrado).HasDefaultValue(false);
            entity.Property(e => e.DateAdded)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DateUpdated).HasColumnType("datetime");
            entity.Property(e => e.FechaConcepto).HasColumnType("datetime");
            entity.Property(e => e.FechaRecepcion).HasColumnType("datetime");
            entity.Property(e => e.Monto).HasColumnType("money");
            entity.Property(e => e.Observaciones).HasColumnType("text");

            entity.HasOne(d => d.IdCasaNavigation).WithMany(p => p.Ingresos)
                .HasForeignKey(d => d.IdCasa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ingresos_Inmueble");

            entity.HasOne(d => d.IdConceptoNavigation).WithMany(p => p.Ingresos)
                .HasForeignKey(d => d.IdConcepto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ingresos_Concepto");
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
            entity.Property(e => e.Usuario).HasMaxLength(50);

            entity.HasOne(d => d.EstadoOcupacionNavigation).WithMany(p => p.Inmuebles)
                .HasForeignKey(d => d.EstadoOcupacion)
                .HasConstraintName("FK_Inmueble_EstadoOcupacion");

            entity.HasOne(d => d.IdUbicacionNavigation).WithMany(p => p.Inmuebles)
                .HasForeignKey(d => d.IdUbicacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Inmueble_Ubicacion");
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

        modelBuilder.Entity<Ubicacion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ubicaciones");

            entity.ToTable("Ubicacion");

            entity.Property(e => e.DatedAdded)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LastUpdated).HasColumnType("datetime");
            entity.Property(e => e.NombreUbicacion)
                .HasMaxLength(50)
                .HasDefaultValueSql("(getdate())");
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
