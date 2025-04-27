using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
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
                Body = htmlMessage,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(email); 
            await client.SendMailAsync(mailMessage);
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