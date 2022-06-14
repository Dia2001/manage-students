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
    public class DiemsController : Controller
    {
        private Model1 db = new Model1();

        // GET: Diems
        public ActionResult Index()
        {
            ViewBag.MaMH = new SelectList(db.MonHocs, "MaMH", "TenMH");
            var diems = db.Diems.Include(d => d.MonHoc).Include(d => d.SinhVien);
            return View(diems.ToList());
        }
        [HttpGet]
        public JsonResult Getdata2()
        {
            var lists = db.MonHocs.ToList();
            var a = JsonConvert.SerializeObject(lists);
            return Json(a, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public void AddData(string msv, string mh, string nh, string hk, string cc, string gk, string ck)
        {

            string masinhvien = msv;
            string monhoc = mh;
            string namhoc = nh;
            string hocky =hk;
            float chuyencan = float.Parse(cc);
            float giuaky = float.Parse(gk);
            float cuoiky =float.Parse(ck);
            double dtb = 0;
            double dtb1 = 0;
            string diemchu = "";
            string ketqua = "";
            dtb1 = (chuyencan *(float)(0.1))+(giuaky * (float)(0.2))+(cuoiky * (float)(0.7));
            dtb=Math.Round(dtb1,2);
            if (dtb >= 9){
			    diemchu= "A+";
                ketqua ="dat.jpg";
            }else
            {
                if (dtb >= 8){
				    diemchu = "A";
                    ketqua = "dat.jpg";
                }else
                {
                    if (dtb >= 7){
					    diemchu = "B+";
                        ketqua = "dat.jpg";
                    }else
                    {
                        if (dtb >= 6){
						    diemchu = "B";
                            ketqua = "dat.jpg";
                        }else
                        {
                            if (dtb >= 5){
							    diemchu = "C";
                                ketqua ="dat.jpg";
                            }else
                            {
                                if (dtb >= 4){
								    diemchu = "D";
                                    ketqua = "dat.jpg";
                                }else
                                {
								    diemchu = "F";
                                    ketqua = "khongdat.png";
                                }
                            }
                        }
                    }
                }
            }
            try
            {
                if (masinhvien != ""&&monhoc!=""&&namhoc!=""&&hocky!="")
                {
                    var data = db.Diems.Where(s => s.MaSV.Equals(masinhvien)&&s.MaMH.Equals(monhoc)&&s.NamHoc.Equals(namhoc)&&s.HocKy.Equals(hocky)).ToList();
                    if (data.Count > 0)
                    {
                        Response.Write("Môn học của sinh viên đã có điểm");
                    }
                    else
                    {
                        Diem diem = new Diem();
                        diem.MaSV = masinhvien;
                        diem.MaMH=monhoc;
                        diem.NamHoc = namhoc;
                        diem.HocKy = hocky;
                        diem.DiemCC = chuyencan;
                        diem.DiemGK = giuaky;
                        diem.DiemCK = cuoiky;
                        diem.DTB = dtb;
                        diem.DiemChu = diemchu;
                        diem.KetQua = ketqua;
                        db.Diems.Add(diem);
                        db.SaveChanges();
                        Response.Write("Thêm thành công");

                    }
                }

                else
                {
                    Response.Write("Mã sinh viên, mã môn học, năm học, học kỳ không được để trống");
                }
            }
            catch
            {
                Response.Write("Thêm thất bại");
            }

        }
        [HttpPost]
        public void EditData(string id,string msv, string mh, string nh, string hk, string cc, string gk, string ck)
        {
            int Id = Convert.ToInt32(id);
            string masinhvien = msv;
            string monhoc = mh;
            string namhoc = nh;
            string hocky =hk;
            float chuyencan = float.Parse(cc);
            float giuaky = float.Parse(gk);
            float cuoiky =float.Parse(ck);
            double dtb = 0;
            double dtb1 = 0;
            string diemchu = "";
            string ketqua = "";
            dtb1 = (chuyencan * (float)(0.1)) + (giuaky * (float)(0.2)) + (cuoiky * (float)(0.7));
            dtb = Math.Round(dtb1, 2);
            if (dtb >= 9){
			    diemchu= "A+";
                ketqua ="dat.jpg";
            }else
            {
                if (dtb >= 8){
				    diemchu = "A";
                    ketqua = "dat.jpg";
                }else
                {
                    if (dtb >= 7){
					    diemchu = "B+";
                        ketqua = "dat.jpg";
                    }else
                    {
                        if (dtb >= 6){
						    diemchu = "B";
                            ketqua = "dat.jpg";
                        }else
                        {
                            if (dtb >= 5){
							    diemchu = "C";
                                ketqua ="dat.jpg";
                            }else
                            {
                                if (dtb >= 4){
								    diemchu = "D";
                                    ketqua = "dat.jpg";
                                }else
                                {
								    diemchu = "F";
                                    ketqua = "khongdat.png";
                                }
                            }
                        }
                    }
                }
            }
            try
            {
                Diem diem = new Diem();
                diem= db.Diems.Where(s => s.Id==Id&&s.MaSV.Equals(masinhvien)&&s.MaMH.Equals(monhoc)&&s.NamHoc.Equals(namhoc)&&s.HocKy.Equals(hocky)).SingleOrDefault();
                diem.DiemCC = chuyencan;
                diem.DiemGK = giuaky;
                diem.DiemCK = cuoiky;
                diem.DTB = dtb;
                diem.DiemChu = diemchu;
                diem.KetQua = ketqua;
                db.SaveChanges();
                Response.Write("Sửa thành công");

            }
            catch (Exception e)
            {
                Response.Write("Sửa thất bại");
            }
        }
        [HttpGet]
        public JsonResult Getdata1(string id)
        {
            int Id = Convert.ToInt32(id);
            var list = db.Diems.Where(d => d.Id == Id).ToList();
            var a = JsonConvert.SerializeObject(list);
            return Json(a, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Getdata3(string id)
        {
            var list = (from d in db.Diems
                        from s in db.SinhViens
                        from m in db.MonHocs
                        where d.MaSV.Contains(id) && d.MaSV.Equals(s.MaSV) && d.MaMH.Equals(m.MaMH)
                        select new
                        {
                            d.Id,
                            d.MaSV,
                            s.TenSV,
                            d.MaMH,
                            m.TenMH,
                            d.DiemCC,
                            d.DiemGK,
                            d.DiemCK,
                            d.NamHoc,
                            d.HocKy,
                            d.DTB,
                            d.DiemChu,
                            d.KetQua,
                        });
            var a = JsonConvert.SerializeObject(list);
            return Json(a, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DelTrash(string id)
        {
            int Id = Convert.ToInt32(id);
            Diem diem = db.Diems.Find(Id);
            diem.TrangThai = 0;
            //Cập nhật lại
            db.Entry(diem).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Trash()
        {
            int count = db.Diems.Count(m => m.TrangThai == 0);
            ViewBag.SoLuong = count;
            var list = db.Diems.Where(m => m.TrangThai == 0).ToList();
            return View("Trash", list);
        }
        public ActionResult ReTrash(string id)
        {
            int Id = Convert.ToInt32(id);
            Diem diem = db.Diems.Find(Id);
            diem.TrangThai = 1;
            db.Entry(diem).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Trash", "Diems");
        }
        [HttpPost]
        public ActionResult XuatFile(string file)
        {
            if (file == "1")
            {
                var gv = new GridView();
                gv.DataSource = db.Diems.Where(m => m.TrangThai != 0).ToList();
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=Diem.xls");
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
                    var data = db.Diems.Where(m => m.TrangThai != 0).ToList();
                    GridView gridview = new GridView();
                    gridview.DataSource = data;
                    gridview.DataBind();
                    Response.ClearContent();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename =Diem.doc");
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
                    var list = db.Diems.Where(m => m.TrangThai != 0).ToList();
                    return new PartialViewAsPdf("PrintPDF", list)
                    {
                        FileName = "Diem.pdf"
                    };
                }
            }
        }

        [HttpPost]
        public ActionResult INDSHL(string MaMH)
        {
            float diem = 4;
            var list = db.Diems.Where(m => m.TrangThai != 0&&m.MaMH.Equals(MaMH)&&m.DTB<diem).ToList();
            return new PartialViewAsPdf("PrintPDF", list)
            {
                FileName = "DSHL.pdf"
            };
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
                        sqlBulkCopy.DestinationTableName = "Diem";
                        sqlBulkCopy.ColumnMappings.Add("Id","Id");
                        sqlBulkCopy.ColumnMappings.Add("MaSV","MaSV");
                        sqlBulkCopy.ColumnMappings.Add("MaMH", "MaMH");
                        sqlBulkCopy.ColumnMappings.Add("DiemCC", "DiemCC");
                        sqlBulkCopy.ColumnMappings.Add("DiemGK", "DiemGK");
                        sqlBulkCopy.ColumnMappings.Add("DiemCK", "DiemCK");
                        sqlBulkCopy.ColumnMappings.Add("DTB", "DTB");
                        sqlBulkCopy.ColumnMappings.Add("DiemChu", "DiemChu");
                        sqlBulkCopy.ColumnMappings.Add("KetQua", "KetQua");
                        sqlBulkCopy.ColumnMappings.Add("NamHoc", "NamHoc");
                        sqlBulkCopy.ColumnMappings.Add("HocKy", "HocKy");
                        con.Open();
                        sqlBulkCopy.WriteToServer(dt);
                        con.Close();
                    }
                }
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult TimDrop(string MaMH)
        {
            int count = 0;
            var list = db.Diems.Where(s => s.TrangThai != 0).ToList();
            ViewBag.MaMH = new SelectList(db.MonHocs, "MaMH", "TenMH");
            if (MaMH == "")
            {
                count = db.Diems.Count(m => m.TrangThai != 0);
                ViewBag.SoLuong = count;
                list = db.Diems.Where(s => s.TrangThai != 0).ToList();
            }
            else
            {
                count = db.Diems.Count(m => m.MaMH.Equals(MaMH) && m.TrangThai != 0);
                ViewBag.SoLuong = count;
                list = db.Diems.Where(m => m.MaMH.Equals(MaMH) && m.TrangThai != 0).ToList();
            }
            return View("Index", list.ToPagedList(1,5));
        }
        [HttpGet]
        public ActionResult Index(int? page)
        {
            ViewBag.MaMH = new SelectList(db.MonHocs, "MaMH", "TenMH");
            int count = db.Diems.Count(m => m.TrangThai != 0);
            ViewBag.SoLuong = count;
            var list = db.Diems.Where(m => m.TrangThai != 0).ToList();
            if (page == null) page = 1;
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }
        //SV
        public ActionResult sv_Diem_Index()
        {
            string msv = Session["MaSV"].ToString();
            var diems = db.Diems.Include(d => d.MonHoc).Include(d => d.SinhVien).Where(s=>s.MaSV.Equals(msv));
            return View(diems.ToList());
        }
        //gv
        public ActionResult gv_Diem_Index()
        {
            var diems = db.Diems.Include(d => d.MonHoc).Include(d => d.SinhVien);
            return View(diems.ToList());
        }
        [HttpPost]
        public void Delete(string Id)
        { 
             try
            {
                int id = Convert.ToInt32(Id);
                Diem diem = db.Diems.Find(id);
                db.Diems.Remove(diem);
                db.SaveChanges();
                Response.Write("Xóa thành công");
            }
            catch (Exception e)
            {
                Response.Write("Xóa thất bại");
            }
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
