using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using EmailSenderMvc.Models;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
namespace EmailSenderMvc.Controllers
{
    public class MailController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

         

        public IActionResult SendEmail()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SendEmail(EmailData emailData)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(emailData.From));
            email.To.Add(MailboxAddress.Parse(emailData.To));
            email.Subject = emailData.Subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = emailData.Body };

            using var smtp = new SmtpClient();

            // Sertifika doğrulamasını devre dışı bırakıyoruz
            smtp.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;

            smtp.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(emailData.From, emailData.Password);
            smtp.Send(email);
            smtp.Disconnect(true);
            TempData["SuccessMessage"] = "E-posta başarıyla gönderildi.";
            return RedirectToAction("SendEmail");
        }

        public IActionResult SentEmail()
        {
            ViewData["Success"] = "Email has been sent successfully!";
            return View();
        }
    }

}
 