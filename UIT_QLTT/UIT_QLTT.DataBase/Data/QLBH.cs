using Microsoft.EntityFrameworkCore;
using UIT_QLTT.Database.Models;

namespace UIT_QLTT.Database.Data;

public partial class QLBHContext : DbContext
{
    public QLBHContext()
    {
    }

    public QLBHContext(DbContextOptions<QLBHContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cthd> Cthds { get; set; }

    public virtual DbSet<Hoadon> Hoadons { get; set; }

    public virtual DbSet<Khachhang> Khachhangs { get; set; }

    public virtual DbSet<Khachhang1> Khachhang1s { get; set; }

    public virtual DbSet<Nhanvien> Nhanviens { get; set; }

    public virtual DbSet<Sanpham> Sanphams { get; set; }

    public virtual DbSet<Sanpham1> Sanpham1s { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cthd>(entity =>
        {
            entity.HasKey(e => new { e.Sohd, e.Masp }).HasName("PK__CTHD__91FD13E2035308BB");

            entity.ToTable("CTHD");

            entity.Property(e => e.Sohd).HasColumnName("SOHD");
            entity.Property(e => e.Masp)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("MASP");
            entity.Property(e => e.Sl).HasColumnName("SL");

            entity.HasOne(d => d.MaspNavigation).WithMany(p => p.Cthds)
                .HasForeignKey(d => d.Masp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CTHD_SP");

            entity.HasOne(d => d.SohdNavigation).WithMany(p => p.Cthds)
                .HasForeignKey(d => d.Sohd)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CTHD__SOHD__3F466844");
        });

        modelBuilder.Entity<Hoadon>(entity =>
        {
            entity.HasKey(e => e.Sohd).HasName("PK__HOADON__A7FF3B411CB5F25F");

            entity.ToTable("HOADON");

            entity.Property(e => e.Sohd)
                .ValueGeneratedNever()
                .HasColumnName("SOHD");
            entity.Property(e => e.Makh)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("MAKH");
            entity.Property(e => e.Manv)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("MANV");
            entity.Property(e => e.Nghd)
                .HasColumnType("smalldatetime")
                .HasColumnName("NGHD");
            entity.Property(e => e.Trigia)
                .HasColumnType("money")
                .HasColumnName("TRIGIA");

            entity.HasOne(d => d.MakhNavigation).WithMany(p => p.Hoadons)
                .HasForeignKey(d => d.Makh)
                .HasConstraintName("FK_HD_KH");

            entity.HasOne(d => d.ManvNavigation).WithMany(p => p.Hoadons)
                .HasForeignKey(d => d.Manv)
                .HasConstraintName("FK_HD_NV");
        });

        modelBuilder.Entity<Khachhang>(entity =>
        {
            entity.HasKey(e => e.Makh).HasName("PK__KHACHHAN__603F592C6EC482A5");

            entity.ToTable("KHACHHANG");

            entity.Property(e => e.Makh)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("MAKH");
            entity.Property(e => e.Dchi)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("DCHI");
            entity.Property(e => e.Doanhso)
                .HasColumnType("money")
                .HasColumnName("DOANHSO");
            entity.Property(e => e.Hoten)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("HOTEN");
            entity.Property(e => e.Loaikh)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("LOAIKH");
            entity.Property(e => e.Ngdk)
                .HasColumnType("smalldatetime")
                .HasColumnName("NGDK");
            entity.Property(e => e.Ngsinh)
                .HasColumnType("smalldatetime")
                .HasColumnName("NGSINH");
            entity.Property(e => e.Sodt)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("SODT");
        });

        modelBuilder.Entity<Khachhang1>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("KHACHHANG1");

            entity.Property(e => e.Dchi)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("DCHI");
            entity.Property(e => e.Doanhso)
                .HasColumnType("money")
                .HasColumnName("DOANHSO");
            entity.Property(e => e.Hoten)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("HOTEN");
            entity.Property(e => e.Loaikh)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("LOAIKH");
            entity.Property(e => e.Makh)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("MAKH");
            entity.Property(e => e.Ngdk)
                .HasColumnType("smalldatetime")
                .HasColumnName("NGDK");
            entity.Property(e => e.Ngsinh)
                .HasColumnType("smalldatetime")
                .HasColumnName("NGSINH");
            entity.Property(e => e.Sodt)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("SODT");
        });

        modelBuilder.Entity<Nhanvien>(entity =>
        {
            entity.HasKey(e => e.Manv).HasName("PK__NHANVIEN__603F511427E87670");

            entity.ToTable("NHANVIEN");

            entity.Property(e => e.Manv)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("MANV");
            entity.Property(e => e.Hoten)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("HOTEN");
            entity.Property(e => e.Ngvl)
                .HasColumnType("smalldatetime")
                .HasColumnName("NGVL");
            entity.Property(e => e.Sodt)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("SODT");
        });

        modelBuilder.Entity<Sanpham>(entity =>
        {
            entity.HasKey(e => e.Masp).HasName("PK__SANPHAM__60228A32D369F9C8");

            entity.ToTable("SANPHAM");

            entity.Property(e => e.Masp)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("MASP");
            entity.Property(e => e.Dvt)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("DVT");
            entity.Property(e => e.Gia)
                .HasColumnType("money")
                .HasColumnName("GIA");
            entity.Property(e => e.Nuocsx)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("NUOCSX");
            entity.Property(e => e.Tensp)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("TENSP");
        });

        modelBuilder.Entity<Sanpham1>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("SANPHAM1");

            entity.Property(e => e.Dvt)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("DVT");
            entity.Property(e => e.Gia)
                .HasColumnType("money")
                .HasColumnName("GIA");
            entity.Property(e => e.Masp)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("MASP");
            entity.Property(e => e.Nuocsx)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("NUOCSX");
            entity.Property(e => e.Tensp)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("TENSP");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
