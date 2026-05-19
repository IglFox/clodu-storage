using Clodu.API.Models;

namespace Clodu.API.Data.Repositories;

public interface IUserRepository
{
    // Базовые CRUD
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByUsernameAsync(string username);
    Task<List<User>> GetAllAsync(int skip = 0, int take = 50);
    Task<User> CreateAsync(User user);
    Task<User> UpdateAsync(User user);
    Task<bool> DeleteAsync(int id); // Soft delete
    Task<bool> HardDeleteAsync(int id); // Полное удаление
    Task<bool> RestoreAsync(int id); // Восстановление после soft delete
    
    // Проверки существования
    Task<bool> ExistsByIdAsync(int id);
    Task<bool> ExistsByEmailAsync(string email);
    Task<bool> ExistsByUsernameAsync(string username);
    
    // Статистика и квоты
    Task<long> GetUsedStorageAsync(int userId);
    Task<long> GetAvailableStorageAsync(int userId);
    Task<int> GetFilesCountAsync(int userId);
    Task<int> GetActiveSessionsCountAsync(int userId);
    
    // Поиск
    Task<List<User>> SearchAsync(string? query, int skip = 0, int take = 50);
    Task<List<User>> GetRecentlyActiveAsync(int days = 7, int take = 50);
    
    // Управление подпиской (через историю)
    Task<UserSubscriptionHistory?> GetActiveSubscriptionAsync(int userId);
    Task<UserSubscriptionHistory> ActivateSubscriptionAsync(int userId, int subscriptionId, int months, int? pricePaid = null);
    Task<bool> CancelSubscriptionAsync(int userId);
    Task<List<UserSubscriptionHistory>> GetSubscriptionHistoryAsync(int userId);
    
    // Группы пользователя
    Task<List<Group>> GetUserGroupsAsync(int userId);
    Task<List<User>> GetGroupMembersAsync(int groupId);
    Task<bool> IsUserInGroupAsync(int userId, int groupId);
    Task<bool> IsGroupAdminAsync(int userId, int groupId);
}