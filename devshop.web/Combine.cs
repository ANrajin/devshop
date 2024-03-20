using Microsoft.AspNetCore.Hosting;

namespace devshop.web
{
    public static class Combine
    {
        public static void Chunk(IWebHostEnvironment webHostEnvironment)
        {
            var finalFilePath = Path.Combine(webHostEnvironment.WebRootPath, "final");

            var tmpPath = Path.Combine(webHostEnvironment.WebRootPath, "tmp");

            var files = Directory.GetFiles(tmpPath);
        }
    }
}
