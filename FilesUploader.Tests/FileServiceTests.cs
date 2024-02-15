using System.Text;
using Azure.Storage.Blobs;
using FilesUploader.Service;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace FilesUploader.Tests
{
    public class FileUploadServiceTests
    {
        [Fact]
        public async Task UploadFileAsync_ShouldUploadFileToBlobStorage()
        {
            var fileContent = "Hello, world!";
            var fileName = "testsss_file.txt";
            var email = "example@email.com";

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new[]
                {
                    new KeyValuePair<string, string>("ConnectionStrings:BlobStorageConnection", "DefaultEndpointsProtocol=https;AccountName=blobcontainerstas;AccountKey=VtHUB2MMh+fP+5h0/1b0jyOALDUyIFczqLRX5uUUcWM4OuekAWGeSvS7i4whKCnrDMKgAN4c2Gf8+AStZ4te0Q==;EndpointSuffix=core.windows.net")
                })
                .Build();

            var fileUploadService = new FileService(configuration);

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(fileContent)))
            {
                await fileUploadService.UploadFileAsync(stream, fileName, email);
            }
            
            var blobServiceClient = new BlobServiceClient(configuration.GetConnectionString("BlobStorageConnection"));
            var containerClient = blobServiceClient.GetBlobContainerClient("filedoc");
            var blobClient = containerClient.GetBlobClient($"{email}/{fileName}");
            
            Assert.True(await blobClient.ExistsAsync());

        }
    }
}