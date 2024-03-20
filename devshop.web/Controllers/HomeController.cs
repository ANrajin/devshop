using devshop.web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace devshop.web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
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
        public IActionResult Upload(string patch)
        {
            var header = HttpContext.Request.Headers;

            var request = HttpContext.Request;

            return Ok(Guid.NewGuid());
        }
    }
}
