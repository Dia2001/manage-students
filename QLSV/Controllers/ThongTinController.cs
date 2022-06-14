using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using QLSV.Models;
namespace QLSV.Controllers
{
    public class ThongTinController : Controller
    {
        // GET: ThongTin
        private Model1 db = new Model1();
        public ActionResult Admin()
        {
            DangNhap dangNhap = db.DangNhaps.Find(Session["TenDN"]);
            if (dangNhap == null)
            {
                return HttpNotFound();
            }
            return View(dangNhap);
        }
        public ActionResult GiangVien()
        {
            if (Session["MaGV"] != null)
            {
                string mgv = Session["MaGV"].ToString();
                var list = db.GiangViens.Where(s => s.MaGV.Equals(mgv));
                return View("GiangVien", list.ToList());
            }
            else
            {
                return RedirectToAction("Index", "DangNhaps");
            }
        }
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }
        [HttpPost]
        public void DoiMatKhau( string mkc,string mkm,string mknl)
        {
            try
             {
            string tdn = Session["TenDN"].ToString();
                string mkkt = GetMD5(mkc);
                var list = db.DangNhaps.Where(s => s.TenDN.Equals(tdn) && s.MatKhau.Equals(mkkt)).ToList();
                if (list.Count > 0)
                {
                    string mkdoi = GetMD5(mkm);
                    DangNhap dangnhap = new DangNhap();
                    dangnhap = db.DangNhaps.Where(s => s.TenDN.Equals(tdn)).SingleOrDefault();
                    dangnhap.MatKhau = mkdoi;
                    db.SaveChanges();
                    Response.Write("Đổi thành công");
                }
                else
                {
                    Response.Write("Vui lòng nhập lại mật khẩu");
                }

            }
            catch(Exception e)
            {
               Response.Write("Đổi thất bại");
            }
        }
        [HttpPost]
        public void DoiMatKhauGV(string mkc, string mkm, string mknl)
        {
            try
            {
                string tdn = Session["MaGV"].ToString();
                var list = db.GiangViens.Where(s => s.MaGV.Equals(tdn) && s.MatKhau.Equals(mkc)).ToList();
                if (list.Count > 0)
                {
                    GiangVien dangnhap = new GiangVien();
                    dangnhap = db.GiangViens.Where(s => s.MaGV.Equals(tdn)).SingleOrDefault();
                    dangnhap.MatKhau = mkm;
                    db.SaveChanges();
                    Response.Write("Đổi thành công");
                }
                else
                {
                    Response.Write("Vui lòng nhập lại mật khẩu");
                }

            }
            catch (Exception e)
            {
                Response.Write("Đổi thất bại");
            }
        }
        [HttpPost]
        public void DoiMatKhauSV(string mkc, string mkm, string mknl)
        {
            try
            {
                string tdn = Session["MaSV"].ToString();
                var list = db.SinhViens.Where(s => s.MaSV.Equals(tdn) && s.MatKhau.Equals(mkc)).ToList();
                if (list.Count > 0)
                {
                    SinhVien sinhvien = new SinhVien();
                    sinhvien = db.SinhViens.Where(s => s.MaSV.Equals(tdn)).SingleOrDefault();
                    sinhvien.MatKhau = mkm;
                    db.SaveChanges();
                    Response.Write("Đổi thành công");
                }
                else
                {
                    Response.Write("Vui lòng nhập lại mật khẩu");
                }

            }
            catch (Exception e)
            {
                Response.Write("Đổi thất bại");
            }
        }
        [HttpPost]
        public ActionResult GiangVien(string TenDN, string HoTen, string Email, string HinhAnh, string SDT, string GioiTinh)
        {
            try
            {


                Boolean gt = Boolean.Parse(GioiTinh);
                if (HinhAnh == "")
                {
                    GiangVien giangvien = new GiangVien();
                    giangvien = db.GiangViens.Where(s => s.MaGV.Equals(TenDN)).SingleOrDefault();
                    giangvien.HoTenGV = HoTen;
                    giangvien.Email = Email;
                    giangvien.SDT = SDT;
                    giangvien.GioiTinh = gt;
                    db.SaveChanges();
                    ViewBag.thongbao = "Sửa thành công";
                }
                else
                {
                    GiangVien giangvien = new GiangVien();
                    giangvien = db.GiangViens.Where(s => s.MaGV.Equals(TenDN)).SingleOrDefault();
                    giangvien.HoTenGV = HoTen;
                    giangvien.Email = Email;
                    giangvien.SDT = SDT;
                    giangvien.GioiTinh = gt;
                    giangvien.HinhAnh = HinhAnh;
                    db.SaveChanges();
                    ViewBag.thongbao = "Sửa thành công";
                }
               
            }
            catch (Exception e)
            {
                ViewBag.thongbao = "Sửa không thành công";
            }
            return RedirectToAction("GiangVien", "ThongTin");
        }
        public ActionResult SinhVien()
        {
            if (Session["MaSV"] != null)
            {
                string msv = Session["MaSV"].ToString();
                var list = db.SinhViens.Where(s => s.MaSV.Equals(msv));
                return View("SinhVien", list.ToList());
            }
            else
            {
                return RedirectToAction("Index", "DangNhaps");
            }
        }
        public ActionResult suathongtin(string MaSV, string HoDem, string Ten, string Email, string GioiTinh, string NgaySinh, string MatKhau, string HinhAnh, string Lop)
        {
            DateTime dt = DateTime.Now;
            bool gt = bool.Parse(GioiTinh);
            if (NgaySinh != "")
            {
                dt = DateTime.Parse(NgaySinh);
            }

            try
            {

                if (HinhAnh == "")
                {             
                        SinhVien dangnhap = new SinhVien();
                        dangnhap = db.SinhViens.Where(s => s.MaSV.Equals(MaSV)).SingleOrDefault();
                        dangnhap.HoDemSV = HoDem;
                        dangnhap.TenSV = Ten;
                        dangnhap.Email = Email;
                        dangnhap.NgaySinh = dt;
                        dangnhap.GioiTinh = gt;
                        db.SaveChanges();
                }
                else
                {
                    
                        SinhVien dangnhap = new SinhVien();
                        dangnhap = db.SinhViens.Where(s => s.MaSV.Equals(MaSV)).SingleOrDefault();
                        dangnhap.HoDemSV = HoDem;
                        dangnhap.TenSV = Ten;
                        dangnhap.Email = Email;
                        dangnhap.NgaySinh = dt;
                        dangnhap.GioiTinh = gt;
                        db.SaveChanges();     
                }

            }
            catch
            {
                ViewBag.thongbao = "Sửa không thành công";
            }

            ViewBag.thongbao = "Sửa thành công";
            return RedirectToAction("SinhVien", "ThongTin");
        }
        [HttpPost]
        public ActionResult Admin(string TenDN, string HinhAnh, string HoTen, string SDT, string Email, string DiaChi)
        {
            try
            {

                if (HinhAnh == "")
                {
      
                        DangNhap dangnhap = new DangNhap();
                        dangnhap = db.DangNhaps.Where(s => s.TenDN.Equals(TenDN)).SingleOrDefault();
                        dangnhap.HoTen = HoTen;
                        dangnhap.SDT = SDT;
                        dangnhap.Email = Email;
                        dangnhap.DiaChi = DiaChi;
                        db.SaveChanges();
                        ViewBag.thongbao = "Sửa thành công";

                }
                else
                {
                   
                        DangNhap dangnhap = new DangNhap();
                        dangnhap = db.DangNhaps.Where(s => s.TenDN.Equals(TenDN)).SingleOrDefault();
                        dangnhap.HoTen = HoTen;
                        dangnhap.SDT = SDT;
                        dangnhap.Email = Email;
                        dangnhap.DiaChi = DiaChi;
                        dangnhap.HinhAnh = HinhAnh;
                        db.SaveChanges();
                        ViewBag.thongbao = "Sửa thành công";
                    }
                }
            catch(Exception e) {
                ViewBag.thongbao = "Sửa không thành công";
            }
            return RedirectToAction("Admin","ThongTin");
        }
    }
}