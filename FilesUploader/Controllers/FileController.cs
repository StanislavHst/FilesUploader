using FilesUploader.Service;
using Microsoft.AspNetCore.Mvc;

namespace FilesUploader.Controllers;

[Route("api/filecontroller")]
[ApiController]
public class FileController : ControllerBase
{
    private readonly IFileService _fileService;

    public FileController
    (
        IFileService fileService
    )
    {
        _fileService = fileService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile([FromForm] IFormFile file, [FromForm] string email)
    {
        if (file == null || file.Length == 0)
            return BadRequest("The file was not successfully uploaded.");

        if (string.IsNullOrEmpty(email))
            return BadRequest("Email address is required.");

        try
        {
            using (var fileStream = file.OpenReadStream())
            {
                await _fileService.UploadFileAsync(fileStream, file.FileName, email);

                return Ok("The file has been successfully uploaded");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Помилка сервера: {ex.Message}");
        }
    }
}