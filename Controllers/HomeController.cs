using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TakeHomeChallenge.Models;

namespace TakeHomeChallenge.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly string _videoPath;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment videoPath)
        {
            _logger = logger;
            _videoPath = Path.Combine(videoPath.WebRootPath, "media");
        }

        public IActionResult Index()
        {
            try
            {
                var videos = Directory.GetFiles(_videoPath, "*.mp4").Select(file => new VideoFile
                {
                    FileName = Path.GetFileName(file),
                    FileSize = new FileInfo(file).Length
                }).ToList();

                _logger.LogInformation("Successfully loaded all videos from path: {Path}", _videoPath);
                return View(videos);
            }
            catch (Exception ex)
            {
                {
                    _logger.LogError(ex, "An unexpected error occurred while loading videos.");
                    return View("Error", "An unexpected error occurred.");
                }
            }
        }
    }
}
