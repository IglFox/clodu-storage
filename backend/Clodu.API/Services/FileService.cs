using Clodu.API.Data.Repositories;
using Clodu.API.Models;
using Clodu.API.Services;

namespace Clodu.API.Services;

public class FileService : IFileService
{
    private readonly IFileRepository _fileRepository;
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<FileService> _logger;
    private readonly ICryptoService _crypto;

    public FileService(
        IFileRepository fileRepository, 
        IWebHostEnvironment environment, 
        ILogger<FileService> logger,
        ICryptoService crypto)
    {
        _fileRepository = fileRepository;
        _environment = environment;
        _logger = logger;
        _crypto = crypto;
    }

    public async Task<FileData> UploadFileAsync(Stream fileStream, string fileName, string contentType, int userId)
{
    // 1. Читаем исходные байты
    using var ms = new MemoryStream();
    await fileStream.CopyToAsync(ms);
    var originalBytes = ms.ToArray();
    
    // 2. Генерируем уникальный ключ для этого файла
    var (fileKey, iv) = _crypto.GenerateAesKey();
    
    // 3. Шифруем данные файла
    var encryptedBytes = _crypto.EncryptData(originalBytes, fileKey, iv);
    
    // 4. Создаём метаданные
    var fileData = new FileData
    {
        Name = fileName,
        Type = contentType,
        SizeBytes = originalBytes.Length,
        Hash = Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(originalBytes)),
        UserId = userId,
        AddedAt = DateTime.UtcNow,
        TotalShards = 1,
        DataShards = 1,
        ShardLocations = "[]"
    };

    // 5. Сохраняем метаданные в БД
    var saved = await _fileRepository.CreateAsync(fileData);
    
    // 6. Сохраняем зашифрованный файл на диск
    var storageDir = Path.Combine(_environment.ContentRootPath, "Storage");
    Directory.CreateDirectory(storageDir);
    
    var physicalPath = Path.Combine(storageDir, $"{userId}_{saved.Id}.enc");
    await System.IO.File.WriteAllBytesAsync(physicalPath, encryptedBytes);
    
    // 7. Шифруем ключ файла мастер-ключом
    var encryptedKey = _crypto.EncryptKeyWithMaster(fileKey);
    
    // 8. Сохраняем путь, зашифрованный ключ и IV в ShardLocations
    saved.ShardLocationsList = new List<ShardLocation>
    {
        new ShardLocation 
        { 
            Index = 0, 
            Path = physicalPath, 
            SizeBytes = encryptedBytes.Length,
            IsParity = false,
            EncryptedKey = Convert.ToBase64String(encryptedKey),
            IV = Convert.ToBase64String(iv)
        }
    };
    
    await _fileRepository.UpdateAsync(saved);
    
    _logger.LogInformation("File uploaded and encrypted: {FileName}, UserId: {UserId}, FileId: {FileId}", 
        fileName, userId, saved.Id);
    
    return saved;
}
    public async Task<byte[]> DownloadFileAsync(int fileId, int userId)
    {
        // 1. Получаем метаданные
        var file = await _fileRepository.GetByIdAsync(fileId);
        if (file == null || file.UserId != userId)
            throw new FileNotFoundException("File not found");
        
        // 2. Получаем путь к зашифрованному файлу
        var shard = file.ShardLocationsList.FirstOrDefault();
        if (shard == null || string.IsNullOrEmpty(shard.Path) || !System.IO.File.Exists(shard.Path))
            throw new FileNotFoundException("Physical file not found");
        
        // 3. Читаем зашифрованные байты с диска
        var encryptedBytes = await System.IO.File.ReadAllBytesAsync(shard.Path);
        
        // 4. Получаем зашифрованный ключ и IV из ShardLocations
        if (string.IsNullOrEmpty(shard.EncryptedKey) || string.IsNullOrEmpty(shard.IV))
            throw new InvalidOperationException("Encryption key not found for this file");
        
        var encryptedKey = Convert.FromBase64String(shard.EncryptedKey);
        var iv = Convert.FromBase64String(shard.IV);
        
        // 5. Расшифровываем ключ файла мастер-ключом
        var fileKey = _crypto.DecryptKeyWithMaster(encryptedKey);
        
        // 6. Расшифровываем данные файла
        var decryptedBytes = _crypto.DecryptData(encryptedBytes, fileKey, iv);
        
        _logger.LogInformation("File downloaded and decrypted: FileId: {FileId}, UserId: {UserId}, OriginalSize: {Size}", 
            fileId, userId, decryptedBytes.Length);
        
        return decryptedBytes;
    }

    public async Task<bool> DeleteFileAsync(int fileId, int userId)
    {
        var file = await _fileRepository.GetByIdAsync(fileId);
        if (file == null || file.UserId != userId) return false;

        // Удаляем физический файл
        foreach (var shard in file.ShardLocationsList)
        {
            if (System.IO.File.Exists(shard.Path))
                System.IO.File.Delete(shard.Path);
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