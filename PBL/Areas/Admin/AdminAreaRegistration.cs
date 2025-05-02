using System.Web.Mvc;

namespace PBL.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "admin_phancong_danhsach",
                "phancong/danhsach",
                new { area = "Admin", controller = "PhanCong", action = "danh_sach", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "admin_nhom_chitiet",
                "nhom/chitiet/{id}",
                new { area = "Admin", controller = "Nhom", action = "chi_tiet_nhom", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "admin_nhom_danhsach",
                "nhom/danhsach",
                new { area = "Admin", controller = "Nhom", action = "danh_sach", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "admin_nguoidung_danhsach",
                "nguoidung/danhsach",
                new { area = "Admin", controller = "NguoiDung", action = "danh_sach", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "admin_cauhoi_danhsach",
                "cauhoi/danhsach",
                new { area = "Admin", controller = "CauHoi", action = "danh_sach", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "admin_monhoc_danhsach",
                "monhoc/danhsach",
                new { area = "Admin", controller = "MonHoc", action = "danh_sach", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "admin_dethi_danhsach",
                "dethi/danhsach",
                new { area = "Admin", controller = "DeThi", action = "danh_sach", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "admin_dethi_them",
                "dethi/them",
                new { area = "Admin", controller ="DeThi", action = "them_de_thi", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "admin_dethi_choncauhoi",
                "dethi/choncauhoi/{id}",
                new { area = "Admin", controller = "DeThi", action = "select_question", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "admin_dethi_sua",
                "dethi/sua/{id}",
                new { area = "Admin", controller = "DeThi", action = "sua_de_thi", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "admin_dethi_chitiet",
                "dethi/chitiet/{id}",
                new { area = "Admin", controller = "DeThi", action = "chi_tiet_de_thi", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}