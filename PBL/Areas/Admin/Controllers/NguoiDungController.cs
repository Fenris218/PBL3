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
    public class NguoiDungController : Controller
    {
        private PBL3Entities7 db = new PBL3Entities7();
        public ActionResult danh_sach()
        {
            return View();
        }
        [HttpGet]
        public JsonResult DsNhomQuyen()
        {
            try
            {
                var dsnhomquyen = (from l in db.nhomquyens.Where(x => x.trangthai != 0)
                             select new
                             {
                                 ID = l.ID,
                                 tennhomquyen = l.tennhomquyen
                             }).ToList();
                return Json(new { code = 200, dsnhomquyen = dsnhomquyen, msg = "Lấy danh sách nhóm quyền thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy danh sách nhóm quyền thất bại!: "+ex.Message, JsonRequestBehavior.AllowGet });
            }
        }
        [HttpGet]
        public JsonResult DsNguoiDung()
        {
            try
            {
                var dsnguoidung = (from l in db.nguoidungs.Where(x => x.trangthai != 0)
                                   select new
                                   {
                                       ID = l.ID,
                                       email = l.email,
                                       hoten = l.hoten,
                                       gioitinh = l.gioitinh,
                                       ngaysinh = l.ngaysinh,
                                       avatar = l.avatar,
                                       ngaythamgia = l.ngaythamgia,
                                       matkhau = l.matkhau,
                                       sodienthoai = l.sodienthoai,
                                       manhomquyen = l.manhomquyen,
                                       manguoidung = l.manguoidung,
                                       trangthai = l.trangthai
                                   }).ToList();
                return Json(new { code = 200, dsnguoidung = dsnguoidung, msg = "Lấy danh sách người dùng thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy danh sách người dùng thất bại!: "+ex.Message, JsonRequestBehavior.AllowGet });
            }
        }

        [HttpPost]
        public JsonResult LayThongTinNguoiDung(int id)
        {
            try
            {
                var nguoidung = db.nguoidungs
                                .Where(x => x.ID == id && x.trangthai != 0)
                                .Select(l => new
                                {
                                    ID = l.ID,
                                    manguoidung = l.manguoidung,
                                    email = l.email,
                                    hoten = l.hoten,
                                    gioitinh = l.gioitinh,
                                    ngaysinh = l.ngaysinh,
                                    sodienthoai = l.sodienthoai,
                                    avatar = l.avatar,
                                    manhomquyen = l.manhomquyen,
                                    ngaythamgia = l.ngaythamgia,
                                    trangthai = l.trangthai,
                                    matkhau = l.matkhau,
                                }).FirstOrDefault();
                if (nguoidung != null)
                {
                    return Json(new { code = 200, nguoidung = nguoidung, msg = "Lấy thông tin người dùng thành công!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { code = 404, msg = "Không tìm thấy người dùng!" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy thông tin người dùng thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ThemNguoiDung(int manguoidung, string email, string hoten, string sodienthoai, string gioitinh, DateTime ngaysinh, int manhomquyen, string matkhau, int trangthai)
        {
            try
            {
                var l = new nguoidung();
                l.manguoidung = manguoidung;
                l.email = email;
                l.hoten = hoten;
                l.sodienthoai = sodienthoai;
                l.gioitinh = gioitinh;
                l.ngaysinh = ngaysinh;
                l.manhomquyen = manhomquyen;
                l.matkhau = matkhau;
                l.ngaythamgia = DateTime.Now;
                l.trangthai = trangthai;
                db.nguoidungs.Add(l);
                db.SaveChanges();

                return Json(new { code = 200, msg = "Thêm người dùng thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Thêm người dùng thất bại. Lỗi: "+ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult CheckNguoiDung(int manguoidung, string email)
        {
            try
            {
                var nguoidung = db.nguoidungs.Any(x => x.manguoidung == manguoidung || x.email == email);
                if (nguoidung)
                {
                    return Json(new { code = 200, exists = true, msg = "Người dùng đã tồn tại!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { code = 200, exists = false, msg = "Người dùng chưa tồn tại." }, JsonRequestBehavior.AllowGet);
                }    
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lỗi kiểm tra người dùng: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult XoaNguoiDung(int id)
        {
            try
            {
                var nguoidung = db.nguoidungs.FirstOrDefault(x => x.ID == id);
                if (nguoidung != null)
                {
                    nguoidung.trangthai = 0; 
                    db.SaveChanges();
                    return Json(new { code = 200, msg = "Xóa người dùng thành công!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { code = 404, msg = "Không tìm thấy người dùng để xóa!" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Xóa người dùng thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
