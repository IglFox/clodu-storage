// Clodu.API/Services/CryptoService.cs
using System.Security.Cryptography;
using Clodu.API.Models;
using Microsoft.Extensions.Options;

namespace Clodu.API.Services;

public class CryptoService : ICryptoService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<CryptoService> _logger;

    public CryptoService(IConfiguration configuration, ILogger<CryptoService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    // Вспомогательный метод: получает мастер-ключ из безопасного места
    private byte[] GetMasterKey()
    {
        // ВАЖНО: Для разработки можно временно захардкодить, 
        // но для продакшена используй переменные окружения или Azure Key Vault.
        // Пример из appsettings.json (небезопасно для прода):
        var masterKeyBase64 = _configuration["Encryption:MasterKey"];
        
        if (string.IsNullOrEmpty(masterKeyBase64))
        {
            // Генерируем фейковый ключ, если его нет. В реальном проекте лучше кидать ошибку.
            _logger.LogWarning("Мастер-ключ не найден! Использую временный фиксированный ключ для разработки.");
            return Convert.FromBase64String("bXlTdXBlclNlY3JldEtleVRoYXRJc1RlcnRpYmx5TmV3MTIzIQ==");
        }
        
        return Convert.FromBase64String(masterKeyBase64);
    }

    // 1. Генерация ключа для ФАЙЛА
    public (byte[] Key, byte[] Iv) GenerateAesKey()
    {
        using var aes = Aes.Create();
        aes.KeySize = 256;
        aes.GenerateKey();
        aes.GenerateIV();
        return (aes.Key, aes.IV);
    }

    // 2. Шифрование ДАННЫХ (самого файла) ключом файла
    public byte[] EncryptData(byte[] data, byte[] key, byte[] iv)
    {
        using var aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        using var memoryStream = new MemoryStream();
        using var cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write);
        
        cryptoStream.Write(data, 0, data.Length);
        cryptoStream.FlushFinalBlock();
        
        return memoryStream.ToArray();
    }

    // 3. Расшифровка ДАННЫХ (самого файла) ключом файла
    public byte[] DecryptData(byte[] encryptedData, byte[] key, byte[] iv)
    {
        using var aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        using var memoryStream = new MemoryStream(encryptedData);
        using var cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Read);
        using var resultStream = new MemoryStream();
        
        cryptoStream.CopyTo(resultStream);
        return resultStream.ToArray();
    }

    // 4. Шифрование КЛЮЧА ФАЙЛА с помощью МАСТЕР-КЛЮЧА (для хранения в БД)
    public byte[] EncryptKeyWithMaster(byte[] fileKey)
    {
        var masterKey = GetMasterKey();
        // Используем простой алгоритм: мастер-ключ шифрует ключ файла.
        // Заметка: На практике можно использовать Key Wrap алгоритм, но AES-CBC подойдет.
        using var aes = Aes.Create();
        aes.Key = masterKey;
        aes.GenerateIV(); // Генерируем случайный IV для этого конкретного ключа
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        // Мы должны сохранить IV вместе с зашифрованным ключом.
        // Поэтому мы просто конкатенируем IV + EncryptedKey
        using var encryptor = aes.CreateEncryptor();
        var encryptedKey = encryptor.TransformFinalBlock(fileKey, 0, fileKey.Length);
        
        var result = new byte[aes.IV.Length + encryptedKey.Length];
        Buffer.BlockCopy(aes.IV, 0, result, 0, aes.IV.Length);
        Buffer.BlockCopy(encryptedKey, 0, result, aes.IV.Length, encryptedKey.Length);
        
        return result;
    }

    // 5. Расшифровка КЛЮЧА ФАЙЛА с помощью МАСТЕР-КЛЮЧА (из БД)
    public byte[] DecryptKeyWithMaster(byte[] encryptedFileKeyWithIv)
    {
        var masterKey = GetMasterKey();
        
        using var aes = Aes.Create();
        aes.Key = masterKey;
        
        // Извлекаем IV (первые 16 байт)
        var iv = new byte[16];
        Buffer.BlockCopy(encryptedFileKeyWithIv, 0, iv, 0, iv.Length);
        aes.IV = iv;
        
        // Извлекаем зашифрованный ключ (остальные байты)
        var encryptedKey = new byte[encryptedFileKeyWithIv.Length - iv.Length];
        Buffer.BlockCopy(encryptedFileKeyWithIv, iv.Length, encryptedKey, 0, encryptedKey.Length);
        
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;
        
        using var decryptor = aes.CreateDecryptor();
        return decryptor.TransformFinalBlock(encryptedKey, 0, encryptedKey.Length);
    }

    // 6. Обертки для старой сигнатуры (если нужно)
    public async Task<(byte[] EncryptedData, byte[] Key, byte[] Iv)> EncryptForOfficeAsync(byte[] data)
    {
        var (key, iv) = GenerateAesKey();
        var encryptedData = EncryptData(data, key, iv);
        return await Task.FromResult((encryptedData, key, iv));
    }

    public async Task<byte[]> DecryptForOfficeAsync(byte[] encryptedData, byte[] key, byte[] iv)
    {
        return await Task.FromResult(DecryptData(encryptedData, key, iv));
    }
}