using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Clodu.API.Models;

[Table("subscription")]
public class Subscription
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    public int Price { get; set; }
    
    public long MaxStorageBytes { get; set; } = 5L * 1024 * 1024 * 1024;
    public int MaxFileSizeMB { get; set; } = 100;
    public int MaxTeamMembers { get; set; } = 1;
    
    // Для БД — храним как JSON строку
    [Column(TypeName = "jsonb")]
    public string Features { get; set; } = "{}";
    
    // Для удобной работы в коде (не хранится в БД)
    [NotMapped]
    public Dictionary<string, object> FeaturesDict
    {
        get => JsonSerializer.Deserialize<Dictionary<string, object>>(Features) ?? new();
        set => Features = JsonSerializer.Serialize(value);
    }
    
    public bool IsActive { get; set; } = true;
    
    public ICollection<UserSubscriptionHistory> UserHistory { get; set; } = new List<UserSubscriptionHistory>();
}