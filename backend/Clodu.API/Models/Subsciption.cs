using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clodu.API.Models;

[Table("subscription")]
public class Subscription
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;  // "Free", "Pro", "Business"
    
    public int Price { get; set; }  // Цена в рублях (копейках если умножить на 100)
    
    // Лимиты
    public long MaxStorageBytes { get; set; } = 5L * 1024 * 1024 * 1024;  // 5 GB
    public int MaxFileSizeMB { get; set; } = 100;
    public int MaxTeamMembers { get; set; } = 1;
    
    // Гибкие фичи (JSON)
    [Column(TypeName = "jsonb")]
    public Dictionary<string, object> Features { get; set; } = new();
    
    public bool IsActive { get; set; } = true;
    
    // Навигация
    public ICollection<UserSubscriptionHistory> UserHistory { get; set; } = new List<UserSubscriptionHistory>();
}