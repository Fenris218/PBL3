using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using PBL.Models;

namespace PBL.Areas.Admin.Controllers
{
    public class MonHocController : Controller
    {
        private PBL3Entities7 db = new PBL3Entities7();

        // GET: Admin/MonHoc
        public ActionResult danh_sach()
        {
            return View();
        }
        [HttpGet]
        public JsonResult DsMon()
        {
            try
            {
                var dsmon = (from l in db.monhocs.Where(x => x.trangthai != 0)
                             select new
                             {
                                 ID = l.ID,
                                 mamonhoc = l.mamonhoc,
                                 tenmonhoc = l.tenmonhoc,
                                 sotinchi = l.sotinchi,
                                 sotietthuchanh = l.sotietthuchanh,
                                 sotietlithuyet = l.sotietlithuyet
                             }).ToList();
                return Json(new { code = 200, dsmon = dsmon, msg = "Lấy danh sách môn học thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy danh sách môn học thất bại!: "+ex.Message, JsonRequestBehavior.AllowGet });
            }
        }
        [HttpGet]
        public JsonResult chiTietMonHoc(int mamon)
        {
            try
            {
                var monhoc = db.monhocs.Where(x => x.trangthai != 0 && x.ID == mamon)
                                       .Select(x => new
                                       {
                                           ID = x.ID,
                                           mamonhoc = x.mamonhoc,
                                           tenmonhoc = x.tenmonhoc,
                                           sotinchi = x.sotinchi,
                                           sotietlithuyet = x.sotietlithuyet,
                                           sotietthuchanh = x.sotietthuchanh
                                       }).FirstOrDefault();

                if (monhoc != null)
                {
                    return Json(new { code = 200, monhoc = monhoc, msg = "Lấy chi tiết môn học thành công!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { code = 404, msg = "Môn học không tồn tại!" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy chi tiết môn học thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult kiemTraMonHoc(int mamon)
        {
            try
            {
                bool kiemTraMonHoc = db.monhocs.Any(x => x.trangthai != 0 && x.mamonhoc == mamon);
                if (kiemTraMonHoc) return Json(new { code = 200, kiemTraMonHoc = true, msg = "Môn học đã tồn tại!" }, JsonRequestBehavior.AllowGet);
                else return Json(new { code = 200, kiemTraMonHoc = false, msg = "Môn học chưa tồn tại!" }, JsonRequestBehavior.AllowGet);  
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lỗi kiểm tra môn học:" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult SuaMon(int id, int mamon, string tenmon, int sotinchi, int sotietlythuyet, int sotietthuchanh)
        {
            try
            {
                var monhoc = db.monhocs.SingleOrDefault(x => x.ID == id && x.trangthai != 0);
                monhoc.mamonhoc = mamon;
                monhoc.tenmonhoc = tenmon;
                monhoc.sotinchi = sotinchi;
                monhoc.sotietlithuyet = sotietlythuyet;
                monhoc.sotietthuchanh = sotietthuchanh;
                db.Entry(monhoc).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { code = 200 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Cập nhật thông tin chương thất bại. Lỗi: "+ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult ThemMon(int mamonhoc, string tenmonhoc, int sotinchi, int sotietlithuyet, int sotietthuchanh)
        {
            try
            {
                var l = new monhoc();
                l.mamonhoc = mamonhoc;
                l.tenmonhoc = tenmonhoc;
                l.sotinchi = sotinchi;
                l.sotietlithuyet = sotietlithuyet;
                l.sotietthuchanh = sotietthuchanh;
                l.trangthai = 1;
                db.monhocs.Add(l);
                db.SaveChanges();

                return Json(new { code = 200, msg = "Thêm môn học thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Thêm môn học thất bại. Lỗi: "+ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult TatCaMonHoc()
        {
            try
            {
                var all = (from mh in db.monhocs.Where(x => x.trangthai != 0)
                           select new
                           {
                               ID = mh.ID,
                               tenmonhoc = mh.tenmonhoc,
                               mamonhoc = mh.mamonhoc
                           }).ToList();
                return Json(new { code = 200, all = all, msg = "Lấy tất cả danh sách môn học thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy tất cả danh sách môn học thất bại. Lỗi: "+ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult DsChuong(int ID)
        {
            try
            {
                var dschuong = (from l in db.chuongs.Where(x => x.mamonhoc == ID && x.trangthai != 0)
                                select new
                                {
                                    ID = l.ID,
                                    tenchuong = l.tenchuong,
                                }).ToList();
                return Json(new { code = 200, dschuong = dschuong, msg = "Lấy danh sách chương thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy danh sách chương thất bại!: "+ex.Message, JsonRequestBehavior.AllowGet });
            }
        }

        [HttpGet]
        public JsonResult ChiTietChuong(int ID)
        {
            try
            {
                var dschuong = (from l in db.chuongs.Where(x => x.ID == ID)
                                select new
                                {
                                    tenchuong = l.tenchuong,
                                }).ToList();
                return Json(new { code = 200, dschuong = dschuong.Count>0 ? dschuong[0] : null, msg = "Lấy thông tin chương thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy thông tin chương thất bại!: "+ex.Message, JsonRequestBehavior.AllowGet });
            }
        }

        [HttpPost]
        public ActionResult ThemChuong(string tenchuong, int mamonhoc)
        {
            try
            {
                var chuong = new chuong();
                chuong.tenchuong = tenchuong;
                chuong.mamonhoc = mamonhoc;
                chuong.trangthai = 1;
                db.chuongs.Add(chuong);
                db.SaveChanges();

                return Json(new { code = 201, msg = "Thêm chương thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Thêm chương thất bại. Lỗi: "+ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult SuaChuong(int id, string tenchuong)
        {
            try
            {
                var chuong = db.chuongs.SingleOrDefault(x => x.ID == id && x.trangthai != 0);
                chuong.tenchuong = tenchuong;
                db.Entry(chuong).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { code = 200, msg = "Cập nhật thông tin chương thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Cập nhật thông tin chương thất bại. Lỗi: "+ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpDelete]
        public JsonResult XoaChuong(int id)
        {
            try
            {
                var chuong = db.chuongs.SingleOrDefault(x => x.ID == id);
                chuong.trangthai = 0;
                db.SaveChanges();
                return Json(new { code = 200, msg = "Chương đã bị xóa!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Chương xóa thất bại. Lỗi: "+ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
