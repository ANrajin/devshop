using devshop.api.Cores.Utilities.FileHandler;
using Microsoft.AspNetCore.Http.HttpResults;

namespace devshop.api.Features.Courses
{
    public static class CoursesEndPoint
    {
        public static RouteGroupBuilder MapCoursesEndPoint(this RouteGroupBuilder builder)
        {
            Upload(builder);
            UploadChunk(builder);

            return builder;
        }

        private static void Upload(RouteGroupBuilder builder)
        {
            builder.MapPost("/upload", Results<Ok<string>, ProblemHttpResult>
                (IFileHandlerService fileHandlerService) =>
            {
                var directoryName = fileHandlerService.PrepareTmpDir();
                
                return TypedResults.Ok(directoryName);
            });
        }

        private static void UploadChunk(RouteGroupBuilder builder)
        {
            builder.MapPatch("/upload", async Task<Results<NoContent, ProblemHttpResult>> 
                (string patch, 
                HttpContext context,
                IFileHandlerService fileHandlerService) =>
            {
                // Read the request body as a byte array
                using var ms = new MemoryStream();

                await context.Request.Body.CopyToAsync(ms);

                var fileUploadRequest = new FileUploadRequest(
                    context.Request.Headers["Upload-Name"]!,
                    patch.Trim('"'),
                    Convert.ToInt32(context.Request.Headers["Upload-Length"]),
                    Convert.ToInt32(context.Request.Headers["Content-Length"]!),
                    Convert.ToInt32(context.Request.Headers["Upload-Offset"]!),
                    ms.ToArray());

                await fileHandlerService.UploadChunkAsync(fileUploadRequest);

                return TypedResults.NoContent();
            });
        }
    }
}
