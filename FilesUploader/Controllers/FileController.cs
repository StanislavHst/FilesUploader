using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;

namespace FilesUploader.Controllers
{
    [Route("api/filecontroller")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public FileController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile()
        {
            try
            {
                var file = Request.Form.Files[0]; 
                var email = Request.Form["email"]; 

                if (file == null || file.Length == 0)
                {
                    return BadRequest("The file was not successfully uploaded.");
                }
                
                string blobStorageConnection = _configuration.GetConnectionString("BlobStorageConnection");

                BlobServiceClient blobServiceClient = new BlobServiceClient(blobStorageConnection);

                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("filedoc");

                if (!await containerClient.ExistsAsync())
                {
                    await containerClient.CreateAsync();
                }
                BlobClient blobClient = containerClient.GetBlobClient($"{email}/{file.FileName}");
                using (var stream = file.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, true);
                }
                
                await SendEmail(email, "Ваш файл успішно завантажено.");

                return Ok("The file has been successfully uploaded, and the message has been sent to the email.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Помилка сервера: {ex.Message}");
            }
        }
        
        [HttpPost("sendemail")]
        public async Task<IActionResult> SendEmail(string email, string message)
        {
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("stasic.rotar@gmail.com", "xnmm ufhx ypin wnpm"), // Використовуйте унікальний пароль додатка
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

                return Ok("Message successfully sent to your email");
            }
            catch (Exception ex)
            {
                return StatusCode(501, $"Email sending error: {ex.Message}");
            }
        }
    }
}