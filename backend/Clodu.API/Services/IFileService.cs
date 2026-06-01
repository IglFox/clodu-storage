using Clodu.API.Models;

namespace Clodu.API.Services;

public interface IFileService
{
    Task<FileData> UploadFileAsync(Stream fileStream, string fileName, string contentType, int userId);
    Task<byte[]> DownloadFileAsync(int fileId, int userId);  // 👈 Новый метод
    Task<bool> DeleteFileAsync(int fileId, int userId);
    Task<FileData?> GetFileAsync(int fileId, int userId);
    Task<List<FileData>> GetUserFilesAsync(int userId);
}