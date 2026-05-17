using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clodu.API.Models;

[Table("users")]
public class User
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Username { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;
    
    public string PasswordHash { get; set; } = string.Empty;
    
    // ❌ НЕТ поля SubscriptionId! Подписка теперь в истории
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; }
    
    // Навигационные свойства
    public ICollection<UserSubscriptionHistory> SubscriptionHistory { get; set; } = new List<UserSubscriptionHistory>();
    public ICollection<File> Files { get; set; } = new List<File>();
    public ICollection<Group> OwnedGroups { get; set; } = new List<Group>();
    public ICollection<GroupMember> GroupMembers { get; set; } = new List<GroupMember>();
    public ICollection<UserSession> Sessions { get; set; } = new List<UserSession>();
    
    // 👇 Удобные методы
    
    /// <summary>
    /// Получить активную подписку пользователя
    /// </summary>
    public UserSubscriptionHistory? GetActiveSubscription()
    {
        return SubscriptionHistory.FirstOrDefault(s => s.IsActive);
    }
    
    /// <summary>
    /// Проверить, есть ли активная подписка (не истекла)
    /// </summary>
    public bool HasActiveSubscription()
    {
        var active = GetActiveSubscription();
        if (active == null) return false;
        
        // Если есть дата окончания и она в прошлом
        if (active.EndsAt.HasValue && active.EndsAt < DateTime.UtcNow)
            return false;
            
        return true;
    }
    
    /// <summary>
    /// Получить текущий план подписки (Free, Pro, Business)
    /// </summary>
    public Subscription? GetCurrentSubscriptionPlan()
    {
        return GetActiveSubscription()?.Subscription;
    }
    
    /// <summary>
    /// Получить название текущей подписки
    /// </summary>
    public string GetCurrentSubscriptionName()
    {
        return GetCurrentSubscriptionPlan()?.Name ?? "Free";
    }
    
    /// <summary>
    /// Получить максимальное место в байтах (с учётом подписки)
    /// </summary>
    public long GetMaxStorageBytes()
    {
        var plan = GetCurrentSubscriptionPlan();
        return plan?.MaxStorageBytes ?? 5L * 1024 * 1024 * 1024; // 5GB default
    }
    
    /// <summary>
    /// Проверить, не истекла ли подписка (нужно ли откатить на Free)
    /// </summary>
    public bool IsSubscriptionExpired()
    {
        var active = GetActiveSubscription();
        if (active == null) return true;
        if (active.SubscriptionId == 1) return false; // Free никогда не истекает
        
        return active.EndsAt.HasValue && active.EndsAt < DateTime.UtcNow;
    }
    
    /// <summary>
    /// Дней до окончания подписки
    /// </summary>
    public int? DaysUntilExpiration()
    {
        var active = GetActiveSubscription();
        if (active?.EndsAt == null) return null;   
        
        var days = (active.EndsAt.Value - DateTime.UtcNow).Days;
        return days > 0 ? days : 0;
    }
}