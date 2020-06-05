using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

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
        public virtual DbSet<NoticiaDetalle> NoticiaDetalle { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.\\;Database=PLANETAVERDEWEB;Trusted_Connection=True;");
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

                entity.Property(e => e.IdCategoria).HasColumnName("ID_CATEGORIA");

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

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
