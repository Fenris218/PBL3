using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using PBL.Models;


namespace PBL.Areas.Admin.Controllers
{
    public class NhomController : Controller
    {
        private PBL3Entities7 db = new PBL3Entities7();
        public ActionResult nhom()
        {
            return View();
        }

        public ActionResult danh_sach()
        {
            return View();
        }
        public ActionResult chi_tiet_nhom(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("admin_nhom_danhsach");
            }

            ViewBag.IDnhom = id.Value;
            return View();
        }
        [HttpGet]
        public JsonResult DsNhom()
        {
            try
            {
                var dsnhom = (from l in db.nhoms.Where(x => x.trangthai != 0)
                                select new
                                {
                                    ID = l.ID,
                                    trangthai = l.trangthai,
                                    tennhom = l.tennhom,
                                    siso = l.siso,
                                    hocky = l.hocky,
                                    giangvien = l.giangvien,
                                    mamonhoc = l.mamonhoc,
                                    mamoi = l.mamoi,
                                    namhoc = l.namhoc
                                }).ToList();
                return Json(new { code = 200, dsnhom = dsnhom, msg = "Lấy danh sách nhóm thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy danh sách nhóm thất bại!: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult DsNhomByID(int IDnhom)
        {
            try
            {
                var dsnhom = (from l in db.nhoms.Where(x => x.trangthai != 0 && x.ID == IDnhom)
                              select new
                              {
                                  ID = l.ID,
                                  tennhom = l.tennhom,
                                  siso = l.siso,
                                  hocky = l.hocky,
                                  ghichu = l.ghichu,
                                  giangvien = l.giangvien,
                                  mamonhoc = l.mamonhoc,
                                  mamoi = l.mamoi,
                                  namhoc = l.namhoc
                              }).FirstOrDefault();
                return Json(new { code = 200, dsnhom = dsnhom, msg = "Lấy danh sách nhóm thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy danh sách nhóm thất bại!: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult DsSinhVien(int IDnhom)
        {
            try
            {
                var dssinhvien = (from nd in db.nguoidungs where db.chitietnhoms.Any(ctn => ctn.manhom == IDnhom && ctn.manguoidung == nd.ID && ctn.hienthi != 0)
                                select new
                                {
                                    ID = nd.ID,
                                    email = nd.email,
                                    hoten = nd.hoten,
                                    gioitinh = nd.gioitinh,
                                    ngaysinh = nd.ngaysinh,
                                    matkhau = nd.matkhau,
                                    sodienthoai = nd.sodienthoai,
                                    manguoidung = nd.manguoidung
                                }).ToList();
                return Json(new { code = 200, dssinhvien = dssinhvien, msg = "Lấy danh sách sinh viên thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy danh sách sinh viên thất bại!: "+ex.Message, JsonRequestBehavior.AllowGet });
            }
        }

        private int SoNgauNhien(int min = 100000, int max = 999999)
        {
            var random = new Random();
            return random.Next(min, max + 1);
        }

        [HttpPost]
        public JsonResult CapNhatMaMoi(int nhomId)
        {
            try
            {
                nhom capnhatnhom = db.nhoms.Find(nhomId);
                if (capnhatnhom == null)
                {
                    return Json(new { code = 404, msg = "Không tìm thấy nhóm!" }, JsonRequestBehavior.AllowGet);
                }

                int mamoi = SoNgauNhien();
                capnhatnhom.mamoi = mamoi;
                db.SaveChanges();

                return Json(new { code = 200, mamoi = mamoi, msg = "Cập nhật mã mời thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Cập nhật mã mời thất bại!: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GetMonHoc(int hienthi)
        {
            try
            {
                var query = db.nhoms
                    .Include(n => n.monhoc)
                    .Where(n => n.trangthai != 0);
                if(hienthi != 2)
                {
                    query = query.Where(n => n.hienthi == hienthi);
                }    
                            
                var grouped = query
                    .AsEnumerable()
                    .GroupBy(n => new {n.mamonhoc, n.monhoc.tenmonhoc, n.namhoc, n.hocky })
                    .Select(g => new
                    {
                        mamonhoc = g.Key.mamonhoc,
                        tenmonhoc = g.Key.tenmonhoc,
                        namhoc = g.Key.namhoc,
                        hocky = g.Key.hocky,
                        nhom = g.Select(n => new
                        {
                            manhom = n.ID,
                            tennhom = n.tennhom,
                            siso = n.siso,
                            hienthi = n.hienthi
                        }).ToList()
                    }).ToList();

                return Json(new { code = 200, msg = "Lấy dữ liệu thành công!", data = grouped }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new { code = 500, msg = "Lỗi khi lấy dữ liệu!: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GetMonHocDaBiAn()
        {
            try
            {
                var query = db.nhoms
                    .Include(n => n.monhoc)
                    .Where(n => n.trangthai != 0 && n.hienthi == 0);

                var grouped = query
                    .AsEnumerable()
                    .GroupBy(n => new { n.mamonhoc, n.monhoc.tenmonhoc, n.namhoc, n.hocky })
                    .Select(g => new
                    {
                        mamonhoc = g.Key.mamonhoc,
                        tenmonhoc = g.Key.tenmonhoc,
                        namhoc = g.Key.namhoc,
                        hocky = g.Key.hocky,
                        nhom = g.Select(n => new
                        {
                            manhom = n.ID,
                            tennhom = n.tennhom,
                            siso = n.siso,
                            hienthi = n.hienthi
                        }).ToList()
                    }).ToList();

                return Json(new { code = 200, msg = "Lấy danh sách nhóm bị ẩn thành công!", data = grouped }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lỗi khi lấy dữ liệu!: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult AnNhom(int id, int hienthi)
        {
            try
            {
                var nhom = db.nhoms.SingleOrDefault(x => x.ID == id && x.trangthai != 0);
                nhom.hienthi = hienthi;
                db.Entry(nhom).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { code = 200 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Ẩn/ hiện nhóm thất bại. Lỗi: "+ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
