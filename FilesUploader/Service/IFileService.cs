namespace FilesUploader.Service;

public interface IFileService
{
    Task UploadFileAsync(Stream fileStream,string fileName, string email);
}