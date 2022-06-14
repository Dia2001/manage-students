using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace QLSV.Models
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<DangNhap> DangNhaps { get; set;}
        public virtual DbSet<Diem> Diems { get; set;}
        public virtual DbSet<GiangVien> GiangViens { get; set;}
        public virtual DbSet<Khoa> Khoas { get; set;}
        public virtual DbSet<Lop> Lops { get; set; }
        public virtual DbSet<MonHoc> MonHocs { get; set; }
        public virtual DbSet<Nganh> Nganhs { get; set; }
        public virtual DbSet<SinhVien> SinhViens { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DangNhap>()
                .Property(e => e.TenDN)
                .IsUnicode(false);

            modelBuilder.Entity<DangNhap>()
                .Property(e => e.MatKhau)
                .IsUnicode(false);

            modelBuilder.Entity<DangNhap>()
                .Property(e => e.HinhAnh)
                .IsUnicode(false);

            modelBuilder.Entity<DangNhap>()
                .Property(e => e.SDT)
                .IsUnicode(false);

            modelBuilder.Entity<DangNhap>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Diem>()
                .Property(e => e.MaSV)
                .IsUnicode(false);

            modelBuilder.Entity<Diem>()
                .Property(e => e.MaMH)
                .IsUnicode(false);

            modelBuilder.Entity<Diem>()
                .Property(e => e.DiemChu)
                .IsUnicode(false);

            modelBuilder.Entity<Diem>()
                .Property(e => e.KetQua)
                .IsUnicode(false);

            modelBuilder.Entity<Diem>()
                .Property(e => e.NamHoc)
                .IsUnicode(false);

            modelBuilder.Entity<Diem>()
                .Property(e => e.HocKy)
                .IsUnicode(false);

            modelBuilder.Entity<GiangVien>()
                .Property(e => e.MaGV)
                .IsUnicode(false);

            modelBuilder.Entity<GiangVien>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<GiangVien>()
                .Property(e => e.SDT)
                .IsUnicode(false);

            modelBuilder.Entity<GiangVien>()
                .Property(e => e.MatKhau)
                .IsUnicode(false);

            modelBuilder.Entity<GiangVien>()
                .Property(e => e.MaKhoa)
                .IsUnicode(false);

            modelBuilder.Entity<Khoa>()
                .Property(e => e.MaKhoa)
                .IsUnicode(false);

            modelBuilder.Entity<Khoa>()
                .Property(e => e.SDT)
                .IsUnicode(false);

            modelBuilder.Entity<Lop>()
                .Property(e => e.MaLop)
                .IsUnicode(false);

            modelBuilder.Entity<Lop>()
                .Property(e => e.MaNganh)
                .IsUnicode(false);

            modelBuilder.Entity<MonHoc>()
                .Property(e => e.MaMH)
                .IsUnicode(false);

            modelBuilder.Entity<MonHoc>()
                .Property(e => e.MaKhoa)
                .IsUnicode(false);

            modelBuilder.Entity<MonHoc>()
                .HasMany(e => e.Diems)
                .WithRequired(e => e.MonHoc)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Nganh>()
                .Property(e => e.MaNganh)
                .IsUnicode(false);

            modelBuilder.Entity<Nganh>()
                .Property(e => e.MaKhoa)
                .IsUnicode(false);

            modelBuilder.Entity<Nganh>()
                .Property(e => e.TongSTC)
                .IsUnicode(false);

            modelBuilder.Entity<SinhVien>()
                .Property(e => e.MaSV)
                .IsUnicode(false);

            modelBuilder.Entity<SinhVien>()
                .Property(e => e.MatKhau)
                .IsUnicode(false);

            modelBuilder.Entity<SinhVien>()
                .Property(e => e.HinhAnh)
                .IsUnicode(false);

            modelBuilder.Entity<SinhVien>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<SinhVien>()
                .Property(e => e.Malop)
                .IsUnicode(false);
        }

        internal void DeleteObject(Diem diem)
        {
            throw new NotImplementedException();
        }
    }
}
