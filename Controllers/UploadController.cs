using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TakeHomeChallenge.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadController : ControllerBase
    {
        private readonly ILogger<UploadController> _logger;
        private readonly string _videoPath;
        private const long MaxUploadSize = 200 * 1024 * 1024; //200MB

        public UploadController(ILogger<UploadController> logger, IWebHostEnvironment videoPath)
        {
            _logger = logger;
            _videoPath = Path.Combine(videoPath.WebRootPath, "media");
        }

        [HttpPost]
        [RequestSizeLimit(MaxUploadSize)]
        public async Task<IActionResult> UploadVideo(List<IFormFile> videoFiles)
        {
            if (videoFiles == null || videoFiles.Count == 0)
            {
                _logger.LogWarning("No video files found. Upload cannot proceed.");
                return BadRequest("The requested files could not be found.");
            }

            foreach (var video in videoFiles)
            {
                var fileName = Path.GetFileName(video.FileName);

                if (!fileName.EndsWith(".mp4", StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogWarning("File {FileName} rejected: unsupported format.", fileName);
                    return BadRequest("Only MP4 format files are supported.");
                }

                var filePath = Path.Combine(_videoPath, fileName);
                await using var stream = new FileStream(filePath, FileMode.Create);
                await video.CopyToAsync(stream);
            }

            _logger.LogInformation("Upload Completed! All files uploaded successfully.");
            return Ok("Your files were uploaded successfully.");
        }
    }
}
