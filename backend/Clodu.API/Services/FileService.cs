using Clodu.API.Services;
using Clodu.API.Data.Repositories;
using Clodu.API.Models;

namespace Clodu.API.Services;

public class FileService : IFileService
{
    private readonly IFileRepository _fileRepository;
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<FileService> _logger;

    public FileService(IFileRepository fileRepository, IWebHostEnvironment environment, ILogger<FileService> logger)
    {
        _fileRepository = fileRepository;
        _environment = environment;
        _logger = logger;
    }

    public async Task<FileData> UploadFileAsync(Stream fileStream, string fileName, string contentType, int userId)
{
    using var ms = new MemoryStream();
    await fileStream.CopyToAsync(ms);
    var bytes = ms.ToArray();

    // 1. Сохраняем метаданные в БД (получаем Id)
    var fileData = new FileData
    {
        Name = fileName,
        Type = contentType,
        SizeBytes = bytes.Length,
        Hash = Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(bytes)),
        UserId = userId,
        AddedAt = DateTime.UtcNow,
        TotalShards = 1,
        DataShards = 1,
        ShardLocations = "[]"
    };

    var saved = await _fileRepository.CreateAsync(fileData);
    
    // 2. Формируем имя файла: userId_fileId_0.bin
    var storageDir = Path.Combine(_environment.ContentRootPath, "Storage");
    Directory.CreateDirectory(storageDir);
    
    var physicalPath = Path.Combine(storageDir, $"{userId}_{saved.Id}_0.bin");
    await File.WriteAllBytesAsync(physicalPath, bytes);
    
    _logger.LogInformation("File {FileName} saved to {Path}", fileName, physicalPath);
    
    // 3. Обновляем ShardLocations
    saved.ShardLocationsList = new List<ShardLocation>
    {
        new ShardLocation 
        { 
            Index = 0, 
            Path = physicalPath, 
            SizeBytes = bytes.Length,
            IsParity = false
        }
    };
    await _fileRepository.UpdateAsync(saved);
    
    return saved;
}

public async Task<bool> DeleteFileAsync(int fileId, int userId)
{
    var file = await _fileRepository.GetByIdAsync(fileId);
    if (file == null || file.UserId != userId) return false;

    // Удаляем все шарды
    foreach (var shard in file.ShardLocationsList)
    {
        if (File.Exists(shard.Path))
            File.Delete(shard.Path);
    }

    return await _fileRepository.SoftDeleteAsync(fileId);
}

    public async Task<FileData?> GetFileAsync(int fileId, int userId)
    {
        var file = await _fileRepository.GetByIdAsync(fileId);
        if (file == null || file.UserId != userId) return null;
        return file;
    }

    public async Task<List<FileData>> GetUserFilesAsync(int userId)
    {
        return await _fileRepository.GetByUserIdAsync(userId);
    }
}