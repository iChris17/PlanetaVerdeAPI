using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using PLANETAVERDE_API.Utilidades;

namespace PLANETAVERDE_API.Models
{
    public partial class PLANETAVERDEWEBContext : DbContext
    {
        public PLANETAVERDEWEBContext()
        {
        }

        public PLANETAVERDEWEBContext(DbContextOptions<PLANETAVERDEWEBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Categoria> Categoria { get; set; }
        public virtual DbSet<Noticia> Noticia { get; set; }
        public virtual DbSet<NoticiaCategoria> NoticiaCategoria { get; set; }
        public virtual DbSet<NoticiaDetalle> NoticiaDetalle { get; set; }
        public virtual DbSet<Usuarios> Usuarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
               optionsBuilder.UseSqlServer($"Server={Global.Connection.Host};Database={Global.Connection.Database};Trusted_Connection=True;User ID={Global.Connection.User};Password={Global.Connection.Password}");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.HasKey(e => e.IdCategoria);

                entity.ToTable("CATEGORIA");

                entity.Property(e => e.IdCategoria).HasColumnName("ID_CATEGORIA");

                entity.Property(e => e.DeCategoria)
                    .HasColumnName("DE_CATEGORIA")
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.FhRegistro)
                    .HasColumnName("FH_REGISTRO")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.NbCategoria)
                    .IsRequired()
                    .HasColumnName("NB_CATEGORIA")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NbCategoriaHeader)
                    .IsRequired()
                    .HasColumnName("NB_CATEGORIA_HEADER")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TpCategoria)
                    .HasColumnName("TP_CATEGORIA")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.UsRegistro)
                    .HasColumnName("US_REGISTRO")
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<Noticia>(entity =>
            {
                entity.HasKey(e => e.IdNoticiaHeader);

                entity.ToTable("NOTICIA");

                entity.Property(e => e.IdNoticiaHeader)
                    .HasColumnName("ID_NOTICIA_HEADER")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.DeNoticia)
                    .IsRequired()
                    .HasColumnName("DE_NOTICIA")
                    .IsUnicode(false);

                entity.Property(e => e.FhRegistro)
                    .HasColumnName("FH_REGISTRO")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.NbNoticia)
                    .IsRequired()
                    .HasColumnName("NB_NOTICIA")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UsRegistro)
                    .HasColumnName("US_REGISTRO")
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.VlImage)
                    .IsRequired()
                    .HasColumnName("VL_IMAGE")
                    .IsUnicode(false);
            });

            modelBuilder.Entity<NoticiaCategoria>(entity =>
            {
                entity.HasKey(e => new { e.IdCategoria, e.IdNoticiaHeader });

                entity.ToTable("NOTICIA_CATEGORIA");

                entity.Property(e => e.IdCategoria).HasColumnName("ID_CATEGORIA");

                entity.Property(e => e.IdNoticiaHeader)
                    .HasColumnName("ID_NOTICIA_HEADER")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.FhRegistro)
                    .HasColumnName("FH_REGISTRO")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.InPrincipal)
                    .HasColumnName("IN_PRINCIPAL")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.UsRegistro)
                    .HasColumnName("US_REGISTRO")
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.HasOne(d => d.IdCategoriaNavigation)
                    .WithMany(p => p.NoticiaCategoria)
                    .HasForeignKey(d => d.IdCategoria)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NOTICIACAT_CATEGORIA");

                entity.HasOne(d => d.IdNoticiaHeaderNavigation)
                    .WithMany(p => p.NoticiaCategoria)
                    .HasForeignKey(d => d.IdNoticiaHeader)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NOTICIACAT_NOTICIA");
            });

            modelBuilder.Entity<NoticiaDetalle>(entity =>
            {
                entity.HasKey(e => e.IdNoticiaHeader);

                entity.ToTable("NOTICIA_DETALLE");

                entity.Property(e => e.IdNoticiaHeader)
                    .HasColumnName("ID_NOTICIA_HEADER")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.FhRegistro)
                    .HasColumnName("FH_REGISTRO")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TxNoticia)
                    .IsRequired()
                    .HasColumnName("TX_NOTICIA")
                    .IsUnicode(false);

                entity.Property(e => e.UsRegistro)
                    .HasColumnName("US_REGISTRO")
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.VlImage)
                    .IsRequired()
                    .HasColumnName("VL_IMAGE")
                    .IsUnicode(false);

                entity.HasOne(d => d.IdNoticiaHeaderNavigation)
                    .WithOne(p => p.NoticiaDetalle)
                    .HasForeignKey<NoticiaDetalle>(d => d.IdNoticiaHeader)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NOTICIA_DETALLE");
            });

            modelBuilder.Entity<Usuarios>(entity =>
            {
                entity.HasKey(e => e.IdUsuario)
                    .HasName("PK_USUARIO");

                entity.ToTable("USUARIOS");

                entity.HasIndex(e => e.NbUsuario)
                    .HasName("UN_USUARIO")
                    .IsUnique();

                entity.Property(e => e.IdUsuario).HasColumnName("ID_USUARIO");

                entity.Property(e => e.FhRegistro)
                    .HasColumnName("FH_REGISTRO")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.NbUsuario)
                    .IsRequired()
                    .HasColumnName("NB_USUARIO")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.VlContraseña)
                    .IsRequired()
                    .HasColumnName("VL_CONTRASEÑA")
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
