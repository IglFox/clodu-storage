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
        ICryptoService crypto)  // 👈 Добавили зависимость
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
        
        // 4. Шифруем ключ файла мастер-ключом (для хранения в БД)
        var encryptedKey = _crypto.EncryptKeyWithMaster(fileKey);
        
        // 5. Создаём метаданные
        var fileData = new FileData
        {
            Name = fileName,
            Type = contentType,
            SizeBytes = originalBytes.Length,  // Сохраняем исходный размер
            Hash = Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(originalBytes)),
            UserId = userId,
            AddedAt = DateTime.UtcNow,
            TotalShards = 1,
            DataShards = 1,
            ShardLocations = "[]"
        };

        // 6. Сохраняем метаданные в БД
        var saved = await _fileRepository.CreateAsync(fileData);
        
        // 7. Сохраняем зашифрованный файл на диск
        var storageDir = Path.Combine(_environment.ContentRootPath, "Storage");
        Directory.CreateDirectory(storageDir);
        
        var physicalPath = Path.Combine(storageDir, $"{userId}_{saved.Id}.enc");
        await File.WriteAllBytesAsync(physicalPath, encryptedBytes);
        
        // 8. Сохраняем ключ в таблицу file_keys
        var fileKeyEntity = new FileKey
        {
            FileId = saved.Id,
            EncryptedKey = encryptedKey,
            IV = iv,
            CreatedAt = DateTime.UtcNow
        };
        
        // Добавляем ключ через контекст (нужно получить доступ к DbSet)
        // Временно сохраним в ShardLocations, но лучше добавить репозиторий для ключей
        saved.ShardLocationsList = new List<ShardLocation>
        {
            new ShardLocation 
            { 
                Index = 0, 
                Path = physicalPath, 
                SizeBytes = encryptedBytes.Length,
                IsParity = false,
                // Временно храним IV и encryptedKey здесь (потом перенесём в отдельную таблицу)
            }
        };
        
        // TODO: Сохранить ключ в отдельную таблицу file_keys
        // _fileKeyRepository.CreateAsync(fileKeyEntity);
        
        await _fileRepository.UpdateAsync(saved);
        
        _logger.LogInformation("File uploaded and encrypted: {FileName}, UserId: {UserId}, FileId: {FileId}, OriginalSize: {Size}, EncryptedSize: {EncryptedSize}", 
            fileName, userId, saved.Id, originalBytes.Length, encryptedBytes.Length);
        
        return saved;
    }

    public async Task<byte[]> DownloadFileAsync(int fileId, int userId)
    {
        // 1. Получаем метаданные
        var file = await _fileRepository.GetByIdAsync(fileId);
        if (file == null || file.UserId != userId)
            throw new FileNotFoundException("File not found");
        
        // 2. Получаем путь к зашифрованному файлу
        var shardPath = file.ShardLocationsList.FirstOrDefault()?.Path;
        if (string.IsNullOrEmpty(shardPath) || !System.IO.File.Exists(shardPath))
            throw new FileNotFoundException("Physical file not found");
        
        // 3. Читаем зашифрованные байты с диска
        var encryptedBytes = await System.IO.File.ReadAllBytesAsync(shardPath);
        
        // 4. TODO: Получить ключ из таблицы file_keys
        // var fileKey = await _fileKeyRepository.GetByFileIdAsync(fileId);
        
        // Временно: используем заглушку (без реального ключа не расшифровать)
        // Пока вернём зашифрованные байты — позже добавим расшифровку
        _logger.LogWarning("Decryption not yet implemented for FileId: {FileId}", fileId);
        
        return encryptedBytes; // Временно возвращаем зашифрованные данные
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

        // TODO: Удалить ключ из таблицы file_keys
        // await _fileKeyRepository.DeleteByFileIdAsync(fileId);

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