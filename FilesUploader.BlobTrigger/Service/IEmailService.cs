using System.Threading.Tasks;

namespace FilesUploader.BlobTrigger.Service;

public interface IEmailService
{
    Task SendEmailAsync(string email,string subjectMessage, string message);
}