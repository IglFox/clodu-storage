using Microsoft.AspNetCore.Mvc;

namespace Clodu.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { status = "alive", timestamp = DateTime.UtcNow });
    }
}