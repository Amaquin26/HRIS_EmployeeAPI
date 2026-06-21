using HRIS_Employee.API.External.BlobStorage;
using Microsoft.AspNetCore.Mvc;

namespace HRIS_Employee.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlobStorageController(IBlobStorageService blobStorageService) : ControllerBase
    {
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file provided.");

            var url = await blobStorageService.UploadFileAsync(file);

            return Ok(new
            {
                message = "File uploaded successfully.",
                fileName = file.FileName,
                url
            });
        }

        [HttpGet("download/{fileName}")]
        public async Task<IActionResult> DownloadFile(string fileName)
        {
            var result = await blobStorageService.DownloadFileAsync(fileName);

            if (result == null)
                return NotFound($"File '{fileName}' not found.");

            var (content, contentType, name) = result.Value;
            return File(content, contentType, name);
        }

        [HttpGet("sas-url/{fileName}")]
        public IActionResult GetSasUrl(string fileName, [FromQuery] int expiryMinutes = 60)
        {
            var url = blobStorageService.GenerateSasUrl(fileName, expiryMinutes);
            return Ok(new { url });
        }
    }
}
