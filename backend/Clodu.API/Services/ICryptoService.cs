using Clodu.API.Models;

namespace Clodu.API.Services;

public interface ICryptoService
{
    Task<(byte[] EncryptedData, EncryptionKey Key)> EncryptForOfficeAsync(byte[] data, int fileId);
    Task<byte[]> DecryptForOfficeAsync(byte[] encryptedData, int fileId);
    (byte[] Key, byte[] Iv) GenerateAesKey();
}