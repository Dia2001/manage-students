using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLSV.Models;
namespace QLSV.Controllers
{
    public class HomesController : Controller
    {
        private Model1 db = new Model1();
        // GET: Homes
        public ActionResult Index()
        {
            var khoa = db.Khoas.Count();
            ViewBag.sokhoa = khoa;
            var nganh = db.Nganhs.Count();
            ViewBag.songanh = nganh;
            var monhoc = db.MonHocs.Count();
            ViewBag.somonhoc = monhoc;
            var giangvien = db.GiangViens.Count();
            ViewBag.sogiangvien = giangvien;
            var lop = db.Lops.Count();
            ViewBag.solop = lop;
            var sinhvien = db.SinhViens.Count();
            ViewBag.sosinhvien = sinhvien;
            return View();
        }
        public ActionResult GetKhoa()
        {
            int cntt = db.Nganhs.Where(x => x.MaKhoa == "KHOA01").Count();
            int sp = db.Nganhs.Where(x => x.MaKhoa == "KHOA02").Count();
            int toanthongke = db.Nganhs.Where(x => x.MaKhoa == "KHOA03").Count();
            int qtkd = db.Nganhs.Where(x => x.MaKhoa == "KHOA04").Count();
            int khtn = db.Nganhs.Where(x => x.MaKhoa == "KHOA05").Count();
            int khxh = db.Nganhs.Where(x => x.MaKhoa == "KHOA06").Count();
            int lyluanchinhtri = db.Nganhs.Where(x => x.MaKhoa == "KHOA07").Count();
            int gdtc = db.Nganhs.Where(x => x.MaKhoa == "KHOA08").Count();

            DemKhoa dem = new DemKhoa();
            dem.cntt = cntt;
            dem.sp = sp;
            dem.gdtc = gdtc;
            dem.khtn = khtn;
            dem.khxh = khxh;
            dem.lyluanchinhtri = lyluanchinhtri;
            dem.qtkd = qtkd;
            dem.toanthongke = toanthongke;
            return Json(dem, JsonRequestBehavior.AllowGet);
        }
        public ActionResult sv_Homes_Index()
        {
            return View();
        }
        public ActionResult gv_Homes_Index()
        {
            return View();
        }
        public ActionResult GetDiem()
        {
            int diema = db.Diems.Where(x => x.DiemChu == "A").Count();
            int diemb = db.Diems.Where(x => x.DiemChu == "B").Count();
            int diemc = db.Diems.Where(x => x.DiemChu == "C").Count();
            int diemd = db.Diems.Where(x => x.DiemChu == "D").Count();
            int dieme = db.Diems.Where(x => x.DiemChu == "E").Count();
            int diemf = db.Diems.Where(x => x.DiemChu == "F").Count();
            int diemacong = db.Diems.Where(x => x.DiemChu == "A+").Count();
            int diembcong = db.Diems.Where(x => x.DiemChu == "B+").Count();
            DemDiem dem = new DemDiem();
            dem.a = diema;
            dem.b = diemb;
            dem.c = diemc;
            dem.d = diemd;
            dem.e = dieme;
            dem.f = diemf;
            dem.acong = diemacong;
            dem.bcong = diembcong;
            return Json(dem, JsonRequestBehavior.AllowGet);
        }
        public class DemKhoa
        {
            public int cntt { get; set; }
            public int sp { get; set; }
            public int gdtc { get; set; }
            public int khtn { get; set; }
            public int khxh { get; set; }
            public int lyluanchinhtri { get; set; }
            public int qtkd { get; set; }
            public int toanthongke { get; set; }

        }
        public class DemDiem
        {
            public int a { get; set; }
            public int b { get; set; }
            public int c { get; set; }
            public int d { get; set; }
            public int e { get; set; }
            public int f { get; set; }
            public int acong { get; set; }
            public int bcong { get; set; }
        }
    }
}