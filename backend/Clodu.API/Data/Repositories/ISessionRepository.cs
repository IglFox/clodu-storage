using Clodu.API.Models;

namespace Clodu.API.Data.Repositories;

public interface ISessionRepository
{
    // Базовые операции
    Task<UserSession?> GetByIdAsync(int id);
    Task<UserSession?> GetByTokenAsync(string token);
    Task<UserSession?> GetByTokenWithUserAsync(string token);
    Task<List<UserSession>> GetByUserIdAsync(int userId);
    Task<List<UserSession>> GetActiveByUserIdAsync(int userId);
    Task<UserSession> CreateAsync(UserSession session);
    Task<UserSession> UpdateAsync(UserSession session);
    
    // Управление сессиями
    Task<bool> RevokeAsync(int sessionId);
    Task<bool> RevokeByTokenAsync(string token);
    Task<int> RevokeAllByUserIdAsync(int userId, string? exceptToken = null);
    Task<int> RevokeAllExpiredAsync();
    Task<int> RevokeOldByUserIdAsync(int userId, int keepCount = 5);
    
    // Проверки
    Task<bool> IsValidAsync(string token);
    Task<bool> IsActiveAsync(int sessionId);
    Task<int> GetActiveCountAsync(int userId);
    
    // Очистка
    Task<int> CleanupExpiredAsync();
    Task<int> CleanupOldAsync(int daysToKeep = 30);
}