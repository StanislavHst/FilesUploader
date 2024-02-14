using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace FilesUploader.BlobTrigger;
[StorageAccount("BlobStorageConnection")]
public static class BlobTriggerStas
{
    [FunctionName("BlobTriggerStas")]
    public static async Task RunAsync([BlobTrigger("filedoc/{userEmail}")] Stream myBlob,
        string userEmail, ILogger log)
    {
        log.LogInformation($"C# Blob trigger function Processed blob\n Name:{userEmail} \n Size: {myBlob.Length} Bytes");
        
        SendEmail(userEmail, "Your file is successfully uploaded.");
    }
    
    public static async Task SendEmail(string email, string message)
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
                Subject = "File successfully uploaded",
                Body = message
            };

            mailMessage.To.Add(email);

            await smtpClient.SendMailAsync(mailMessage);

        }
        catch (Exception ex)
        {
        }
    }
}