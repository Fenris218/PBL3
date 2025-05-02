using System;
using System.Linq;
using System.Web.Mvc;
using PBL.Helpers;
using PBL.Models;

namespace PBL.Areas.Admin.Controllers
{
    public class AuthenticationController : Controller
    {
        private PBL3Entities7 db = new PBL3Entities7();

        //public ActionResult TestConnection()
        //{
        //    try
        //    {
        //        using (var db = new PBL3Entities7())
        //        {
        //            db.Database.Connection.Open();
        //            ViewBag.Message = "Kết nối thành công!";
        //            ViewBag.items = db.nguoidungs.ToList();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.Message = "Lỗi kết nối: " + ex.Message;
        //    }

        //    return View();
        //}

        // GET: Admin/Authenticationentication
        [HttpGet]
        public ActionResult Login()
        {
            if (Session["user"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            var usr = username;
            var pwd = password;
            var acc = db.nguoidungs.SingleOrDefault(x => x.email == usr && x.matkhau == pwd);
            if (acc != null)
            {
                System.Diagnostics.Debug.WriteLine("Đăng nhập thành công với: " + acc.email);
                Session["user"] = acc;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Đăng nhập thất bại!");
                return View();
            }
        }

        public ActionResult Recover()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Recover(string email)
        {
            System.Diagnostics.Debug.WriteLine("Email nhận được: " + email);
            var user = db.nguoidungs.SingleOrDefault(x => x.email == email);
            if (user == null)
            {
                ViewBag.Error = "Email không tồn tại.";
                return View();
            }

            string token = Guid.NewGuid().ToString();
            string otp = new Random().Next(100000, 999999).ToString();

            user.token = token;
            user.token_expire = DateTime.Now.AddMinutes(15);
            user.otp = otp;
            user.otp_created_at = DateTime.Now;
            db.SaveChanges();

            // Gửi email
            string link = Url.Action("Otp", "Authentication", new { token = token }, Request.Url.Scheme);
            string body = $"Mã OTP của bạn là: {otp}\nNhấn vào liên kết sau để xác minh:\n{link}";

            MailHelper.SendEmail(user.email, "Xác minh tài khoản", body); // trong Helpers/MailHelper.cs

            return RedirectToAction("Otp", new { token = token });
        }

        [HttpGet]
        public ActionResult Otp(string token)
        {
            var user = db.nguoidungs.SingleOrDefault(x => x.token == token);
            if (user == null || user.token_expire < DateTime.Now)
            {
                TempData["Error"] = "Liên kết đã hết hạn. Vui lòng thử lại.";
                return RedirectToAction("Recover");
            }

            ViewBag.Token = token;
            return View();
        }

        [HttpPost]
        public ActionResult Otp(string token, string otp)
        {
            var user = db.nguoidungs.SingleOrDefault(x => x.token == token);

            if (user != null && user.otp == otp && user.token_expire >= DateTime.Now)
            {
                return RedirectToAction("ResetPassword", new { token = token });
            }

            ViewBag.Token = token;
            ViewBag.Error = "Mã OTP không hợp lệ hoặc đã hết hạn.";
            return View();
        }

        [HttpGet]
        public ActionResult ResetPassword(string token)
        {
            var user = db.nguoidungs.FirstOrDefault(x => x.token == token);
            if (user == null || user.token_expire < DateTime.Now)
            {
                TempData["Error"] = "Liên kết đã hết hạn hoặc không hợp lệ. Vui lòng thử lại.";
                return RedirectToAction("Recover");
            }

            ViewBag.Token = token;
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(string token, string password, string confirm)
        {
            if (string.IsNullOrWhiteSpace(password) || password != confirm)
            {
                ViewBag.Error = "Mật khẩu không khớp hoặc rỗng.";
                ViewBag.Token = token;
                return View();
            }

            var user = db.nguoidungs.FirstOrDefault(x => x.token == token);
            if (user == null || user.token_expire < DateTime.Now)
            {
                TempData["Error"] = "Liên kết không hợp lệ hoặc đã hết hạn.";
                return RedirectToAction("Recover");
            }

            // Cập nhật mật khẩu (nên mã hoá ở đây nếu dùng thật)
            user.matkhau = password;
            user.token = null;
            user.otp = null;
            user.token_expire = null;
            db.SaveChanges();

            TempData["Success"] = "Đặt lại mật khẩu thành công. Vui lòng đăng nhập.";
            return RedirectToAction("Login");
        }





    }


}