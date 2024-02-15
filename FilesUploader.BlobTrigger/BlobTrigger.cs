using System.IO;
using System.Threading.Tasks;
using FilesUploader.BlobTrigger.Service;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace FilesUploader.BlobTrigger;
[StorageAccount("BlobStorageConnection")]
public static class BlobTrigger
{
    [FunctionName("BlobTrigger")]
    public static async Task RunAsync([BlobTrigger("filedoc/{userEmail}/{file}")] Stream myBlob,string userEmail, ILogger log)
    {
        var emailService = new EmailService();
        await emailService.SendEmailAsync(userEmail, "Your file is successfully uploaded.","Your file has been successfully uploaded.");
    }
}