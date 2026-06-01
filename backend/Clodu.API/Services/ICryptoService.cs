// Clodu.API/Services/ICryptoService.cs
using Clodu.API.Models;

namespace Clodu.API.Services;

public interface ICryptoService
{
    // Генерирует случайную пару: ключ + вектор инициализации (IV)
    (byte[] Key, byte[] Iv) GenerateAesKey();

    // Шифрует данные (файл) с использованием переданных ключа и IV
    byte[] EncryptData(byte[] data, byte[] key, byte[] iv);

    // Расшифровывает данные (файл) с использованием переданных ключа и IV
    byte[] DecryptData(byte[] encryptedData, byte[] key, byte[] iv);

    // Шифрует сам ключ файла с помощью мастер-ключа (будет храниться в БД)
    byte[] EncryptKeyWithMaster(byte[] fileKey);

    // Расшифровывает ключ файла с помощью мастер-ключа
    byte[] DecryptKeyWithMaster(byte[] encryptedFileKey);

    // Обертка для работы с офисом (если нужна старая сигнатура)
    // Но лучше использовать новые отдельные методы
    // (Твоя старая сигнатура)
    Task<(byte[] EncryptedData, byte[] Key, byte[] Iv)> EncryptForOfficeAsync(byte[] data);
    Task<byte[]> DecryptForOfficeAsync(byte[] encryptedData, byte[] key, byte[] iv);
}