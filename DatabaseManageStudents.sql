create database quanlysinhvien
go
USE quanlysinhvien
GO
CREATE TABLE DangNhap(
	TenDN varchar(50) PRIMARY KEY NOT NULL,
	MatKhau varchar(50) NOT NULL,
	HoTen nvarchar(50) NULL,
	HinhAnh varchar(50) NULL,
	SDT varchar(50) NULL,
	Email varchar(50) NULL,
	DiaChi nvarchar(50) NULL,
)
go
CREATE TABLE Diem(
	Id int IDENTITY (1,1) not null primary key,
	MaSV varchar(50) NOT NULL,
	MaMH varchar(50) NOT NULL,
	DiemCC float NULL,
	DiemGK float NULL,
	DiemCK float NULL,
	DTB float NULL,
	DiemChu varchar(50) NULL,
	KetQua varchar(50) NULL,
	NamHoc varchar(50) NULL,
	HocKy varchar(50) NULL,
	TrangThai int DEFAULT 1,
)
go
CREATE TABLE GiangVien(
	MaGV varchar(50) PRIMARY KEY NOT NULL,
	HoTenGV nvarchar(50) NULL,
	GioiTinh bit NULL,
	Email varchar(50) NULL,
	SDT varchar(50) NULL,
	MatKhau varchar(50) NULL,
	TrinhDo nvarchar(50) NULL,
	MaKhoa varchar(50) NOT NULL,
	TrangThai int DEFAULT 1,
	HinhAnh varchar(50) NULL,
)
go
CREATE TABLE Khoa(
	MaKhoa varchar(50) PRIMARY KEY NOT NULL,
	TenKhoa nvarchar(50) NULL,
	DiaChi nvarchar(50) NULL,
	SDT varchar(50) NULL,
	TrangThai int DEFAULT 1,
)
go
CREATE TABLE Lop(
	MaLop varchar(50) PRIMARY KEY NOT NULL,
	TenLop nvarchar(50) NULL,
	MaNganh varchar(50) NOT NULL,
	SiSoLop int NULL,
	TrangThai int DEFAULT 1,
)
go
CREATE TABLE MonHoc(
	MaMH varchar(50) PRIMARY KEY NOT NULL,
	TenMH nvarchar(50) NOT NULL,
	STC int NULL,
	SoTietLT int NULL,
	SoTietTH int NULL,
	MaKhoa varchar(50) NOT NULL,
	TrangThai int DEFAULT 1,
)
go
CREATE TABLE Nganh(
	MaNganh varchar(50) primary key NOT NULL,
	TenNganh nvarchar(50) NULL,
	MaKhoa varchar(50) NOT NULL,
	TongSTC varchar(50) NULL,
	TrangThai int DEFAULT 1,
)
go
CREATE TABLE SinhVien(
	MaSV varchar(50)primary key NOT NULL,
	HoDemSV nvarchar(50) NULL,
	TenSV nvarchar(50) NULL,
	MatKhau varchar(50) NULL,
	HinhAnh varchar(50) NULL,
	NgaySinh date NULL,
	GioiTinh bit NULL,
	Email varchar(50) NULL,
	Malop varchar(50) NOT NULL,
	TrangThai int DEFAULT 1,
)
go
ALTER TABLE Diem   ADD  CONSTRAINT fk_mamh FOREIGN KEY(MaMH)REFERENCES MonHoc (MaMH)
	
GO
ALTER TABLE Diem   ADD  CONSTRAINT fk_masv FOREIGN KEY(MaSV)REFERENCES SinhVien (MaSV)
	ON UPDATE CASCADE
	ON DELETE CASCADE
GO
ALTER TABLE GiangVien   ADD  CONSTRAINT fk_makhoa1 FOREIGN KEY(MaKhoa)REFERENCES Khoa (MaKhoa)
	ON UPDATE CASCADE
	ON DELETE CASCADE
GO
ALTER TABLE Lop   ADD  CONSTRAINT fk_manganh FOREIGN KEY(MaNganh) REFERENCES Nganh (MaNganh)
	ON UPDATE CASCADE
	ON DELETE CASCADE
GO
ALTER TABLE MonHoc   ADD  CONSTRAINT fk_makhoa6 FOREIGN KEY(MaKhoa)REFERENCES Khoa (MaKhoa)
	ON UPDATE CASCADE
	ON DELETE CASCADE
GO
ALTER TABLE Nganh  ADD  CONSTRAINT fk_makhoa2 FOREIGN KEY(MaKhoa)REFERENCES Khoa (MaKhoa)
	ON UPDATE CASCADE
	ON DELETE CASCADE
GO
ALTER TABLE SinhVien  ADD  CONSTRAINT fk_malop3 FOREIGN KEY(Malop)REFERENCES Lop (MaLop)
	ON UPDATE CASCADE
	ON DELETE CASCADE
GO