using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace FilesUploader.BlobTrigger.Service;

public class EmailService : IEmailService
{
    public async Task SendEmailAsync(string email, string subject, string message)
    {
        try
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("stasic.rotar@gmail.com", "xnmm ufhx ypin wnpm"),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("stasic.rotar@gmail.com"),
                Subject = subject,
                Body = message
            };

            mailMessage.To.Add(email);

            await smtpClient.SendMailAsync(mailMessage);
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to send email: {ex.Message}");
        }
    }
}