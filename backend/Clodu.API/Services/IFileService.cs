// Services/IFileService.cs
using Clodu.API.Models;

namespace Clodu.API.Services;  // 👈 namespace должен быть такой же

public interface IFileService
{
    Task<FileData> UploadFileAsync(Stream fileStream, string fileName, string contentType, int userId);
    Task<FileData?> GetFileAsync(int fileId, int userId);
    Task<bool> DeleteFileAsync(int fileId, int userId);
    Task<List<FileData>> GetUserFilesAsync(int userId);
}