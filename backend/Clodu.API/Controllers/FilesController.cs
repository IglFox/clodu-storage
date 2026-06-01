using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Clodu.API.Services;

namespace Clodu.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FilesController : ControllerBase
{
    private readonly IFileService _fileService;

    public FilesController(IFileService fileService)
    {
        _fileService = fileService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        var userId = GetCurrentUserId();

        if (file == null || file.Length == 0)
            return BadRequest("No file provided");

        var result = await _fileService.UploadFileAsync(
            file.OpenReadStream(),
            file.FileName,
            file.ContentType,
            userId);

        return Ok(new
        {
            fileId = result.Id,
            name = result.Name,
            size = result.SizeBytes,
            uploadedAt = result.AddedAt
        });
    }

[HttpGet("{id}")]
public async Task<IActionResult> Download(int id)
{
    var userId = GetCurrentUserId();
    
    try
    {
        var encryptedBytes = await _fileService.DownloadFileAsync(id, userId);
        var file = await _fileService.GetFileAsync(id, userId);
        
        if (file == null)
            return NotFound();
        
        return File(encryptedBytes, "application/octet-stream", $"encrypted_{file.Name}");
    }
    catch (FileNotFoundException)
    {
        return NotFound();
    }
}

    [HttpGet]
    public async Task<IActionResult> GetUserFiles()
    {
        var userId = GetCurrentUserId();
        var files = await _fileService.GetUserFilesAsync(userId);
        return Ok(files.Select(f => new { f.Id, f.Name, f.SizeBytes, f.AddedAt }));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = GetCurrentUserId();
        var deleted = await _fileService.DeleteFileAsync(id, userId);
        return deleted ? Ok() : NotFound();
    }

    private int GetCurrentUserId()
    {
        var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(claim))
            throw new UnauthorizedAccessException();
        return int.Parse(claim);
    }
    
}