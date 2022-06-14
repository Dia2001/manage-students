using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using PagedList;
using QLSV.Models;
using Rotativa.MVC;
namespace QLSV.Controllers
{
    public class SinhViensController : Controller
    {
        private Model1 db = new Model1();

        // GET: GiangViens
        public ActionResult Index()
        {
            ViewBag.MaLop = new SelectList(db.Lops, "MaLop", "TenLop");
            var list = db.SinhViens.Include(g => g.Lop);
            return View(list.ToList());
        }
        [HttpGet]
        public JsonResult Getdata2()
        {
            var lists = db.Lops.ToList();
            var a = JsonConvert.SerializeObject(lists);
            return Json(a, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult Getdata4()
        {
            var lists = db.SinhViens.ToList();
            var a = JsonConvert.SerializeObject(lists);
            return Json(a, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public void AddData(string msv, string hodem, string ten, string ns, string email, string gt, string mkh, string anh, string lop)
        {

            string masinhvien = msv;
            string hd = hodem;
            string tenn = ten;
            string em = email;
            string malop = lop;
            bool gioitinh = bool.Parse(gt);
            string matkhau = mkh;
            string anhsv = Path.GetFileName(anh);
            DateTime dat = DateTime.Now;
            if (ns != "")
            {
                dat = Convert.ToDateTime(ns);
            }
            try
            {
                if (masinhvien != "")
                {
                    var data = db.SinhViens.Where(s => s.MaSV.Equals(masinhvien)).ToList();
                    if (data.Count > 0)
                    {
                        Response.Write("Trùng mã sinh viên");
                    }
                    else
                    {
                        SinhVien sv = new SinhVien();
                        sv.MaSV = masinhvien;
                        sv.HoDemSV = hodem;
                        sv.TenSV = tenn;
                        sv.Email = em;
                        sv.NgaySinh = dat;
                        sv.GioiTinh = gioitinh;
                        sv.MatKhau = matkhau;
                        sv.Malop = malop;
                        sv.HinhAnh = anhsv;
                        db.SinhViens.Add(sv);
                        db.SaveChanges();
                        Response.Write("Thêm thành công");

                    }
                }

                else
                {
                    Response.Write("Mã sinh viên không được để trống");
                }
            }
            catch
            {
                Response.Write("Thêm thất bại");
            }

        }
        [HttpPost]
        public void EditData(string msv, string hodem, string ten, string ns, string email, string gt, string mkh, string anh, string lop)
        {


            string masinhvien = msv;
            string hd = hodem;
            string tenn = ten;
            string em = email;
            string malop = lop;
            bool gioitinh = bool.Parse(gt);
            string matkhau = mkh;
            string anhsv = Path.GetFileName(anh);
            DateTime dat = DateTime.Now;
            if (ns != "")
            {
                dat = Convert.ToDateTime(ns);
            }
            try
            {
                if (anhsv == "")
                {
                    SinhVien sv = new SinhVien();
                    sv = db.SinhViens.Where(s => s.MaSV == masinhvien).SingleOrDefault();
                    sv.HoDemSV = hodem;
                    sv.TenSV = tenn;
                    sv.Email = em;
                    sv.NgaySinh = dat;
                    sv.GioiTinh = gioitinh;
                    sv.MatKhau = matkhau;
                    sv.Malop = malop;
                    db.SaveChanges();
                    Response.Write("Sửa thành công");
                }
                else
                {
                    SinhVien sv = new SinhVien();
                    sv = db.SinhViens.Where(s => s.MaSV == masinhvien).SingleOrDefault();
                    sv.HoDemSV = hodem;
                    sv.TenSV = tenn;
                    sv.Email = em;
                    sv.NgaySinh = dat;
                    sv.GioiTinh = gioitinh;
                    sv.MatKhau = matkhau;
                    sv.Malop = malop;
                    sv.HinhAnh = anhsv;
                    db.SaveChanges();
                    Response.Write("Sửa thành công");
                }

            }
            catch (Exception e)
            {
                Response.Write("Sửa thất bại");
            }
        }
        [HttpGet]
        public JsonResult Getdata1(string id)
        {
            var list = (from l in db.SinhViens
                        from c in db.Lops
                        where l.Malop == c.MaLop && l.MaSV.Equals(id)
                        select new
                        {
                            l.MaSV,
                            l.HoDemSV,
                            l.TenSV,
                            l.Email,
                            l.GioiTinh,
                            l.Malop,
                            c.TenLop,
                            l.MatKhau,
                            l.HinhAnh,
                            l.NgaySinh
                        });
            var a = JsonConvert.SerializeObject(list);
            return Json(a, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Getdata3(string id)
        {
            var list = (from l in db.SinhViens
                        from c in db.Lops
                        where l.Malop == c.MaLop && l.MaSV.Contains(id)
                        select new
                        {
                            l.MaSV,
                            l.HoDemSV,
                            l.TenSV,
                            l.Email,
                            l.GioiTinh,
                            l.Malop,
                            c.TenLop,
                            l.MatKhau,
                            l.HinhAnh,
                            l.NgaySinh
                        });
            var a = JsonConvert.SerializeObject(list);
            return Json(a, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DelTrash(string id)
        {
            SinhVien sv = db.SinhViens.Find(id);
            sv.TrangThai = 0;
            //Cập nhật lại
            db.Entry(sv).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Trash()
        {
            int count = db.SinhViens.Count(m => m.TrangThai == 0);
            ViewBag.SoLuong = count;
            var list = db.SinhViens.Where(m => m.TrangThai == 0).ToList();
            return View("Trash", list);
        }
        public ActionResult ReTrash(string id)
        {
            SinhVien sv = db.SinhViens.Find(id);
            sv.TrangThai = 1;
            db.Entry(sv).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Trash", "SinhViens");
        }
        [HttpPost]
        public ActionResult XuatFile(string file)
        {
            if (file == "1")
            {
                var gv = new GridView();
                gv.DataSource = db.SinhViens.Where(m => m.TrangThai != 0).ToList();
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=SinhVien.xls");
                Response.ContentType = "application/ms-excel";
                //Response.Charset = "";
                StringWriter objStringWriter = new StringWriter();
                HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
                objHtmlTextWriter.WriteLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
                gv.RenderControl(objHtmlTextWriter);
                Response.Output.Write(objStringWriter.ToString());
                Response.Flush();
                Response.End();
                return View();
            }
            else
            {
                if (file == "2")
                {
                    //sử dụng code https://techfunda.com/howto/309/export-data-into-ms-word-from-mvc
                    var data = db.SinhViens.Where(m => m.TrangThai != 0).ToList();
                    GridView gridview = new GridView();
                    gridview.DataSource = data;
                    gridview.DataBind();
                    Response.ClearContent();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename = SinhVien.doc");
                    Response.ContentType = "application/ms-word";
                    Response.Charset = "";
                    using (StringWriter sw = new StringWriter())
                    {
                        using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                        {
                            htw.WriteLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
                            gridview.RenderControl(htw);
                            Response.Output.Write(sw.ToString());
                            Response.Flush();
                            Response.End();
                        }
                    }
                    return View();
                }
                else
                {
                    // sử dụng thư viện https://www.c-sharpcorner.com/article/export-pdf-from-html-in-mvc-net4/
                    var list = db.SinhViens.Where(m => m.TrangThai != 0).ToList();
                    return new PartialViewAsPdf("PrintPDF", list)
                    {
                        FileName = "SinhVien.pdf"
                    };
                }
            }
        }
        [HttpPost]
        public ActionResult Import(HttpPostedFileBase file)
        {
            string filePath = string.Empty;
            if (file != null)
            {
                string path = Server.MapPath("~/Content/Uploads/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                filePath = path + Path.GetFileName(file.FileName);
                string extension = Path.GetExtension(file.FileName);
                file.SaveAs(filePath);
                string conString = string.Empty;
                switch (extension)
                {
                    case ".xls": //Excel 97-03.
                        //Provider=Microsoft.Jet.OLEDB.4.0
                        conString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Microsoft.ACE.OLEDB.12.0" + filePath + ";Extended Properties='Excel 8.0;HDR=YES'";
                        break;
                    case ".xlsx": //Excel 07 and above.
                        conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=YES'";
                        break;
                }
                DataTable dt = new DataTable();
                conString = string.Format(conString, filePath);
                using (OleDbConnection connExcel = new OleDbConnection(conString))
                {
                    using (OleDbCommand cmdExcel = new OleDbCommand())
                    {
                        using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                        {
                            cmdExcel.Connection = connExcel;
                            connExcel.Open();
                            DataTable dtExcelSchema;
                            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                            string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                            connExcel.Close();
                            connExcel.Open();
                            cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                            odaExcel.SelectCommand = cmdExcel;
                            odaExcel.Fill(dt);
                            connExcel.Close();
                        }
                    }
                }
                conString = @"Data Source=DESKTOP-M3IMJK3\SQLEXPRESS01;Initial Catalog=quanlysinhvien;Integrated Security=True";
                using (SqlConnection con = new SqlConnection(conString))
                {
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                    {
                        sqlBulkCopy.DestinationTableName = "SinhVien";
                        sqlBulkCopy.ColumnMappings.Add("MaSV", "MaSV");
                        sqlBulkCopy.ColumnMappings.Add("HoDemSV", "HoDemSV");
                        sqlBulkCopy.ColumnMappings.Add("TenSV", "TenSV");
                        sqlBulkCopy.ColumnMappings.Add("Email", "Email");
                        sqlBulkCopy.ColumnMappings.Add("HinhAnh", "HinhAnh");
                        sqlBulkCopy.ColumnMappings.Add("MatKhau", "MatKhau");
                        sqlBulkCopy.ColumnMappings.Add("NgaySinh", "NgaySinh");
                        sqlBulkCopy.ColumnMappings.Add("GioiTinh", "GioiTinh");
                        sqlBulkCopy.ColumnMappings.Add("Malop", "Malop");
                        con.Open();
                        sqlBulkCopy.WriteToServer(dt);
                        con.Close();
                    }
                }
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult TimDrop(string MaLop)
        {
            int count = 0;
            var list = db.SinhViens.Where(s => s.TrangThai != 0).ToList();
            ViewBag.MaLop = new SelectList(db.Lops, "MaLop", "TenLop");
            if (MaLop == "")
            {
                count = db.SinhViens.Count(m => m.TrangThai != 0);
                ViewBag.SoLuong = count;
                list = db.SinhViens.Where(s => s.TrangThai != 0).ToList();
            }
            else
            {
                count = db.SinhViens.Count(m => m.Malop.Equals(MaLop) && m.TrangThai != 0);
                ViewBag.SoLuong = count;
                list = db.SinhViens.Where(m => m.Malop.Equals(MaLop) && m.TrangThai != 0).ToList();
            }
            return View("Index", list.ToPagedList(1, 5));
        }
        [HttpGet]
        public ActionResult Index(int? page)
        {
            ViewBag.MaLop = new SelectList(db.Lops, "MaLop", "TenLop");
            int count = db.SinhViens.Count(m => m.TrangThai != 0);
            ViewBag.SoLuong = count;
            var list = db.SinhViens.Where(m => m.TrangThai != 0).ToList();
            if (page == null) page = 1;
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }
        [HttpPost]
        public void Delete(string Id)
        {
            try
            {
                SinhVien sinhvien = db.SinhViens.Find(Id);
                db.SinhViens.Remove(sinhvien);
                db.SaveChanges();
                Response.Write("Xóa thành công");
            }
            catch (Exception e)
            {
                Response.Write("Xóa thất bại");
            }
        }
        // SV_INDEX
        public ActionResult sv_Index()
        {
            string masv=Session["MaSV"].ToString();
            var list = db.SinhViens.Include(g => g.Lop).Where(s => s.MaSV.Equals(masv));
            return View(list.ToList());
        }
        // GV
        public ActionResult gv_SinhVien_Index()
        {
            var list = db.SinhViens.Include(g => g.Lop);
            return View(list.ToList());
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
