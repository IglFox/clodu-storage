using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Clodu.API.Data;
using Clodu.API.Models;
using Clodu.API.Data.Repositories;
using Clodu.API.Models.Requests;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        AppDbContext context,
        IConfiguration configuration,
        ILogger<AuthController> logger)
    {
        _context = context;
        _configuration = configuration;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        // LINQ проверка существующего пользователя
        var existingUser = await _context.Users
            .Where(u => u.Email == request.Email || u.Username == request.Username)
            .FirstOrDefaultAsync();
        
        if (existingUser != null)
        {
            if (existingUser.Email == request.Email)
                return BadRequest(new { message = "User with this email already exists" });
            if (existingUser.Username == request.Username)
                return BadRequest(new { message = "User with this username already exists" });
        }

        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var token = GenerateJwtToken(user);
        
        var session = new UserSession
        {
            UserId = user.Id,
            JwtToken = token,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
            UserAgent = Request.Headers["User-Agent"].ToString(),
            IsRevoked = false
        };
        
        _context.UserSessions.Add(session);
        await _context.SaveChangesAsync();

        // LINQ для получения количества сессий
        var sessionCount = await _context.UserSessions
            .Where(s => s.UserId == user.Id && !s.IsRevoked)
            .CountAsync();

        _logger.LogInformation("New user registered: {Email} (ID: {UserId}), active sessions: {SessionCount}", 
            user.Email, user.Id, sessionCount);

        return Ok(new 
        { 
            token = token,
            username = user.Username,
            email = user.Email,
            userId = user.Id,
            expiresAt = DateTime.UtcNow.AddDays(7)
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        // LINQ поиск пользователя
        var user = await _context.Users
            .Where(u => u.Email == request.Email && !u.IsDeleted)
            .FirstOrDefaultAsync();
        
        if (user == null)
        {
            _logger.LogWarning("Login failed: User not found {Email}", request.Email);
            return Unauthorized(new { message = "Invalid email or password" });
        }

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            _logger.LogWarning("Login failed: Invalid password for {Email}", request.Email);
            return Unauthorized(new { message = "Invalid email or password" });
        }

        var token = GenerateJwtToken(user);
        
        var session = new UserSession
        {
            UserId = user.Id,
            JwtToken = token,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
            UserAgent = Request.Headers["User-Agent"].ToString(),
            IsRevoked = false
        };
        
        _context.UserSessions.Add(session);
        
        // LINQ: оставляем только последние 5 сессий, остальные деактивируем
        var oldSessions = await _context.UserSessions
            .Where(s => s.UserId == user.Id && !s.IsRevoked)
            .OrderByDescending(s => s.CreatedAt)
            .Skip(5)
            .ToListAsync();
        
        foreach (var oldSession in oldSessions)
        {
            oldSession.IsRevoked = true;
        }
        
        await _context.SaveChangesAsync();

        _logger.LogInformation("User logged in: {Email} (ID: {UserId})", user.Email, user.Id);

        return Ok(new 
        { 
            token = token,
            username = user.Username,
            email = user.Email,
            userId = user.Id,
            expiresAt = DateTime.UtcNow.AddDays(7)
        });
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        var userId = GetCurrentUserId();
        var token = GetCurrentToken();
        
        // LINQ поиск активной сессии
        var session = await _context.UserSessions
            .Where(s => s.UserId == userId && s.JwtToken == token && !s.IsRevoked)
            .FirstOrDefaultAsync();
        
        if (session != null)
        {
            session.IsRevoked = true;
            await _context.SaveChangesAsync();
            _logger.LogInformation("User logged out: UserId {UserId}", userId);
        }
        
        return Ok(new { message = "Logged out successfully" });
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUserInfo()
    {
        var userId = GetCurrentUserId();
        
        // LINQ с проекцией (выбираем только нужные поля)
        var user = await _context.Users
            .Where(u => u.Id == userId && !u.IsDeleted)
            .Select(u => new 
            {
                u.Id,
                u.Username,
                u.Email,
                u.CreatedAt,
                // Не забываем про сессии
                ActiveSessionsCount = _context.UserSessions
                    .Count(s => s.UserId == u.Id && !s.IsRevoked)
            })
            .FirstOrDefaultAsync();
        
        if (user == null)
            return NotFound(new { message = "User not found" });
        
        return Ok(user);
    }

    // Бонус: статистика по пользователям (чистый LINQ)
    [HttpGet("stats")]
    [Authorize]
    public async Task<IActionResult> GetStats()
    {
        var userId = GetCurrentUserId();
        
        // Сложный LINQ запрос с группировкой
        var stats = await _context.Users
            .Where(u => u.Id == userId && !u.IsDeleted)
            .Select(u => new
            {
                u.Username,
                TotalFiles = _context.Files.Count(f => f.UserId == u.Id && f.DeletedAt == null),
                TotalSizeBytes = _context.Files
                    .Where(f => f.UserId == u.Id && f.DeletedAt == null)
                    .Sum(f => f.SizeBytes),
                ActiveSessions = _context.UserSessions
                    .Count(s => s.UserId == u.Id && !s.IsRevoked),
                GroupsCount = _context.GroupMembers
                    .Count(gm => gm.UserId == u.Id && gm.LeftAt == null)
            })
            .FirstOrDefaultAsync();
        
        return Ok(stats);
    }

    // ===================== ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ =====================

    private string GenerateJwtToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration["Jwt:Key"] ?? "clodu-super-secret-key-2024-very-long-and-secure!"));
        
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"] ?? "Clodu",
            audience: _configuration["Jwt:Audience"] ?? "CloduClient",
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private int GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            throw new UnauthorizedAccessException("User ID not found in token");
        
        return int.Parse(userIdClaim);
    }

    private string GetCurrentToken()
    {
        var authHeader = Request.Headers["Authorization"].FirstOrDefault();
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            throw new UnauthorizedAccessException("Token not found");
        
        return authHeader.Substring("Bearer ".Length);
    }
}