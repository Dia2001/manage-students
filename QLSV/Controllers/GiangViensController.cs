using Newtonsoft.Json;
using PagedList;
using QLSV.Models;
using Rotativa.MVC;
using System;
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

namespace QLSV.Controllers
{
    public class GiangViensController : Controller
    {
        private Model1 db = new Model1();

        // GET: GiangViens
        public ActionResult Index()
        {
            ViewBag.MaKhoa = new SelectList(db.Khoas, "MaKhoa", "TenKhoa");
            var giangViens = db.GiangViens.Include(g => g.Khoa);
            return View(giangViens.ToList());
        }
        [HttpGet]
        public JsonResult Getdata2()
        {
            var lists = db.Khoas.ToList();
           var a = JsonConvert.SerializeObject(lists);
            return Json(a, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public void AddData(string mgv, string ht, string mk, string em, string sdt, string gt, string mkh, string td, string anh)
        {

            string magiangvien = mgv;
            string hoten = ht;
            string makhoa = mk;
            string email = em;
            string sdthoai = sdt;
            bool gioitinh =bool.Parse(gt);
            string matkhau = mkh;
            string trinhdo = td;
            string hinhanh = Path.GetFileName(anh);
            try
            {
                if (magiangvien != "")
                {
                    var data = db.GiangViens.Where(s => s.MaGV.Equals(magiangvien)).ToList();
                    if (data.Count > 0)
                    {
                        Response.Write("Trùng mã giảng viên");
                    }
                    else
                    {
                        GiangVien gv = new GiangVien();
                        gv.MaGV = magiangvien;
                        gv.HoTenGV = hoten;
                        gv.MaKhoa = makhoa;
                        gv.Email = email;
                        gv.SDT = sdthoai;
                        gv.GioiTinh = gioitinh;
                        gv.MatKhau = matkhau;
                        gv.TrinhDo = trinhdo;
                        gv.HinhAnh = hinhanh;
                        db.GiangViens.Add(gv);
                        db.SaveChanges();
                        Response.Write("Thêm thành công");

                    }
                }

                else
                {
                    Response.Write("Mã giảng viên không được để trống");
                }
            }
            catch
            {
                Response.Write("Thêm thất bại");
            }

        }
        [HttpPost]
        public void EditData(string mgv, string ht, string mk, string em, string sdt, string gt, string mkh, string td, string anh)
        {

            string magiangvien = mgv;
            string hoten = ht;
            string makhoa = mk;
            string email = em;
            string sdthoai = sdt;
            bool gioitinh = bool.Parse(gt);
            string matkhau = mkh;
            string trinhdo = td;
            string hinhanh = Path.GetFileName(anh); ;
            try
            {
                if (hinhanh == "")
                {
                    GiangVien gv = new GiangVien();
                    gv = db.GiangViens.Where(s => s.MaGV == magiangvien).SingleOrDefault();
                    gv.HoTenGV = hoten;
                    gv.MaKhoa = makhoa;
                    gv.Email = email;
                    gv.SDT = sdthoai;
                    gv.GioiTinh = gioitinh;
                    gv.MatKhau = matkhau;
                    gv.TrinhDo = trinhdo;
                    db.SaveChanges();
                    Response.Write("Sửa thành công");
                }
                else
                {
                    GiangVien gv = new GiangVien();
                    gv = db.GiangViens.Where(s => s.MaGV == magiangvien).SingleOrDefault();
                    gv.HoTenGV = hoten;
                    gv.MaKhoa = makhoa;
                    gv.Email = email;
                    gv.SDT = sdthoai;
                    gv.GioiTinh = gioitinh;
                    gv.MatKhau = matkhau;
                    gv.TrinhDo = trinhdo;
                    gv.HinhAnh = hinhanh;
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
            var list = (from l in db.GiangViens
                        from c in db.Khoas
                        where l.MaKhoa == c.MaKhoa && l.MaGV.Equals(id)
                        select new
                        {
                            l.MaGV,
                            l.HoTenGV,
                            l.MaKhoa,
                            c.TenKhoa,
                            l.Email,
                            l.SDT,
                            l.GioiTinh,
                            l.MatKhau,
                            l.TrinhDo,
                            l.HinhAnh,
                        });
            var a = JsonConvert.SerializeObject(list);
            return Json(a, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Getdata3(string id)
        {
            var list = (from l in db.GiangViens
                        from c in db.Khoas
                        where l.MaKhoa == c.MaKhoa && l.HoTenGV.Contains(id)
                        select new
                        {
                            l.MaGV,
                            l.HoTenGV,
                            l.MaKhoa,
                            c.TenKhoa,
                            l.Email,
                            l.SDT,
                            l.GioiTinh,
                            l.MatKhau,
                            l.TrinhDo,
                            l.HinhAnh,
                        });
            var a = JsonConvert.SerializeObject(list);
            return Json(a, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DelTrash(string id)
        {
            GiangVien giangvien = db.GiangViens.Find(id);
            giangvien.TrangThai = 0;
            //Cập nhật lại
            db.Entry(giangvien).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Trash()
        {
            int count = db.GiangViens.Count(m => m.TrangThai == 0);
            ViewBag.SoLuong = count;
            var list = db.GiangViens.Where(m => m.TrangThai == 0).ToList();
            return View("Trash", list);
        }
        public ActionResult ReTrash(string id)
        {
            GiangVien giangvien = db.GiangViens.Find(id);
            giangvien.TrangThai = 1;
            db.Entry(giangvien).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Trash", "GiangViens");
        }
        [HttpPost]
        public ActionResult XuatFile(string file)
        {
            if (file == "1")
            {
                var gv = new GridView();
                gv.DataSource = db.GiangViens.Where(m => m.TrangThai != 0).ToList();
                gv.DataBind();

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=GiangVien.xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
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
                    // get the data from database
                    var data = db.GiangViens.Where(m => m.TrangThai != 0).ToList();
                    // instantiate the GridView control from System.Web.UI.WebControls namespace
                    // set the data source
                    GridView gridview = new GridView();
                    gridview.DataSource = data;
                    gridview.DataBind();

                    // Clear all the content from the current response
                    Response.ClearContent();
                    Response.Buffer = true;
                    // set the header
                    Response.AddHeader("content-disposition", "attachment;filename = GiangVien.doc");
                    Response.ContentType = "application/ms-word";
                    Response.Charset = "";
                    // create HtmlTextWriter object with StringWriter
                    using (StringWriter sw = new StringWriter())
                    {
                        using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                        {
                            htw.WriteLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
                            // render the GridView to the HtmlTextWriter
                            gridview.RenderControl(htw);
                            // Output the GridView content saved into StringWriter
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
                    var list = db.GiangViens.Where(m => m.TrangThai != 0).ToList();
                    return new PartialViewAsPdf("PrintPDF", list)
                    {
                        FileName = "GiangVien.pdf"
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
                        sqlBulkCopy.DestinationTableName = "GiangVien";
                        sqlBulkCopy.ColumnMappings.Add("MaGV", "MaGV");
                        sqlBulkCopy.ColumnMappings.Add("HoTenGV", "HoTenGV");
                        sqlBulkCopy.ColumnMappings.Add("GioiTinh", "GioiTinh");
                        sqlBulkCopy.ColumnMappings.Add("Email", "Email");
                        sqlBulkCopy.ColumnMappings.Add("SDT", "SDT");
                        sqlBulkCopy.ColumnMappings.Add("MatKhau", "MatKhau");
                        sqlBulkCopy.ColumnMappings.Add("TrinhDo", "TrinhDo");
                        sqlBulkCopy.ColumnMappings.Add("MaKhoa", "MaKhoa");
                        sqlBulkCopy.ColumnMappings.Add("HinhAnh", "HinhAnh");
                        con.Open();
                        sqlBulkCopy.WriteToServer(dt);
                        con.Close();
                    }
                }
            }
            ViewBag.Success = "File Imported and excel data saved into database";
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult TimDrop(string Makhoa)
        {
            int count = 0;
            var list = db.GiangViens.Where(s => s.TrangThai != 0).ToList();
            ViewBag.MaKhoa = new SelectList(db.Khoas, "MaKhoa", "TenKhoa");
            if (Makhoa == "")
            {
                count = db.GiangViens.Count(m => m.TrangThai != 0);
                ViewBag.SoLuong = count;
                list = db.GiangViens.Where(s => s.TrangThai != 0).ToList();
            }
            else
            {
                count = db.GiangViens.Count(m => m.MaKhoa.Equals(Makhoa) && m.TrangThai != 0);
                ViewBag.SoLuong = count;
                list = db.GiangViens.Where(m => m.MaKhoa.Equals(Makhoa) && m.TrangThai != 0).ToList();
            }
            return View("Index", list.ToPagedList(1, 5));
        }
        [HttpGet]
        public ActionResult Index(int? page)
        {
            ViewBag.MaKhoa = new SelectList(db.Khoas, "MaKhoa", "TenKhoa");
            int count = db.GiangViens.Count(m => m.TrangThai != 0);
            ViewBag.SoLuong = count;
            var list = db.GiangViens.Where(m => m.TrangThai != 0).ToList();
            if (page == null) page = 1;
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            //Drop tham khảo https://sethphat.com/sp-372/razor-view-asp-net-mvc-dropdownlist
            return View(list.ToPagedList(pageNumber, pageSize));
        }
        [HttpPost]
        public void Delete(string Id)
        {
            try
            {
                GiangVien giangVien = db.GiangViens.Find(Id);
                db.GiangViens.Remove(giangVien);
                db.SaveChanges();
                Response.Write("Xóa thành công");
            }
            catch (Exception e)
            {
                Response.Write("Xóa thất bại");
            }
        }
        // SV
        public ActionResult sv_GiangVien_Index(){
            var giangViens = db.GiangViens.Include(g => g.Khoa);
            return View(giangViens.ToList());
        }
        //GV
        public ActionResult gv_Index()
        {
            string magv = Session["MaGV"].ToString();
            var giangViens = db.GiangViens.Include(g => g.Khoa).Where(s=>s.MaGV.Equals(magv));
            return View(giangViens.ToList());
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
