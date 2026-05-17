using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clodu.API.Models;

[Table("user_subscription_history")]
public class UserSubscriptionHistory
{
    [Key]
    public int Id { get; set; }
    
    // Внешние ключи
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    
    public int SubscriptionId { get; set; }
    public Subscription Subscription { get; set; } = null!;
    
    // Временные метки
    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public DateTime? EndsAt { get; set; }      // NULL = бессрочно (Free)
    public DateTime? CancelledAt { get; set; } // если отменили досрочно
    
    // Статус
    public bool IsActive { get; set; } = true; // активная ли сейчас эта запись
    
    // Цена на момент покупки (если менялась)
    public int? PricePaid { get; set; }
    
    // 👇 Удобные методы
    
    public bool IsExpired()
    {
        if (!IsActive) return true;
        if (EndsAt == null) return false;  // Бессрочная
        return EndsAt < DateTime.UtcNow;
    }
    
    public bool IsCancelled()
    {
        return CancelledAt != null;
    }
    
    public TimeSpan? TimeRemaining()
    {
        if (EndsAt == null) return null;
        var remaining = EndsAt.Value - DateTime.UtcNow;
        return remaining > TimeSpan.Zero ? remaining : TimeSpan.Zero;
    }
    
    public int DaysRemaining()
    {
        return (int?)TimeRemaining()?.TotalDays ?? 0;
    }
}