using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Solanum_back.Models
{
    public partial class SolanumContext : DbContext
    {
        public SolanumContext()
        {
        }

        public SolanumContext(DbContextOptions<SolanumContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Contrato> Contratos { get; set; } = null!;
        public virtual DbSet<Cultivo> Cultivos { get; set; } = null!;
        public virtual DbSet<Gasto> Gastos { get; set; } = null!;
        public virtual DbSet<Ingreso> Ingresos { get; set; } = null!;
        public virtual DbSet<Trabajadore> Trabajadores { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<Contrato>(entity =>
            {
                entity.HasKey(e => new { e.IdUsuario, e.IdTrabajador })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("contratos");

                entity.HasIndex(e => e.IdTrabajador, "id_trabajador");

                entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");

                entity.Property(e => e.IdTrabajador).HasColumnName("id_trabajador");

                entity.Property(e => e.Dato)
                    .HasMaxLength(1)
                    .HasColumnName("dato")
                    .IsFixedLength();

                entity.Property(e => e.Estado)
                    .HasMaxLength(1)
                    .HasColumnName("estado")
                    .IsFixedLength();

                entity.HasOne(d => d.IdTrabajadorNavigation)
                    .WithMany(p => p.Contratos)
                    .HasForeignKey(d => d.IdTrabajador)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("contratos_ibfk_2");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Contratos)
                    .HasForeignKey(d => d.IdUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("contratos_ibfk_1");
            });

            modelBuilder.Entity<Cultivo>(entity =>
            {
                entity.HasKey(e => e.IdCultivo)
                    .HasName("PRIMARY");

                entity.ToTable("cultivos");

                entity.HasIndex(e => e.IdUsuario, "id_usuario");

                entity.Property(e => e.IdCultivo).HasColumnName("id_cultivo");

                entity.Property(e => e.DescripcionCultivo)
                    .HasMaxLength(150)
                    .HasColumnName("descripcion_cultivo");

                entity.Property(e => e.Estado)
                    .HasMaxLength(1)
                    .HasColumnName("estado")
                    .IsFixedLength();

                entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");

                entity.Property(e => e.NombreCultivo)
                    .HasMaxLength(40)
                    .HasColumnName("nombre_cultivo");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Cultivos)
                    .HasForeignKey(d => d.IdUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("cultivos_ibfk_1");
            });

            modelBuilder.Entity<Gasto>(entity =>
            {
                entity.HasKey(e => e.IdGasto)
                    .HasName("PRIMARY");

                entity.ToTable("gastos");

                entity.HasIndex(e => e.IdCultivo, "id_cultivo");

                entity.HasIndex(e => e.IdTrabajador, "id_trabajador");

                entity.Property(e => e.IdGasto).HasColumnName("id_gasto");

                entity.Property(e => e.CalorUnidad).HasColumnName("calor_unidad");

                entity.Property(e => e.DescripcionGasto)
                    .HasMaxLength(150)
                    .HasColumnName("descripcion_gasto");

                entity.Property(e => e.Estado)
                    .HasMaxLength(1)
                    .HasColumnName("estado")
                    .IsFixedLength();

                entity.Property(e => e.EstadoPago)
                    .HasMaxLength(1)
                    .HasColumnName("estado_pago")
                    .IsFixedLength();

                entity.Property(e => e.Etapa)
                    .HasMaxLength(2)
                    .HasColumnName("etapa")
                    .IsFixedLength();

                entity.Property(e => e.FechaGasto).HasColumnName("fecha_gasto");

                entity.Property(e => e.IdCultivo).HasColumnName("id_cultivo");

                entity.Property(e => e.IdTrabajador).HasColumnName("id_trabajador");

                entity.Property(e => e.NombreGasto)
                    .HasMaxLength(40)
                    .HasColumnName("nombre_gasto");

                entity.Property(e => e.NumeroJorbul).HasColumnName("numero_jorbul");

                entity.Property(e => e.TipoGasto)
                    .HasMaxLength(2)
                    .HasColumnName("tipo_gasto")
                    .IsFixedLength();

                entity.Property(e => e.ValorGasto).HasColumnName("valor_gasto");

                entity.HasOne(d => d.IdCultivoNavigation)
                    .WithMany(p => p.Gastos)
                    .HasForeignKey(d => d.IdCultivo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("gastos_ibfk_1");

                entity.HasOne(d => d.IdTrabajadorNavigation)
                    .WithMany(p => p.Gastos)
                    .HasForeignKey(d => d.IdTrabajador)
                    .HasConstraintName("gastos_ibfk_2");
            });

            modelBuilder.Entity<Ingreso>(entity =>
            {
                entity.HasKey(e => e.IdIngreso)
                    .HasName("PRIMARY");

                entity.ToTable("ingresos");

                entity.HasIndex(e => e.IdCultivo, "id_cultivo");

                entity.Property(e => e.IdIngreso).HasColumnName("id_ingreso");

                entity.Property(e => e.Cantidad).HasColumnName("cantidad");

                entity.Property(e => e.DescripcionIngreso)
                    .HasMaxLength(150)
                    .HasColumnName("descripcion_ingreso");

                entity.Property(e => e.Estado)
                    .HasMaxLength(1)
                    .HasColumnName("estado")
                    .IsFixedLength();

                entity.Property(e => e.FechaIngreso).HasColumnName("fecha_ingreso");

                entity.Property(e => e.IdCultivo).HasColumnName("id_cultivo");

                entity.Property(e => e.NombreIngreso)
                    .HasMaxLength(40)
                    .HasColumnName("nombre_ingreso");

                entity.Property(e => e.ValorIngreso).HasColumnName("valor_ingreso");

                entity.HasOne(d => d.IdCultivoNavigation)
                    .WithMany(p => p.Ingresos)
                    .HasForeignKey(d => d.IdCultivo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ingresos_ibfk_1");
            });

            modelBuilder.Entity<Trabajadore>(entity =>
            {
                entity.HasKey(e => e.IdTrabajador)
                    .HasName("PRIMARY");

                entity.ToTable("trabajadores");

                entity.Property(e => e.IdTrabajador).HasColumnName("id_trabajador");

                entity.Property(e => e.Estado)
                    .HasMaxLength(1)
                    .HasColumnName("estado")
                    .IsFixedLength();

                entity.Property(e => e.NombreTrabajador)
                    .HasMaxLength(50)
                    .HasColumnName("nombre_trabajador");

                entity.Property(e => e.NumeroContacto)
                    .HasMaxLength(10)
                    .HasColumnName("numero_contacto");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario)
                    .HasName("PRIMARY");

                entity.ToTable("usuarios");

                entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");

                entity.Property(e => e.Apellidos)
                    .HasMaxLength(25)
                    .HasColumnName("apellidos");

                entity.Property(e => e.Contrasenia)
                    .HasMaxLength(64)
                    .HasColumnName("contrasenia");

                entity.Property(e => e.Estado)
                    .HasMaxLength(1)
                    .HasColumnName("estado")
                    .IsFixedLength();

                entity.Property(e => e.NombreUsuario)
                    .HasMaxLength(20)
                    .HasColumnName("nombre_usuario");

                entity.Property(e => e.Nombres)
                    .HasMaxLength(25)
                    .HasColumnName("nombres");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
