using System.Net;
using System.Net.Mail;
namespace PBL.Helpers
{
    public class MailHelper
    {
        public static void SendEmail(string toEmail, string subject, string body)
        {
            string fromEmail = "hoangfdf8212005@gmail.com"; // Thay bằng Gmail của bạn
            string password = "ssbw yqwv rzzt tmng"; // Mật khẩu ứng dụng (App Password)

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail, password)
            };

            using (MailMessage message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            {
                smtp.Send(message);
            }
        }
    }
}