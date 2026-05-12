using Microsoft.AspNetCore.Mvc;

namespace Clodu.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FilesController : ControllerBase
{
    private readonly ILogger<FilesController> _logger;

    public FilesController(ILogger<FilesController> logger)
    {
        _logger = logger;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file");

        _logger.LogInformation("Uploading {FileName}, size: {Size}", file.FileName, file.Length);

        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        var hash = Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(ms.ToArray()));

        return Ok(new 
        { 
            fileName = file.FileName, 
            size = file.Length,
            hash = hash,
            message = "File received!"
        });
    }

    [HttpGet("test")]
    public IActionResult Test()
    {
        return Ok(new { message = "API is working!" });
    }
}