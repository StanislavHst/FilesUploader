using Azure.Storage.Blobs;

namespace FilesUploader.Service;

public class FileService : IFileService
{
    private readonly IConfiguration _configuration;

    public FileService
    (
        IConfiguration configuration
    )
    {
        _configuration = configuration;
    }

    public async Task UploadFileAsync(Stream fileStream, string fileName, string email)
    {
        string blobStorageConnection = _configuration.GetConnectionString("BlobStorageConnection");

        BlobServiceClient blobServiceClient = new BlobServiceClient(blobStorageConnection);

        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("filedoc");

        if (!await containerClient.ExistsAsync())
        {
            await containerClient.CreateAsync();
        }

        BlobClient blobClient = containerClient.GetBlobClient($"{email}/{fileName}");

        await blobClient.UploadAsync(fileStream, true);

    }
}