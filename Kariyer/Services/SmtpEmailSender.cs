using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace Kariyer.Services
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly IConfiguration config;

        public SmtpEmailSender(IConfiguration _config)
        {
            config = _config;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            // SMTP ayarlarını al
            var host = config["EmailSettings:Host"];
            var portStr = config["EmailSettings:Port"];
            var fromAddress = config["EmailSettings:From"];
            if (string.IsNullOrEmpty(host))
            {
                throw new InvalidOperationException("1.");
            }
            if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(fromAddress) || !int.TryParse(portStr, out var port))
            {
                throw new InvalidOperationException("SMTP ayarları eksik veya hatalı.");
            }

            var client = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(config["EmailSettings:Username"], config["EmailSettings:Password"]),
                EnableSsl = true,
                Timeout = 20000 // Zaman aşımı süresi
            };

            try
            {
                await client.SendMailAsync(new MailMessage(fromAddress, email, subject, message) { IsBodyHtml = true });
            }
            catch (SmtpException ex)
            {
                // Hata durumunda loglama veya başka işlem
                throw new InvalidOperationException("E-posta gönderim hatası: " + ex.ToString());
            }
        }

    }
}
