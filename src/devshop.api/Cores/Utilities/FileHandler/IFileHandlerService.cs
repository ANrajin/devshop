namespace devshop.api.Cores.Utilities.FileHandler;

public interface IFileHandlerService
{
    string PrepareTmpDir();

    Task UploadChunkAsync(FileUploadRequest fileUploadRequest);
}
