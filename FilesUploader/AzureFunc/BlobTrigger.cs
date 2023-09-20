using System.Net;
using Microsoft.Azure.WebJobs;
using System.Net.Mail;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using System.IO;
using Microsoft.Extensions.Logging;
using System;


[StorageAccount("BlobConnectionString")]
public static class BlobTriggerFunction
{
    [FunctionName("BlobStorageTriggerss")]
    public static void Run(
        [BlobTrigger("filedoc/{email}/{fileName}")] Stream blobStream,
        string fileName,
        string email,
        ILogger log)
    {
        try
        {
            var container = GetBlobContainerClient();
            var blob = container.GetBlobClient($"{email}/{fileName}");
            var sasToken = GetSasToken(blob, TimeSpan.FromHours(1));

            SendEmail(email, $"Your file is successfully uploaded. Secure URL: {blob.Uri + sasToken}", log);
        }
        catch (Exception ex)
        {
            log.LogError($"Error processing blob: {ex.Message}");
        }
    }

    private static BlobContainerClient GetBlobContainerClient()
    {
        string blobStorageConnection = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
        BlobServiceClient blobServiceClient = new BlobServiceClient(blobStorageConnection);
        return blobServiceClient.GetBlobContainerClient("filedoc");
    }

    private static string GetSasToken(BlobClient blob, TimeSpan expiration)
    {
        var blobSasBuilder = new BlobSasBuilder
        {
            BlobContainerName = blob.BlobContainerName,
            BlobName = blob.Name,
            Resource = "b",
            ExpiresOn = DateTimeOffset.UtcNow.Add(expiration),
        };

        blobSasBuilder.SetPermissions(BlobSasPermissions.Read);
        return blob.GenerateSasUri(blobSasBuilder).Query;
    }

    private static void SendEmail(string email, string message, ILogger log)
    {

        try
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("stasic.rotar@gmail.com", "password"), 
                EnableSsl = true,
            };
        
            var mailMessage = new MailMessage
            {
                From = new MailAddress("stasic.rotar@gmail.com"),
                Subject = "Файл успішно завантажено",
                Body = message
            };

            mailMessage.To.Add(email);

             smtpClient.SendMailAsync(mailMessage);
        }
        catch (Exception ex)
        {
            log.LogError($"Error sending email: {ex.Message}");
        }
    }
}
