namespace devshop.api.Cores.Utilities.FileHandler;

public class FileHandlerService(IWebHostEnvironment webHostEnvironment) : IFileHandlerService
{
    private readonly string _videoContentDir = Path.Combine(webHostEnvironment.ContentRootPath, "assets/videos");
    private readonly string _tmpFilesDir = Path.Combine(webHostEnvironment.ContentRootPath, "assets/tmp");

    public string PrepareTmpDir()
    {
        var uniqueDir = string.Concat(Guid.NewGuid().ToString().Split('-'));

        var tmpFilePath = Path.Combine(_tmpFilesDir, uniqueDir);

        if (!Directory.Exists(tmpFilePath))
        {
            Directory.CreateDirectory(tmpFilePath);
        }

        return uniqueDir;
    }

    public async Task UploadChunkAsync(FileUploadRequest request)
    {
        var chunkIndex = 1;
        var totalChunk = request.UploadedFileOffset + request.UploadedContentLength;
        var tmpFilePath = Path.Combine(_tmpFilesDir, request.DirectoryName);

        if (request.UploadedFileOffset > 0)
        {
            chunkIndex = (request.UploadedFileOffset / request.UploadedContentLength) + 1;
        }

        await File.WriteAllBytesAsync(Path.Combine(tmpFilePath, $"{request.FileName}{chunkIndex}"), 
            request.UploadedFileData);

        if (totalChunk.Equals(request.UploadedFileLength))
        {
            var destPath = Path.Combine(_videoContentDir, request.FileName);

            var filePaths = Directory.GetFiles(tmpFilePath)
                .OrderBy(p => int.Parse(p.Replace(request.FileName, "$")
                .Split('$')[1])).ToArray();

            using var finalFileStream = new FileStream(destPath, FileMode.Create);

            foreach (var chunkFile in filePaths)
            {
                byte[] chunkDatas = await File.ReadAllBytesAsync(chunkFile);

                await finalFileStream.WriteAsync(chunkDatas);
            }

            Directory.Delete(tmpFilePath, true);
        }
    }
}
