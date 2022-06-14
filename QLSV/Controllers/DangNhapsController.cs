using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QLSV.Models;
using System.Security.Cryptography;
using System.Text;
namespace QLSV.Controllers
{
    public class DangNhapsController : Controller
    {
        private Model1 db = new Model1();

        // GET: DangNhaps
        public ActionResult Index()
        {
            HttpCookie c1 = Request.Cookies["username"];
            HttpCookie c2 = Request.Cookies["password"];
            HttpCookie c3 = Request.Cookies["usernamesv"];
            HttpCookie c4 = Request.Cookies["passwordsv"];
            HttpCookie c5 = Request.Cookies["usernamegv"];
            HttpCookie c6 = Request.Cookies["passwordgv"];

            if (c1 != null && c2 != null)
            {

                return RedirectToAction("Index", "Homes");
            }
            else
            {
                if (c3 != null && c4 != null)
                {
                    return RedirectToAction("sv_Homes_Index", "Homes");
                }
                else
                {
                    if (c5 != null && c6 != null)
                    {
                        return RedirectToAction("gv_Homes_Index", "Homes");
                    }
                    else
                    {
                        return View(db.DangNhaps.ToList());
                    }
                }
            }
        }
        [HttpPost]
        public ActionResult Index(string TenDN, string MatKhau,bool? checklogin)
        {
            if (ModelState.IsValid)
            {
                string tdn = TenDN.Trim();
                string mk = MatKhau.Trim();
                if (tdn == "" || mk == "")
                {
                    ViewBag.thongbao = "Vui lòng điển đầy đủ thông tin";

                }
                else
                {
                    var dtadmin = db.DangNhaps.Where(s => s.TenDN.Equals(tdn)).ToList();
                    var dtsv = db.SinhViens.Where(s => s.MaSV.Equals(tdn)).ToList();
                    var dtgv = db.GiangViens.Where(s => s.MaGV.Equals(tdn)).ToList();
                    if (dtadmin.Count > 0)
                    {
                        string mk1 = GetMD5(mk);
                        var kt = db.DangNhaps.Where(s => s.TenDN.Equals(tdn) && s.MatKhau.Equals(mk1)).ToList();
                        if (kt.Count > 0)
                        {
                            Session["TenDN"] = dtadmin.FirstOrDefault().TenDN;
                            Session["HinhAnh"] = dtadmin.FirstOrDefault().HinhAnh;
                            if (checklogin == true)
                            {
                                HttpCookie c1 = new HttpCookie("username", tdn);
                                HttpCookie c2 = new HttpCookie("password", mk1);
                                DateTime d = DateTime.Now;
                                TimeSpan ts = new TimeSpan(0, 0, 5, 0, 0);
                                c1.Expires = d.Add(ts);
                                c2.Expires = d.Add(ts);
                                Response.Cookies.Add(c1);
                                Response.Cookies.Add(c2);
           
                                return RedirectToAction("Index", "Homes");
                            }
                            else
                            {


                      
                                return RedirectToAction("Index", "Homes");
                            }
                        }
                        else
                        {
                            ViewBag.thongbao = "Sai tên đăng nhập hoặc mật khẩu";
                        }

                    }
                    else
                    {

                        if (dtsv.Count > 0)
                        {
                            var kt = db.SinhViens.Where(s => s.MaSV.Equals(tdn) && s.MatKhau.Equals(mk)).ToList();
                            if (kt.Count > 0)
                            {
                                Session["MaSV"] = dtsv.FirstOrDefault().MaSV;
                                Session["HinhAnh"] = dtsv.FirstOrDefault().HinhAnh;
                                if (checklogin == true)
                                {
                                    HttpCookie c3 = new HttpCookie("usernamesv", tdn);
                                    HttpCookie c4 = new HttpCookie("passwordsv", mk);
                                    DateTime d = DateTime.Now;
                                    TimeSpan ts = new TimeSpan(0, 0, 5, 0, 0);
                                    c3.Expires = d.Add(ts);
                                    c4.Expires = d.Add(ts);
                                    Response.Cookies.Add(c3);
                                    Response.Cookies.Add(c4);
                                    
                                    return RedirectToAction("sv_Homes_Index", "Homes");
                                }
                                else
                                {
                                    return RedirectToAction("sv_Homes_Index", "Homes");
                                }
                            }
                            else
                            {
                                ViewBag.thongbao = "Sai tên đăng nhập hoặc mật khẩu";
                            }
                        }
                        else
                        {
                            var kt = db.GiangViens.Where(s => s.MaGV.Equals(tdn) && s.MatKhau.Equals(mk)).ToList();
                            if (kt.Count > 0)
                            {
                                Session["MaGV"] = dtgv.FirstOrDefault().MaGV;
                                Session["HinhAnh"] = dtgv.FirstOrDefault().HinhAnh;
                                if (checklogin == true)
                                {
                                    HttpCookie c5 = new HttpCookie("usernamegv", tdn);
                                    HttpCookie c6 = new HttpCookie("passwordgv", mk);
                                    // ngày và giờ hiện tại trên máy tính
                                    DateTime d = DateTime.Now;
                                   // public TimeSpan(int days, int hours, int minutes, int seconds, int milliseconds)
                                    TimeSpan ts = new TimeSpan(0, 0, 5, 0, 0);
                                    //Expires: hết hạn
                                    c5.Expires = d.Add(ts);
                                    c6.Expires = d.Add(ts);
                                    Response.Cookies.Add(c5);
                                    Response.Cookies.Add(c6);
                                    
                                    return RedirectToAction("gv_Homes_Index", "Homes");
                                }
                                else
                                {
                                    return RedirectToAction("gv_Homes_Index", "Homes");
                                }
                            }
                            else
                            {
                                ViewBag.thongbao = "Sai tên đăng nhập hoặc mật khẩu";
                            }
                        }
                    }
                }

            }
            return View();
        }
        public ActionResult Thoat()
        {
            Session.Abandon();
            Response.Cookies["username"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["password"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["usernamesv"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["passwordsv"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["usernamegv"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["passwordgv"].Expires = DateTime.Now.AddDays(-1);
            return RedirectToAction("Index", "DangNhaps");
        }

        // GET: DangNhaps/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DangNhap dangNhap = db.DangNhaps.Find(id);
            if (dangNhap == null)
            {
                return HttpNotFound();
            }
            return View(dangNhap);
        }

        // GET: DangNhaps/Create
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult DangXuat()
        {
            Session.Abandon();
            Response.Cookies["username"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["password"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["usernamesv"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["passwordsv"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["usernamegv"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["passwordgv"].Expires = DateTime.Now.AddDays(-1);
            return View("Index","DangNhaps");
        }
        [HttpPost]

        public ActionResult Create(string TenDN, string MatKhau, string MatKhauMoi, string HinhAnh,string HoTen,string SDT,string Email,string DiaChi)
        {
            if (ModelState.IsValid)
            {
                string tdn = TenDN.Trim();
                string mk = MatKhau.Trim();
                string mkm = MatKhauMoi.Trim();
                string ha = HinhAnh;
                string ht = HoTen.Trim();
                string sdt = SDT.Trim();
                string email =Email.Trim();
                string diachi = DiaChi.Trim();
                var data = db.DangNhaps.Where(s => s.TenDN.Equals(tdn)).ToList();
                if (tdn == "" || mk == "" || mkm == "")
                {
                    ViewBag.thongbao = "Vui lòng điền đầy đủ thông tin";
                }
                else
                {
                    if (data.Count > 0)
                    {
                        ViewBag.thongbao = "Tên đăng nhập đã tồn tại";
                    }
                    else
                    {
                        if (mk != mkm)
                        {
                            ViewBag.thongbao = "Mật khẩu không khớp";
                        }
                        else
                        {
                            string mkdk = GetMD5(mkm);
                            DangNhap dn = new DangNhap();
                            dn.TenDN = tdn;
                            dn.MatKhau = mkdk;
                            dn.HoTen = ht;
                            dn.HinhAnh = ha;
                            dn.SDT = sdt;
                            dn.Email = email;
                            dn.DiaChi = diachi;
                            db.DangNhaps.Add(dn);
                            db.SaveChanges();
                            return RedirectToAction("Index");
                        }
                    }
                }
            }
            return View();
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
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
