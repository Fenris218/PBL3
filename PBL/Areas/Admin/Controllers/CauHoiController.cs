using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography.Xml;
using System.Web;
using System.Web.Mvc;
using PBL.Models;

namespace PBL.Areas.Admin.Controllers
{
    public class CauHoiController : Controller
    {
        private PBL3Entities7 db = new PBL3Entities7();

        // GET: Admin/CauHoi
        public ActionResult danh_sach()
        {
            return View();
        }
        [HttpGet]

        public JsonResult DsCauHoi()
        {
            try
            {
                var dscauhoi = (from ch in db.cauhois
                                join mh in db.monhocs on ch.mamonhoc equals mh.ID
                                where ch.trangthai != 0
                                select new
                                {
                                    ID = ch.ID,
                                    noidung = ch.noidung,
                                    madokho = ch.madokho,
                                    mamonhoc = ch.mamonhoc,
                                    tenmonhoc = mh.tenmonhoc,
                                    machuong = ch.machuong,
                                    nguoitao = ch.nguoitao
                                }).ToList();

                return Json(new { code = 200, dscauhoi = dscauhoi, msg = "Lấy danh sách câu hỏi thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy danh sách câu hỏi thất bại! " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetCauHoiByID(int ID)
        {
            try
            {
                var cauhoi = db.cauhois.Where(x => x.ID == ID && x.trangthai != 0)
                                       .Select(x => new
                                       {
                                           noidung = x.noidung,
                                           madokho = x.madokho,
                                           mamonhoc = x.mamonhoc,
                                           machuong = x.machuong
                                       }).SingleOrDefault();
                return Json(new { code = 200, cauhoi = cauhoi, msg = "Lấy câu hỏi thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy câu hỏi thất bại! " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GetDapAnByID(int ID)
        {
            try
            {
                var dsdapan = (from da in db.tralois
                               join ch in db.cauhois on da.macauhoi equals ch.ID
                               where ch.ID == ID && da.trangthai != 0
                               select new
                               {
                                   ID = da.ID,
                                   noidung = da.noidung,
                                   ladapan = da.ladapan
                               }).ToList();
                return Json(new { code = 200, dsdapan = dsdapan }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy câu hỏi thất bại! " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult SuaCauHoi(int ID, string noidung, int madokho, int mamonhoc, int machuong)
        {
            try
            {
                var cauhoi = db.cauhois.SingleOrDefault(x => x.ID == ID && x.trangthai != 0);
                cauhoi.noidung = noidung;
                cauhoi.madokho = madokho;
                cauhoi.mamonhoc = mamonhoc;
                cauhoi.machuong = machuong;
                db.Entry(cauhoi).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { code = 200 }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new { code = 500, msg = "Sửa câu hỏi thất bại! " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult ThemCauHoi(string noidung, int madokho, int mamonhoc, int machuong)
        {
            try
            {
                var l = new cauhoi();
                l.noidung = noidung;
                l.madokho = madokho;
                l.mamonhoc = mamonhoc;
                l.machuong = machuong;
                l.nguoitao = null;
                l.trangthai = 1;
                db.cauhois.Add(l);
                db.SaveChanges();
                return Json(new { code = 200, cauhoiid = l.ID, msg = "Thêm câu hỏi thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Thêm câu hỏi thất bại. Lỗi: "+ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult TatCaMonHoc()
        {
            try
            {
                var all = (from mh in db.monhocs.Where(x =>x.trangthai != 0)
                           select new
                           {
                               ID = mh.ID,
                               tenmonhoc = mh.tenmonhoc
                           }).ToList();
                return Json(new { code = 200, all = all, msg = "Lấy tất cả danh sách môn học thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy tất cả danh sách môn học thất bại. Lỗi: "+ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult TatCaChuongByIDMonHoc(int IDMonHoc)
        {
            try
            {
                var all = (from mh in db.chuongs.Where(x => x.mamonhoc == IDMonHoc && x.trangthai != 0)
                           select new
                           {
                               ID = mh.ID,
                               tenchuong = mh.tenchuong
                           }).ToList();
                return Json(new { code = 200, all = all, msg = "Lấy tất cả danh sách chương thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy tất cả danh sách chương thất bại. Lỗi: "+ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult TatCaDoKho()
        {
            try
            {
                var all = (from mh in db.dokhoes
                           select new
                           {
                               ID = mh.ID,
                               tendokho = mh.tendokho
                           }).ToList();
                return Json(new { code = 200, all = all, msg = "Lấy tất cả danh sách độ khó thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy tất cả danh sách độ khó thất bại. Lỗi: "+ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ThemTraLoi(string noidung, int macauhoi, bool ladapan)
        {
            try
            {
                var traloi = new traloi();
                traloi.noidung = noidung;
                traloi.macauhoi = macauhoi;
                traloi.ladapan = ladapan;
                traloi.trangthai = 1;
                db.tralois.Add(traloi);
                db.SaveChanges();
                return Json(new { code = 201, msg = "Thêm câu trả lời thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Thêm câu trả lời thất bại. Lỗi: "+ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult DsTraLoi(int IDCauHoi)
        {
            try
            {
                var dstraloi = (from l in db.tralois.Where(x => x.macauhoi == IDCauHoi && x.trangthai != 0)
                                select new
                                {
                                    ID = l.ID,
                                    macauhoi = l.macauhoi,
                                    noidung = l.noidung,
                                    ladapan = l.ladapan
                                }).ToList();
                return Json(new { code = 200, dstraloi = dstraloi, msg = "Lấy danh sách câu trả lời thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy danh sách câu trả lời thất bại!: "+ex.Message, JsonRequestBehavior.AllowGet });
            }
        }
        [HttpGet]
        public JsonResult DsChuoiTraLoi(int[] idCauHoiArray) 
        {
            try
            {
                if (idCauHoiArray == null || idCauHoiArray.Length == 0)
                {
                    return Json(new
                    {
                        code = 400,
                        msg = "Danh sách ID câu hỏi không hợp lệ hoặc rỗng"
                    }, JsonRequestBehavior.AllowGet);
                }
                var dstraloi = db.tralois
                                .Where(x => idCauHoiArray.Contains(x.macauhoi) && x.trangthai != 0)
                                .Select(l => new
                                {
                                    ID = l.ID,
                                    macauhoi = l.macauhoi,
                                    noidung = l.noidung,
                                    ladapan = l.ladapan
                                })
                                .ToList();

                return Json(new
                {
                    code = 200,
                    dstraloi = dstraloi,
                    msg = "Lấy danh sách câu trả lời thành công!"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    msg = "Lấy danh sách câu trả lời thất bại!: " + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Test()
        {
            return View();
        }
        [HttpPost]
        public JsonResult XoaCauHoi(int id)
        {
            try
            {
                var cauhoi = db.cauhois.SingleOrDefault(x => x.ID == id && x.trangthai != 0);
                if(cauhoi == null)
                {
                    return Json(new { code = 404, msg = "Câu hỏi không tồn tại!" }, JsonRequestBehavior.AllowGet);
                }
                var dapans = db.tralois.Where(x => x.macauhoi == id && x.trangthai != 0).ToList();
                if(dapans != null) db.tralois.RemoveRange(dapans);
                db.cauhois.Remove(cauhoi);
                db.SaveChanges();
                return Json(new { code = 200, msg = "Xóa câu hỏi và đáp án thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Xóa câu hỏi thất bại! Lỗi: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult DsCauHoiTheoMonHoc(int idmonhoc)
        {
            try
            {
                var dscauhoi = (from ch in db.cauhois
                                join mh in db.monhocs on ch.mamonhoc equals mh.ID
                                where ch.trangthai != 0 && ch.mamonhoc == idmonhoc
                                select new {
                                    ID = ch.ID,
                                    noidung = ch.noidung,
                                    madokho = ch.madokho,
                                    mamonhoc = ch.mamonhoc,
                                    tenmonhoc = mh.tenmonhoc,
                                    machuong = ch.machuong,
                   
                                }).ToList();
                return Json(new { code = 200, dscauhoi = dscauhoi, msg = "Lấy danh sách câu hỏi theo môn học thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy danh sách câu hỏi theo môn học thất bại! " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
