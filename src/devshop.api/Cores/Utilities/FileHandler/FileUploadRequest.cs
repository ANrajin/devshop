namespace devshop.api.Cores.Utilities.FileHandler
{
    public sealed record FileUploadRequest(
        string FileName,
        string DirectoryName,
        int UploadedFileLength,
        int UploadedContentLength,
        int UploadedFileOffset,
        byte[] UploadedFileData);
}
