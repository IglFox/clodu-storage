using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Clodu.API.Models;

[Table("user_session")]
public class UserSession
{
    [Key]
    public int Id { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    
    [JsonIgnore]
    public string JwtToken { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Дополнительные поля (если хочешь расширить)
    public DateTime? ExpiresAt { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public bool IsRevoked { get; set; }
    
    // Удобные методы
    public bool IsExpired() => ExpiresAt.HasValue && ExpiresAt < DateTime.UtcNow;
    
    public bool IsValid() => !IsRevoked && !IsExpired();
    
    public void Revoke() => IsRevoked = true;
}