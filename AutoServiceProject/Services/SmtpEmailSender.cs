using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace AutoServiceProject.Services
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly SmtpSettings _smtpSettings;

        public SmtpEmailSender(IOptions<SmtpSettings> options)
        {
            _smtpSettings = options.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient
            {
                Host = _smtpSettings.Host,
                Port = _smtpSettings.Port,
                EnableSsl = _smtpSettings.EnableSsl,
                Credentials = new NetworkCredential(_smtpSettings.UserName, _smtpSettings.Password)
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpSettings.UserName),
                Subject = subject,
                Body = WrapInTemplate(htmlMessage),
                IsBodyHtml = true,
            };

            mailMessage.To.Add(email);
            await client.SendMailAsync(mailMessage);
        }

        private string WrapInTemplate(string innerHtml)
        {
            return $@"
<div style='max-width:600px;margin:auto;font-family:Arial,sans-serif;background:#f9f9f9;border:1px solid #ddd;border-radius:8px;overflow:hidden;'>
    <div style='background:#000;color:#fff;padding:20px;text-align:center;'>
        <h2>Welcome to AutoService</h2>
    </div>
    <div style='padding:20px;'>
        {innerHtml}
        <p style='margin-top:40px;'>— AutoService Team</p>
    </div>
</div>";
        }
    }

    public class SmtpSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
