namespace Infrastructure.FileStorage
{
    public interface IFileStorageClient
    {
        Task<bool> ExistsFileAsync(string path);
        Task UploadFileAsync(string path, Stream content, bool overwrite = false, bool createDirectoryIfNotExists = false);
        Task<Stream> DownloadFileAsync(string path);
        Task DeleteFileAsync(string path);
        Task RenameFileAsync(string path, string targetPath);
        Task<bool> ExistsDirectoryAsync(string path);
        Task CreateDirectoryAsync(string path);
        Task DeleteDirectoryAsync(string path, bool recursive);
        Task RenameDirectoryAsync(string path, string targetPath);
        Task<IEnumerable<PathItem>> DirectoryGetltemsAsync(string path);
        Task UploadZipFileAsync(string path, Stream content, bool overwrite = false, bool createDirectoryIfNotExists = false);
        Task<Stream> DownloadZipFileAsync(string path);
        string GetExternalFilePath(string path);
        Task CopyDirectory(string sourcePath, string goalPath, bool overwrite = true);
    }
}
