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
using Rotativa;
using Rotativa.MVC;

namespace projectCK.Controllers
{
    public class MonHocsController : Controller
    {
        private Model1 db = new Model1();

        // GET: MonHocs
        //public ActionResult Index()
        //{

        //    var monHocs = db.MonHocs.Include(m => m.Khoa);
        //    return View(monHocs.ToList());
        //}
        [HttpGet]
        public ActionResult Index(int? page)
        {


            ViewBag.MaKhoa = new SelectList(db.Khoas, "MaKhoa", "TenKhoa");
            int count = db.MonHocs.Count(m => m.TrangThai != 0);
            ViewBag.SoLuong = count;
            var list = db.MonHocs.Where(m => m.TrangThai != 0).ToList();
            if (page == null) page = 1;
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            //Drop tham khảo https://sethphat.com/sp-372/razor-view-asp-net-mvc-dropdownlist
            return View(list.ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public JsonResult GetKhoa()
        {
            var list = db.Khoas.ToList();
            var a = JsonConvert.SerializeObject(list);
            return Json(a, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Getdata3(string id)
        {

            var list = (from l in db.MonHocs
                        from c in db.Khoas
                        where l.MaKhoa == c.MaKhoa && l.TenMH.Contains(id)
                        select new
                        {
                            l.MaMH,
                            l.TenMH,
                            l.MaKhoa,
                            c.TenKhoa,
                            l.SoTietLT,
                            l.SoTietTH,
                            l.STC,
                        });
            var a = JsonConvert.SerializeObject(list);
            return Json(a, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public void AddData(string mm, string tm, string stc, string stlt, string stth, string makhoa)
        {

            string mamon = mm;
            string tenmon = tm;
            string mkhoa = makhoa;
            int STLT = 0;
            int STTH = 0;
            int STC = 0;
            if (stlt != "")
            {
                STLT = Convert.ToInt32(stlt);
            }
            if (stth != "")
            {
                STTH = Convert.ToInt32(stth);
            }
            if (stc != "")
            {
                STC = Convert.ToInt32(stc);
            }
            try
            {
                var data = db.MonHocs.Where(s => s.MaMH.Equals(mamon)).ToList();
                if (data.Count > 0)
                {
                    Response.Write("Trùng mã môn học");
                }
                else
                {
                    MonHoc mh = new MonHoc();
                    mh.MaMH = mamon;
                    mh.TenMH = tenmon;
                    mh.STC = STC;
                    mh.SoTietLT = STLT;
                    mh.SoTietTH = STTH;
                    mh.MaKhoa = mkhoa;
                    db.MonHocs.Add(mh);
                    db.SaveChanges();
                    Response.Write("Thêm thành công");
                }

            }
            catch(Exception e)
            {
                Response.Write("Thêm thất bại");
            }
        }
        [HttpGet]
        public JsonResult Getdata1(string id)
        {
            var list = (from l in db.MonHocs
                        from c in db.Khoas
                        where l.MaKhoa == c.MaKhoa && l.MaMH.Equals(id)
                        select new
                        {
                            l.MaMH,
                            l.TenMH,
                            l.SoTietLT,
                            l.MaKhoa,
                            c.TenKhoa,
                            l.SoTietTH,
                            l.STC
                        });
            var a = JsonConvert.SerializeObject(list);
            return Json(a, JsonRequestBehavior.AllowGet);
        }
        public void EditData(string mm, string tm, string stc, string stlt, string stth, string makhoa)
        {

            string mamon = mm;
            string tenmon = tm;
            string mkhoa = makhoa;
            int STLT = 0;
            int STTH = 0;
            int STC = 0;
            if (stlt != "")
            {
                STLT = Convert.ToInt32(stlt);
            }
            if (stth != "")
            {
                STTH = Convert.ToInt32(stth);
            }
            if (stc != "")
            {
                STC = Convert.ToInt32(stc);
            }
            try
            {
                MonHoc mh = new MonHoc();
                mh = db.MonHocs.Where(p => p.MaMH.Equals(mamon)).SingleOrDefault();
                mh.TenMH = tenmon;
                mh.STC = STC;
                mh.SoTietLT = STLT;
                mh.SoTietTH = STTH;
                mh.MaKhoa = mkhoa;
                db.SaveChanges();
                Response.Write("Sửa thành công");
            }
            catch
            {
                Response.Write("Sửa thất bại");
            }
        }
        public ActionResult DelTrash(string id)
        {
            MonHoc mh = db.MonHocs.Find(id);
            mh.TrangThai = 0;
            //Cập nhật lại
            db.Entry(mh).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Trash()
        {
            int count = db.MonHocs.Count(m => m.TrangThai == 0);
            ViewBag.SoLuong = count;
            var list = db.MonHocs.Where(m => m.TrangThai == 0).ToList();
            return View("Trash", list);
        }
        public ActionResult ReTrash(string id)
        {
            MonHoc mh = db.MonHocs.Find(id);
            mh.TrangThai = 1;
            db.Entry(mh).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Trash", "MonHocs");
        }
        [HttpPost]
        public ActionResult XuatFile(string file)
        {
            if (file == "1")
            {
                var gv = new GridView();
                gv.DataSource = db.MonHocs.Where(m => m.TrangThai != 0).ToList();
                gv.DataBind();

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=MonHoc.xls");
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
                    var data = db.MonHocs.Where(m => m.TrangThai != 0).ToList();
                    // instantiate the GridView control from System.Web.UI.WebControls namespace
                    // set the data source
                    GridView gridview = new GridView();
                    gridview.DataSource = data;
                    gridview.DataBind();

                    // Clear all the content from the current response
                    Response.ClearContent();
                    Response.Buffer = true;
                    // set the header
                    Response.AddHeader("content-disposition", "attachment;filename = monhoc.doc");
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
                    var list = db.MonHocs.Where(m => m.TrangThai != 0).ToList();
                    return new PartialViewAsPdf("PrintPDF", list)
                    {
                        FileName = "MonHoc.pdf"
                    };
                }
            }
        }
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

                            //Get the name of First Sheet.
                            connExcel.Open();
                            DataTable dtExcelSchema;
                            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                            string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                            connExcel.Close();

                            //Read Data from First Sheet.
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
                        //Set the database table name.
                        sqlBulkCopy.DestinationTableName = "MonHoc";

                        // Map the Excel columns with that of the database table, this is optional but good if you do
                        // 
                        sqlBulkCopy.ColumnMappings.Add("MaMH", "MaMH");
                        sqlBulkCopy.ColumnMappings.Add("TenMH", "TenMH");
                        sqlBulkCopy.ColumnMappings.Add("STC", "STC");
                        sqlBulkCopy.ColumnMappings.Add("SoTietLT", "SoTietLT");
                        sqlBulkCopy.ColumnMappings.Add("SoTietTH", "SoTietTH");
                        sqlBulkCopy.ColumnMappings.Add("MaKhoa", "MaKhoa");
                        con.Open();
                        sqlBulkCopy.WriteToServer(dt);
                        con.Close();
                    }
                }
            }
            //if the code reach here means everthing goes fine and excel data is imported into database
            ViewBag.Success = "File Imported and excel data saved into database";

            // return View(db.Khoas.ToList());
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult TimDrop(string MaKhoa)
        {
            int count = 0;
            var list = db.MonHocs.Where(s => s.TrangThai != 0).ToList();
            ViewBag.MaKhoa = new SelectList(db.Khoas, "MaKhoa", "TenKhoa");
            if (MaKhoa == "")
            {
                count = db.MonHocs.Count(m => m.TrangThai != 0);
                ViewBag.SoLuong = count;
                list = db.MonHocs.Where(s => s.TrangThai != 0).ToList();
            }
            else
            {
                count = db.MonHocs.Count(m => m.MaKhoa.Equals(MaKhoa) && m.TrangThai != 0);
                ViewBag.SoLuong = count;
                list = db.MonHocs.Where(m => m.MaKhoa.Equals(MaKhoa) && m.TrangThai != 0).ToList();
            }
            return View("Index", list.ToPagedList(1, 5));
        }
        [HttpPost]
        public void Delete(string Id)
        {
            try
            {
                MonHoc mon = db.MonHocs.Find(Id);
                var diem = db.Diems.Where(s => s.MaMH ==Id).FirstOrDefault();
                db.MonHocs.Remove(mon);
                db.Diems.Remove(diem);
                db.SaveChanges();
                Response.Write("Xóa thành công");
            }
            catch (Exception e)
            {
                Response.Write("Xóa thất bại");
            }
        }
        // SV
        public ActionResult sv_MonHoc_Index()
        {
            var monHocs = db.MonHocs.Include(m => m.Khoa);
            return View(monHocs.ToList());
        }
        //GV
        public ActionResult gv_MonHoc_Index()
        {
            var monHocs = db.MonHocs.Include(m => m.Khoa);
            return View(monHocs.ToList());
        }

    }
}