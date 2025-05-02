using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PBL.Models;

namespace PBL.Areas.Admin.Controllers
{
    public class PhanCongController : Controller
    {
        private PBL3Entities7 db = new PBL3Entities7();

        public ActionResult danh_sach()
        {
            return View();
        }
        [HttpGet]     
        
        public JsonResult DsPhanCong()
        {
            try
            {
                var dsphancong = (from pc in db.phancongs
                                join mh in db.monhocs on pc.mamonhoc equals mh.ID
                                join nd in db.nguoidungs on pc.manguoidung equals nd.ID
                                where pc.trangthai != 0
                                select new
                                {
                                    ID = pc.ID,
                                    hoten = nd.hoten,
                                    mamonhoc = mh.mamonhoc,
                                    tenmonhoc = mh.tenmonhoc,
                                    sotinchi = mh.sotinchi,
                                    sotietthuchanh = mh.sotietthuchanh,
                                    sotietlithuyet = mh.sotietlithuyet,
                                    IDmonhoc = mh.ID,
                                    manguoidung = nd.ID
                                }).ToList();

                return Json(new { code = 200, dsphancong = dsphancong, msg = "Lấy danh sách phân công thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy danh sách phân công thất bại! " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult DsGiangVien()
        {
            try
            {
                var dsgiangvien = (from nd in db.nguoidungs 
                                  where nd.trangthai != 0 && nd.manhomquyen == 2
                                  select new
                                  {
                                      ID = nd.ID,
                                      email = nd.email,
                                      hoten = nd.hoten, 
                                      gioitinh = nd.gioitinh,
                                      ngaysinh = nd.ngaysinh,
                                  }).ToList();

                return Json(new { code = 200, dsgiangvien = dsgiangvien, msg = "Lấy danh sách giảng viên thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy danh sách giảng viên thất bại! " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
