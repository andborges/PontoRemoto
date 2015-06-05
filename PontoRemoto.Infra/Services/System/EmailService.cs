using Microsoft.AspNet.Identity;
using PontoRemoto.Application.Interfaces.Infrastructure.System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace PontoRemoto.Infra.Services.System
{
    public class EmailService : IEmailService
    {
        public async Task SendAsync(string to, string subject, string message)
        {
            using (var mailMessage = new MailMessage())
            {
                mailMessage.To.Add(to);
                mailMessage.Subject = subject;
                mailMessage.Body = message;

                using (var smtpClient = new SmtpClient())
                {
                    await smtpClient.SendMailAsync(mailMessage);
                }
            }
        }

        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }
    }
}