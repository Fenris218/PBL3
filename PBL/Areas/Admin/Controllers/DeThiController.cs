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
    public class DeThiController : Controller
    {
        private PBL3Entities7 db = new PBL3Entities7();

        // GET: Admin/DeThi
        public ActionResult danh_sach()
        {
            return View();

        }
        public ActionResult select_question(int? id)
        {
            ViewBag.ID = id.Value;
            return View();
        }
        public ActionResult them_de_thi()
        {
            ViewBag.Action = "create";
            return View();
        }

        public ActionResult chi_tiet_de_thi(int? id)
        {
            dethi a = db.dethis.FirstOrDefault(d => d.ID == id.Value && d.trangthai != 0);
            monhoc b = db.monhocs.FirstOrDefault(d => d.ID == a.mamon && d.trangthai != 0);
            if(a == null)
            {
                return HttpNotFound("Không tìm thấy đề thi có ID = " + id.Value);
            }    
            ViewBag.tendethi = a.tende;
            ViewBag.thoigiantao = a.thoigiantao;
            ViewBag.tenmonhoc = b.tenmonhoc;
            ViewBag.ID = id.Value;
            return View();
        }

        public ActionResult sua_de_thi(int? id)
        {
            ViewBag.Action = "update";
            ViewBag.ID = id.Value;
            return View();
        }
        public ActionResult CreateDeThi_CauHoi()
        {
            dethi deThiVuaTao = Session["DeThiVuaTao"] as dethi;

            if (deThiVuaTao != null)
            {
                Session["DeThiVuaTao"] = deThiVuaTao;
                return View(deThiVuaTao);
            }
            else
            {
                return RedirectToAction("Create");
            }
        }

        [HttpPost]
        public JsonResult taodethi(int mamonthi, string tende, int thoigianthi, DateTime thoigianbatdau, DateTime thoigianketthuc, bool hienthibailam, bool xemdiemthi, bool xemdapan, bool troncauhoi, bool trondapan, bool nopbaichuyentab, int loaide, int socaude, int socautb, int socaukho, List<int> chuongs, List<int> nhoms)
        {
            try
            {
                var dethi = new dethi
                {
                    mamon = mamonthi,
                    nguoitao = 1,
                    tende = tende,
                    thoigianthi = thoigianthi,
                    thoigianbatdau = thoigianbatdau,
                    thoigianketthuc = thoigianketthuc,
                    hienthibailam = hienthibailam,
                    xemdiemthi = xemdiemthi,
                    xemdapan = xemdapan,
                    troncauhoi = troncauhoi,
                    trondapan = trondapan,
                    nopbaichuyentab = nopbaichuyentab,
                    loaide = loaide,
                    socaude = socaude,
                    socautb = socautb,
                    socaukho = socaukho,
                    trangthai = 1,
                    thoigiantao = DateTime.Now
                };
                db.dethis.Add(dethi);
                db.SaveChanges();
                taogiaodethi(dethi.ID, nhoms);
                taochuongdethi(dethi.ID, chuongs);
                return Json(new { code = 200, msg = "Tạo đề thi thành công!", made = dethi.ID });
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Tạo đề thi thất bại. Lỗi: " + ex.Message });
            }
        }

        [HttpGet]
        public JsonResult DsDeThi()
        {
            try
            {
                var dsdethi = (from l in db.dethis.Where(x => x.trangthai != 0)
                               select new
                               {
                                   ID = l.ID,
                                   tende = l.tende,
                                   mamon = l.mamon,
                                   thoigianthi = l.thoigianthi,
                                   thoigianbatdau = l.thoigianbatdau,
                                   thoigianketthuc = l.thoigianketthuc,
                                   nguoitao = l.nguoitao
                               }).ToList();
                return Json(new { code = 200, dsdethi = dsdethi, msg = "Lấy danh sách đề thi thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy danh sách đề thi thất bại!" +ex.Message, JsonRequestBehavior.AllowGet });
            }
        }

        [HttpGet]
        public JsonResult taodethitudong(int mamonhoc, List<int> chuongs, int socaude, int socautb, int socaukho)
        {
            try
            {
                var caude = db.cauhois
                            .Where(ch => ch.mamonhoc == mamonhoc && ch.madokho == 1 && ch.trangthai == 1 && chuongs.Contains(ch.machuong))
                            .OrderBy(x => Guid.NewGuid())
                            .Take(socaude)
                            .ToList();
                var cautb = db.cauhois
                            .Where(ch => ch.mamonhoc == mamonhoc && ch.madokho == 2 && ch.trangthai == 1 && chuongs.Contains(ch.machuong))
                            .OrderBy(x => Guid.NewGuid())
                            .Take(socautb)
                            .ToList();
                var caukho = db.cauhois
                            .Where(ch => ch.mamonhoc == mamonhoc && ch.madokho == 3 && ch.trangthai == 1 && chuongs.Contains(ch.machuong))
                            .OrderBy(x => Guid.NewGuid())
                            .Take(socaukho)
                            .ToList();
                var dscauhoi = caude.Concat(cautb).Concat(caukho).OrderBy(x => Guid.NewGuid()).ToList();

                var result = dscauhoi.Select(x => new
                {
                    ID = x.ID,
                    noidung = x.noidung,
                    mamonhoc = x.mamonhoc, 
                    machuong = x.machuong,
                    madokho = x.madokho
                });
                return Json(new { code = 200, dscauhoi = result, msg = "Tạo đề thi tự động thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new { code = 500, msg = "Tạo đề thi thất bại. Lỗi: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult taochuongdethi(int made, List<int> chuongs)
        {
            try
            {
                foreach(var machuong in chuongs)
                {
                    var item = new dethitudong
                    {
                        made = made,
                        machuong = machuong
                    };
                    db.dethitudongs.Add(item);
                }
                db.SaveChanges();
                return Json(new { code = 200, msg = "Tạo chương đề thi tự động thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new { code = 500, msg = "Tạo chương đề thi thất bại. Lỗi: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public bool taogiaodethi(int made, List<int> nhoms)
        {
            try
            {
                foreach(var manhom in nhoms)
                {
                    var item = new giaodethi
                    {
                        made = made,
                        manhom = manhom,
                        maketqua = 1,
                        manguoidung = 1,
                        diemthi = 2,
                        thoigianvaothi = null,
                        thoigianlambai = 3,
                        socaudung = 1,
                        solanchuyentab = 2,
                    };
                    db.giaodethis.Add(item);
                }
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        [HttpGet]
        public JsonResult TatCaHocVaMonHoc()
        {
            try
            {
                var all = (from mh in db.monhocs
                           join nh in db.nhoms.Where(x => x.trangthai != 0) on mh.ID equals nh.mamonhoc
                           select new
                           {
                               ID = mh.ID,
                               mamonhoc = mh.mamonhoc,
                               tenmonhoc = mh.tenmonhoc,
                               tennhom = nh.tennhom,
                               hocky = nh.hocky
                           }).ToList();
                return Json(new { code = 200, all = all, msg = "Lấy tất cả danh sách môn học thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy tất cả danh sách môn học thất bại. Lỗi: "+ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult ThemDeThi(string tende, int mamon, int thoigianthi, DateTime thoigianbatdau, DateTime thoigianketthuc, int socaude, int socautb, int socaukho, bool xemdiemthi, bool troncauhoi, bool trondapan, bool nopbaichuyentab, bool hienthibailam, bool xemdapan, int loaide)
        {
            try
            {
                var l = new dethi();
                l.tende = tende;
                l.mamon = mamon;
                l.thoigiantao = DateTime.Now;
                l.thoigianthi = thoigianthi;
                l.thoigianbatdau = thoigianbatdau;
                l.thoigianketthuc = thoigianketthuc;
                l.hienthibailam = hienthibailam;
                l.xemdiemthi = xemdiemthi;
                l.xemdapan = xemdapan;
                l.troncauhoi = troncauhoi;
                l.trondapan = trondapan;
                l.nopbaichuyentab = nopbaichuyentab;
                l.socaude = socaude;
                l.socautb = socautb;
                l.socaukho = socaukho;
                l.loaide = loaide;
                l.nguoitao = 1;
                l.trangthai = 1;
                db.dethis.Add(l);
                db.SaveChanges();
                return Json(new { code = 201, madethi = l.ID, msg = "Thêm đề thi thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Thêm đề thi thất bại. Lỗi: "+ex.Message }, JsonRequestBehavior.AllowGet);
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
        public JsonResult DsCauHoiByIDMonHocvaIDChuong(int IDMonHoc, int IDChuong)
        {
            try
            {
                var dscauhoi = (from l in db.cauhois.Where(x => x.trangthai != 0 && x.mamonhoc == IDMonHoc && x.machuong == IDChuong)
                                select new
                                {
                                    ID = l.ID,
                                    noidung = l.noidung,
                                    madokho = l.madokho
                                }).ToList();
                return Json(new { code = 200, dscauhoi = dscauhoi, msg = "Lấy danh sách câu hỏi thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy danh sách câu hỏi thất bại!: "+ex.Message, JsonRequestBehavior.AllowGet });
            }
        }
        [HttpPost]
        public JsonResult ThemDeThiChiTiet(int madethi, int macauhoi, int thutu)
        {
            try
            {
                var l = new chitietdethi();
                l.made = madethi;
                l.macauhoi = macauhoi;
                l.thutu = thutu;
                db.chitietdethis.Add(l);
                db.SaveChanges();
                return Json(new { code = 201, chitietdethi = l, msg = "Thêm chi tiết đề thi thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Thêm chi tiết đề thi thất bại. Lỗi: "+ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult DsCauHoiBychitietdethi(int madethi)
        {
            try
            {
                var dscauhoidapan = (from ch in db.cauhois
                                     join tl in db.tralois on ch.ID equals tl.macauhoi
                                     where ch.trangthai != 0 &&
                                db.chitietdethis.Any(ct => ct.made == madethi && ct.macauhoi == ch.ID)
                                     select new
                                     {
                                         ID = ch.ID,
                                         noidung = ch.noidung,
                                         noidungdapan = tl.noidung,
                                         ladapan = tl.ladapan
                                     }).ToList();
                return Json(new { code = 200, dscauhoidapan = dscauhoidapan, msg = "Lấy danh sách câu hỏi thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy danh sách câu hỏi thất bại!: "+ex.Message, JsonRequestBehavior.AllowGet });
            }
        }
        [HttpPost]
        public JsonResult GetSoLuongCauHoi(List<int> chuongs, int monhoc, int dokho)
        {
            try
            {
                int soluong;
                var query = db.cauhois.Where(ch => ch.madokho == dokho && ch.trangthai == 1);
                if (chuongs != null && chuongs.Count > 0)
                {
                    query = query.Where(ch => chuongs.Contains(ch.machuong));
                }
                else
                {
                    query = query.Where(ch => ch.mamonhoc == monhoc);
                }
                soluong = query.Count();
                return Json(new { code = 200, soluong = soluong, msg = "Lấy số lượng câu hỏi thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy số lượng câu hỏi thất bại!: "+ex.Message, JsonRequestBehavior.AllowGet });
            }
        }

        [HttpPost]
        public JsonResult LayThongTinDeThi(int id)
        {
            try
            {
                var dethi = db.dethis.FirstOrDefault(x => x.ID == id);
                if (dethi != null)
                {
                    var chuongs = db.dethitudongs
                            .Where(c => c.made == id)
                            .Join(
                                db.chuongs,
                                c => c.machuong,
                                ch => ch.ID,
                                (c, ch) => new
                                {
                                    ch.ID,
                                    ch.tenchuong
                                }
                            )
                            .ToList();
                    var nhoms = db.giaodethis
                            .Where(g => g.made == id)
                            .Join(
                                db.nhoms,
                                g => g.manhom,
                                n => n.ID,
                                (g, n) => new
                                {
                                    n.ID,
                                    n.hocky,
                                    n.namhoc,
                                    n.mamonhoc
                                }
                            )
                            .Join(
                                db.monhocs,
                                n => n.mamonhoc,
                                m => m.ID,
                                (n, m) => new
                                {
                                    n.ID,
                                    n.hocky,
                                    n.namhoc,
                                    m.tenmonhoc
                                }
                                )
                            .ToList();
                    var thongtin = new
                    {
                        ID = dethi.ID,
                        tende = dethi.tende,
                        mamon = dethi.mamon,
                        thoigiantao = dethi.thoigiantao,
                        thoigianthi = dethi.thoigianthi,
                        thoigianbatdau = dethi.thoigianbatdau,
                        thoigianketthuc = dethi.thoigianketthuc,
                        hienthibailam = dethi.hienthibailam,
                        xemdiemthi = dethi.xemdiemthi,
                        xemdapan = dethi.xemdapan,
                        troncauhoi = dethi.troncauhoi,
                        trondapan = dethi.trondapan,
                        nopbaichuyentab = dethi.nopbaichuyentab,
                        socaude = dethi.socaude,
                        socautb = dethi.socautb,
                        socaukho = dethi.socaukho,
                        loaide = dethi.loaide,
                        nguoitao = dethi.nguoitao,
                        trangthai = dethi.trangthai,
                        chuongs = chuongs,
                        nhoms = nhoms,
                    };
                    return Json(new { code = 200, dethi = thongtin, msg = "Lấy thông tin đề thi thành công!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { code = 404, msg = "Không tìm thấy đề thi!" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy thông tin đề thi thất bại! Lỗi: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult XoaDeThi(int id)
        {
            try
            {
                var dethi = db.dethis.FirstOrDefault(x => x.ID == id);
                if (dethi != null)
                {
                    dethi.trangthai = 0;
                    db.SaveChanges();
                    return Json(new { code = 200, msg = "Xóa đề thi thành công!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { code = 404, msg = "Không tìm thấy đề thi cần xóa!" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Xóa đề thi thất bại! Lỗi: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
