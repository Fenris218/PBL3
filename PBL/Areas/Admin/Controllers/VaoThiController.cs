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
    public class VaoThiController : Controller
    {
        private PBL3Entities7 db = new PBL3Entities7();
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult Dscauhoi()
        {
            try
            {
                var dscauhoidapan = (from ch in db.cauhois
                                     join tl in db.tralois on ch.ID equals tl.macauhoi
                                     where ch.trangthai != 0 &&
                                db.chitietdethis.Any(ct => ct.made == 1 && ct.macauhoi == ch.ID)
                                     select new
                                     {

                                         ID = ch.ID,
                                         noidung = ch.noidung,
                                         noidungdapan = tl.noidung,
                                         ladapan = tl.ladapan.ToString().ToLower()
                                     }).ToList();

                return Json(new { code = 200, dscauhoidapan = dscauhoidapan, msg = "Lấy danh sách câu hỏi thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy danh sách câu hỏi thất bại!: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult thongtindethi()
        {
            try
            {
                var thongtindethi = (from l in db.dethis.Where(x => x.ID == 1 && x.trangthai != 0)
                                     select new
                                     {
                                        tende = l.tende,
                                        thoigianthi = l.thoigianthi
                                     }).ToList();

                return Json(new { code = 200, thongtindethi = thongtindethi, msg = "Lấy thông tin đề thi thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy thông tin đề thi thất bại!: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
