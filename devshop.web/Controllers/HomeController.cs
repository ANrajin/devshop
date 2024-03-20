using devshop.web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Drawing;

namespace devshop.web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IWebHostEnvironment _webHostEnvironment;

        public HomeController(ILogger<HomeController> logger,
            IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            Combine.Chunk(_webHostEnvironment);

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult Upload()
        {
            return Ok(Guid.NewGuid());
        }

        [HttpPatch]
        public async Task<IActionResult> UploadAsync(string patch)
        {
            // Read the request body as a byte array
            using (var ms = new MemoryStream())
            {
                await Request.Body.CopyToAsync(ms);

                byte[] chunkData = ms.ToArray();

                // Example: use file name and unique identifier to track chunks
                string fileName = Request.Headers["Upload-Name"];
                var chunkIndex = DateTime.Now.Ticks;

                // Example: save chunk data to disk
                string chunkPath = Path.Combine(Path.Combine(_webHostEnvironment.WebRootPath, "tmp"), $"{fileName}_{chunkIndex}");
                await System.IO.File.WriteAllBytesAsync(chunkPath, chunkData);

                return Ok();
            }
        }
    }
}
