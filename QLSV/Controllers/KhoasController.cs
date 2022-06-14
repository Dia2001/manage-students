using QLSV.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Data.SqlClient;
using PagedList;
using Rotativa;
using Newtonsoft.Json;
using Microsoft.Ajax.Utilities;
using Rotativa.MVC;

namespace QLSV.Controllers
{
    public class KhoasController : Controller
    {
        private Model1 db = new Model1();

        // GET: Khoas
        [HttpGet]
        public JsonResult Getdata(string id)
        {
            var list = db.Khoas.Where(p => p.MaKhoa.Equals(id));
            var a = JsonConvert.SerializeObject(list);
            return Json(a, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Getdata1(string id)
        {
            var list = db.Khoas.Where(p => p.TenKhoa.Contains(id));
            var a = JsonConvert.SerializeObject(list);
            return Json(a, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public void AddData(string mk, string tk, string sodienthoai, string dc)
        {

            string makhoa = mk;
            string tenkhoa = tk;
            string diachi = dc;
            string sdt = sodienthoai;

            if (makhoa.ToString() != "")
            {
                var data = db.Khoas.Where(s => s.MaKhoa.Equals(makhoa)).ToList();
                if (data.Count > 0)
                {
                    Response.Write("trùng mã khoa");
                }
                else
                {
                    Khoa dn = new Khoa();
                    dn.MaKhoa = makhoa;
                    dn.TenKhoa = tenkhoa;
                    dn.SDT = sdt;
                    dn.DiaChi = diachi;
                    db.Khoas.Add(dn);
                    db.SaveChanges();
                    Response.Write("thêm thành công");

                }
            }

            else
            {
                Response.Write("Mã khoa không được để trống");
            }

        }
        [HttpPost]
        public void EditData(string mk, string tk, string sodienthoai, string dc)
        {

            string makhoa = mk;
            string tenkhoa = tk;
            string diachi = dc;
            string sdt = sodienthoai;
            try
            {
                Khoa kh = new Khoa();
                kh = db.Khoas.Where(s => s.MaKhoa ==makhoa).SingleOrDefault();
                kh.TenKhoa = tenkhoa;
                kh.SDT = sdt;
                kh.DiaChi = diachi;
                db.SaveChanges();
                Response.Write("Sửa thành công");

            }
            catch(Exception e)
            {

            }
        }
        [HttpPost]
        public ActionResult XuatFile(string file)
       {
            if (file == "1")
            {
                var gv = new GridView();
                gv.DataSource = db.Khoas.Where(m => m.TrangThai != 0).ToList();
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=Khoa.xls");
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
                if(file == "2")
                {
                    //sử dụng code https://techfunda.com/howto/309/export-data-into-ms-word-from-mvc
                    var data = db.Khoas.Where(m => m.TrangThai != 0).ToList(); ;
                    GridView gridview = new GridView();
                    gridview.DataSource = data;
                    gridview.DataBind();
                    Response.ClearContent();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename = Khoa.doc");
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
                    var list = db.Khoas.Where(m => m.TrangThai != 0).ToList();
                    return new PartialViewAsPdf("PrintPDF", list)
                    {
                        FileName = "Khoa.pdf"
                    };
                }
            }
        }
        public ActionResult Index(int? page)
        {
            if (page == null) page = 1;
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            int count = db.Khoas.Count(m => m.TrangThai != 0);
            ViewBag.SoLuong = count;
            var list = db.Khoas.Where(m => m.TrangThai != 0).ToList();
            return View(list.ToPagedList(pageNumber, pageSize));
        }
        [HttpPost]
        public ActionResult Search(FormCollection f,int? page)
        {
            string b = (f["tenkhoa"]).Trim();
            var list = db.Khoas.Where(p => p.TenKhoa.Contains(b)).ToList();
            int count = list.Count(m => m.TrangThai != 0);
            ViewBag.SoLuong = count;
            return View("Index", list.ToPagedList(1, 3));
        }
        public ActionResult PrintPDF()
        {
            // sử dụng thư viện https://www.c-sharpcorner.com/article/export-pdf-from-html-in-mvc-net4/
            var list = db.Khoas.Where(m => m.TrangThai != 0).ToList();
            return new PartialViewAsPdf("PrintPDF", list)
            {
                FileName = "Khoa.pdf"
            };
        }
        public void ExportToWord()
        {
            //sử dụng code https://techfunda.com/howto/309/export-data-into-ms-word-from-mvc
            var data = db.Khoas.ToList();
            GridView gridview = new GridView();
            gridview.DataSource = data;
            gridview.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename = khoa.doc");
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
        }
        public ActionResult DelTrash(string id)
        {
            Khoa khoa = db.Khoas.Find(id);
            khoa.TrangThai = 0;
            //Cập nhật lại
            db.Entry(khoa).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Trash()
        {
            int count = db.Khoas.Count(m => m.TrangThai == 0);
            ViewBag.SoLuong = count;
            var list = db.Khoas.Where(m => m.TrangThai == 0).ToList();
            return View("Trash", list);
        }
        public ActionResult ReTrash(string id)
        {
            Khoa khoa = db.Khoas.Find(id);
            khoa.TrangThai = 1;
            db.Entry(khoa).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Trash", "Khoas");
        }
        public void Export()
        {
            var gv = new GridView();
            gv.DataSource =db.Khoas.Where(m => m.TrangThai != 0).ToList();
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=khoa.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
            objHtmlTextWriter.WriteLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
            gv.RenderControl(objHtmlTextWriter);
            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();
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
                conString =@"Data Source=DESKTOP-M3IMJK3\SQLEXPRESS01;Initial Catalog=quanlysinhvien;Integrated Security=True";
                using (SqlConnection con = new SqlConnection(conString))
                {
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                    {
                        //Set the database table name.
                        sqlBulkCopy.DestinationTableName = "Khoa";

                        // Map the Excel columns with that of the database table, this is optional but good if you do
                        // 
                        sqlBulkCopy.ColumnMappings.Add("MaKhoa", "MaKhoa");
                        sqlBulkCopy.ColumnMappings.Add("TenKhoa", "TenKhoa");
                        sqlBulkCopy.ColumnMappings.Add("DiaChi", "DiaChi");
                        sqlBulkCopy.ColumnMappings.Add("SDT", "SDT");
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
        public void Delete(string Id)
        {
            try
            {
                Khoa khoa = db.Khoas.Find(Id);
                db.Khoas.Remove(khoa);
                db.SaveChanges();
                Response.Write("Xóa thành công");
            }
            catch (Exception e)
            {
                Response.Write("Xóa thất bại");
            }
        }
        //SV
        public ActionResult sv_Khoa_Index()
        {
            var khoa = db.Khoas;
            return View(khoa.ToList());
        }
        //GV
        public ActionResult gv_Khoa_Index()
        {
            var khoa = db.Khoas;
            return View(khoa.ToList());
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
