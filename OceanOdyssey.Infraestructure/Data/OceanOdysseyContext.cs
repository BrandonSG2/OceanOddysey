using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using OceanOdyssey.Infraestructure.Models;

namespace OceanOdyssey.Infraestructure.Data;

public partial class OceanOdysseyContext : DbContext
{
    public OceanOdysseyContext(DbContextOptions<OceanOdysseyContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Barco> Barco { get; set; }

    public virtual DbSet<BarcoHabitacion> BarcoHabitacion { get; set; }

    public virtual DbSet<Complemento> Complemento { get; set; }

    public virtual DbSet<Crucero> Crucero { get; set; }

    public virtual DbSet<FechaCrucero> FechaCrucero { get; set; }

    public virtual DbSet<Habitacion> Habitacion { get; set; }

    public virtual DbSet<Itinerario> Itinerario { get; set; }

    public virtual DbSet<Pais> Pais { get; set; }

    public virtual DbSet<Pasajero> Pasajero { get; set; }

    public virtual DbSet<PrecioHabitacion> PrecioHabitacion { get; set; }

    public virtual DbSet<Puerto> Puerto { get; set; }

    public virtual DbSet<ReservaComplemento> ReservaComplemento { get; set; }

    public virtual DbSet<ReservaHabitacion> ReservaHabitacion { get; set; }

    public virtual DbSet<ResumenReservacion> ResumenReservacion { get; set; }

    public virtual DbSet<Usuario> Usuario { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Barco>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Barco__3214EC27E1F5790D");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Descripcion).HasMaxLength(255);
            entity.Property(e => e.Nombre).HasMaxLength(100);

        });

        modelBuilder.Entity<BarcoHabitacion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BarcoHab__3214EC273F01F8D7");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Idbarco).HasColumnName("IDBarco");
            entity.Property(e => e.Idhabitacion).HasColumnName("IDHabitacion");

            entity.HasOne(d => d.IdbarcoNavigation).WithMany(p => p.BarcoHabitacion)
                .HasForeignKey(d => d.Idbarco)
                .HasConstraintName("FK__BarcoHabi__IDBar__30F848ED");

            entity.HasOne(d => d.IdhabitacionNavigation).WithMany(p => p.BarcoHabitacion)
                .HasForeignKey(d => d.Idhabitacion)
                .HasConstraintName("FK__BarcoHabi__IDHab__31EC6D26");
        });

        modelBuilder.Entity<Complemento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Compleme__3214EC27717F34AB");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Detalle).HasMaxLength(255);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Aplicado).HasMaxLength(50);
        });

        modelBuilder.Entity<Crucero>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Crucero__3214EC2711792854");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Costo).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Idbarco).HasColumnName("IDBarco");
            entity.Property(e => e.Nombre).HasMaxLength(100);

            entity.HasOne(d => d.IdbarcoNavigation).WithMany(p => p.Crucero)
                .HasForeignKey(d => d.Idbarco)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Crucero__IDBarco__37A5467C");
        });

        modelBuilder.Entity<FechaCrucero>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FechaCru__3214EC276A17533C");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Idcrucero).HasColumnName("IDCrucero");

            entity.HasOne(d => d.IdcruceroNavigation).WithMany(p => p.FechaCrucero)
                .HasForeignKey(d => d.Idcrucero)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FechaCrucero_Crucero");
        });

        modelBuilder.Entity<Habitacion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Habitaci__3214EC2711BB4008");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Detalles).HasMaxLength(255);
            entity.Property(e => e.Nombre).HasMaxLength(100);
         
        });

        modelBuilder.Entity<Itinerario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Itinerar__3214EC276EA37722");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Descripcion).HasMaxLength(255);
            entity.Property(e => e.Idcrucero).HasColumnName("IDCrucero");
            entity.Property(e => e.Idpuerto).HasColumnName("IDPuerto");

            entity.HasOne(d => d.IdcruceroNavigation).WithMany(p => p.Itinerario)
                .HasForeignKey(d => d.Idcrucero)
                .HasConstraintName("FK__Itinerari__IDCru__3E52440B");

            entity.HasOne(d => d.IdpuertoNavigation).WithMany(p => p.Itinerario)
                .HasForeignKey(d => d.Idpuerto)
                .HasConstraintName("FK__Itinerari__IDPue__3F466844");
        });

        modelBuilder.Entity<Pais>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Pais__3214EC2774CE1E1A");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<Pasajero>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Pasajero__3214EC278C2DD688");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Correo).HasMaxLength(500);
            entity.Property(e => e.Direccion).HasMaxLength(500);
            entity.Property(e => e.Idhabitacion).HasColumnName("IDHabitacion");
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Sexo).HasMaxLength(50);
            entity.Property(e => e.Telefono).HasMaxLength(20);

            entity.HasOne(d => d.IdhabitacionNavigation).WithMany(p => p.Pasajero)
                .HasForeignKey(d => d.Idhabitacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Pasajero__IDHabi__4D94879B");
        });

        modelBuilder.Entity<PrecioHabitacion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PrecioHa__3214EC27EC6AFD19");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Costo).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Idhabitacion).HasColumnName("IDHabitacion");

            entity.HasOne(d => d.IdFechaCruceroNavigation)
                .WithMany(p => p.PrecioHabitacion)
                .HasForeignKey(d => d.IdFechaCrucero)
                .HasConstraintName("FK_PrecioHabitacion_FechaCrucero");

            entity.HasOne(d => d.IdhabitacionNavigation)
                .WithMany(p => p.PrecioHabitacion)
                .HasForeignKey(d => d.Idhabitacion)
                .OnDelete(DeleteBehavior.ClientNoAction).IsRequired(false)
                .HasConstraintName("FK__PrecioHab__IDHab__34C8D9D1");
        });


        modelBuilder.Entity<Puerto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Puerto__3214EC27115904EF");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Idpais).HasColumnName("IDPais");
            entity.Property(e => e.Nombre).HasMaxLength(100);

            entity.HasOne(d => d.IdpaisNavigation).WithMany(p => p.Puerto)
                .HasForeignKey(d => d.Idpais)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Puerto__IDPais__2A4B4B5E");
        });

        modelBuilder.Entity<ReservaComplemento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ReservaC__3214EC27B1D904EE");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Idcomplemento).HasColumnName("IDComplemento");
            entity.Property(e => e.IdresumenReserva).HasColumnName("IDResumenReserva");

            entity.HasOne(d => d.IdcomplementoNavigation).WithMany(p => p.ReservaComplemento)
                .HasForeignKey(d => d.Idcomplemento)
                .HasConstraintName("FK__ReservaCo__IDCom__534D60F1");

            entity.HasOne(d => d.IdresumenReservaNavigation).WithMany(p => p.ReservaComplemento)
                .HasForeignKey(d => d.IdresumenReserva)
                .HasConstraintName("FK__ReservaCo__IDRes__52593CB8");
        });

        modelBuilder.Entity<ReservaHabitacion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ReservaH__3214EC274EBC79D0");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Idhabitacion).HasColumnName("IDHabitacion");
            entity.Property(e => e.Idpasajero).HasColumnName("IDPasajero");
            entity.Property(e => e.IdresumenReserva).HasColumnName("IDResumenReserva");

            entity.HasOne(d => d.IdhabitacionNavigation).WithMany(p => p.ReservaHabitacion)
                .HasForeignKey(d => d.Idhabitacion)
                .HasConstraintName("FK_ReservaHabitacion_Habitacion");

            entity.HasOne(d => d.IdpasajeroNavigation).WithMany(p => p.ReservaHabitacion)
                .HasForeignKey(d => d.Idpasajero)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReservaHabitacion_Pasajero");

            entity.HasOne(d => d.IdresumenReservaNavigation).WithMany(p => p.ReservaHabitacion)
                .HasForeignKey(d => d.IdresumenReserva)
                .HasConstraintName("FK_ReservaHabitacion_ResumenReservacion");
        });

        modelBuilder.Entity<ResumenReservacion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ResumenR__3214EC271F97EA63");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Estado).HasMaxLength(50);
            entity.Property(e => e.Idcrucero).HasColumnName("IDCrucero");
            entity.Property(e => e.Idusuario).HasColumnName("IDUsuario");
            entity.Property(e => e.Impuestos).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PrecioTotal).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TotalFinal).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TotalHabitaciones).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.FechaCruceroNavigation).WithMany(p => p.ResumenReservacion)
                .HasForeignKey(d => d.FechaCrucero)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ResumenReservacion_FechaCrucero");

            entity.HasOne(d => d.IdcruceroNavigation).WithMany(p => p.ResumenReservacion)
                .HasForeignKey(d => d.Idcrucero)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ResumenRe__IDCru__45F365D3");

            entity.HasOne(d => d.IdusuarioNavigation).WithMany(p => p.ResumenReservacion)
                .HasForeignKey(d => d.Idusuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ResumenRe__IDUsu__44FF419A");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Usuario__3214EC270029DEB8");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Clave).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Idpais).HasColumnName("IDPais");
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Rol).HasMaxLength(50);
            entity.Property(e => e.Telefono).HasMaxLength(20);

            entity.HasOne(d => d.IdpaisNavigation).WithMany(p => p.Usuario)
                .HasForeignKey(d => d.Idpais)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Usuario__IDPais__4222D4EF");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
