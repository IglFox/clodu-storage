using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Clodu.API.Services;

namespace Clodu.API.Controllers;

[ApiController]
[Route("api/test/crypto")]
[AllowAnonymous] // Временный доступ для теста
public class TestCryptoController : ControllerBase
{
    private readonly ICryptoService _crypto;
    private readonly ILogger<TestCryptoController> _logger;

    public TestCryptoController(ICryptoService crypto, ILogger<TestCryptoController> logger)
    {
        _crypto = crypto;
        _logger = logger;
    }

    /// <summary>
    /// Тест: шифрование и расшифровка текста
    /// </summary>
    [HttpPost("encrypt-decrypt")]
    public IActionResult EncryptDecrypt([FromBody] string plainText)
    {
        try
        {
            var data = System.Text.Encoding.UTF8.GetBytes(plainText);
            
            // 1. Генерируем ключ
            var (key, iv) = _crypto.GenerateAesKey();
            
            // 2. Шифруем
            var encrypted = _crypto.EncryptData(data, key, iv);
            var encryptedBase64 = Convert.ToBase64String(encrypted);
            
            // 3. Расшифровываем
            var decrypted = _crypto.DecryptData(encrypted, key, iv);
            var decryptedText = System.Text.Encoding.UTF8.GetString(decrypted);
            
            return Ok(new
            {
                original = plainText,
                encryptedBase64,
                decrypted = decryptedText,
                success = plainText == decryptedText,
                keyBase64 = Convert.ToBase64String(key),
                ivBase64 = Convert.ToBase64String(iv)
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Crypto test failed");
            return StatusCode(500, new { error = ex.Message });
        }
    }

    /// <summary>
    /// Тест: шифрование ключа мастер-ключом
    /// </summary>
    [HttpPost("key-wrapping")]
    public IActionResult KeyWrapping()
    {
        try
        {
            // 1. Генерируем случайный ключ файла
            var (fileKey, iv) = _crypto.GenerateAesKey();
            
            // 2. Шифруем его мастер-ключом
            var wrappedKey = _crypto.EncryptKeyWithMaster(fileKey);
            
            // 3. Расшифровываем обратно
            var unwrappedKey = _crypto.DecryptKeyWithMaster(wrappedKey);
            
            // 4. Проверяем, что ключи совпадают
            var success = Convert.ToBase64String(fileKey) == Convert.ToBase64String(unwrappedKey);
            
            return Ok(new
            {
                success,
                originalKeyBase64 = Convert.ToBase64String(fileKey),
                wrappedKeyBase64 = Convert.ToBase64String(wrappedKey),
                unwrappedKeyBase64 = Convert.ToBase64String(unwrappedKey)
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}