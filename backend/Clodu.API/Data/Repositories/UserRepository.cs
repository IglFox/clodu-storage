using Microsoft.EntityFrameworkCore;
using Clodu.API.Models;

namespace Clodu.API.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    
    public UserRepository(AppDbContext context)
    {
        _context = context;
    }
    
    // ==================== Базовые CRUD ====================
    
    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users
            .Include(u => u.SubscriptionHistory.Where(h => h.IsActive))
            .ThenInclude(h => h.Subscription)
            .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
    }
    
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted);
    }
    
    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Username == username && !u.IsDeleted);
    }
    
    public async Task<List<User>> GetAllAsync(int skip = 0, int take = 50)
    {
        return await _context.Users
            .Where(u => !u.IsDeleted)
            .OrderByDescending(u => u.CreatedAt)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }
    
    public async Task<User> CreateAsync(User user)
    {
        user.CreatedAt = DateTime.UtcNow;
        user.IsDeleted = false;
        
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }
    
    public async Task<User> UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return user;
    }
    
    public async Task<bool> DeleteAsync(int id) // Soft delete
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null || user.IsDeleted) return false;
        
        user.IsDeleted = true;
        
        // Деактивируем все сессии
        var sessions = await _context.UserSessions
            .Where(s => s.UserId == id && !s.IsRevoked)
            .ToListAsync();
        
        foreach (var session in sessions)
        {
            session.IsRevoked = true;
        }
        
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> HardDeleteAsync(int id)
    {
        var user = await _context.Users
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(u => u.Id == id);
        
        if (user == null) return false;
        
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> RestoreAsync(int id)
    {
        var user = await _context.Users
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(u => u.Id == id && u.IsDeleted);
        
        if (user == null) return false;
        
        user.IsDeleted = false;
        await _context.SaveChangesAsync();
        return true;
    }
    
    // ==================== Проверки существования ====================
    
    public async Task<bool> ExistsByIdAsync(int id)
    {
        return await _context.Users
            .AnyAsync(u => u.Id == id && !u.IsDeleted);
    }
    
    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _context.Users
            .AnyAsync(u => u.Email == email && !u.IsDeleted);
    }
    
    public async Task<bool> ExistsByUsernameAsync(string username)
    {
        return await _context.Users
            .AnyAsync(u => u.Username == username && !u.IsDeleted);
    }
    
    // ==================== Статистика и квоты ====================
    
    public async Task<long> GetUsedStorageAsync(int userId)
    {
        return await _context.Files
            .Where(f => f.UserId == userId && f.DeletedAt == null)
            .SumAsync(f => f.SizeBytes);
    }
    
    public async Task<long> GetAvailableStorageAsync(int userId)
    {
        var user = await GetByIdAsync(userId);
        if (user == null) return 0;
        
        var activeSubscription = await GetActiveSubscriptionAsync(userId);
        var maxStorage = activeSubscription?.Subscription?.MaxStorageBytes ?? 5L * 1024 * 1024 * 1024;
        var used = await GetUsedStorageAsync(userId);
        
        return maxStorage - used;
    }
    
    public async Task<int> GetFilesCountAsync(int userId)
    {
        return await _context.Files
            .CountAsync(f => f.UserId == userId && f.DeletedAt == null);
    }
    
    public async Task<int> GetActiveSessionsCountAsync(int userId)
    {
        return await _context.UserSessions
            .CountAsync(s => s.UserId == userId && !s.IsRevoked && s.ExpiresAt > DateTime.UtcNow);
    }
    
    // ==================== Поиск ====================
    
    public async Task<List<User>> SearchAsync(string? query, int skip = 0, int take = 50)
    {
        var users = _context.Users.Where(u => !u.IsDeleted);
        
        if (!string.IsNullOrEmpty(query))
        {
            users = users.Where(u => 
                u.Username.Contains(query) || 
                u.Email.Contains(query));
        }
        
        return await users
            .OrderBy(u => u.Username)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }
    
    public async Task<List<User>> GetRecentlyActiveAsync(int days = 7, int take = 50)
    {
        var cutoffDate = DateTime.UtcNow.AddDays(-days);
        
        var activeUserIds = await _context.UserSessions
            .Where(s => s.CreatedAt >= cutoffDate && !s.IsRevoked)
            .Select(s => s.UserId)
            .Distinct()
            .ToListAsync();
        
        return await _context.Users
            .Where(u => activeUserIds.Contains(u.Id) && !u.IsDeleted)
            .OrderByDescending(u => u.CreatedAt)
            .Take(take)
            .ToListAsync();
    }
    
    // ==================== Управление подпиской ====================
    
    public async Task<UserSubscriptionHistory?> GetActiveSubscriptionAsync(int userId)
    {
        return await _context.UserSubscriptionHistory
            .Include(h => h.Subscription)
            .FirstOrDefaultAsync(h => h.UserId == userId && h.IsActive);
    }
    
    public async Task<UserSubscriptionHistory> ActivateSubscriptionAsync(
        int userId, int subscriptionId, int months, int? pricePaid = null)
    {
        // Деактивируем текущую активную подписку
        var currentActive = await GetActiveSubscriptionAsync(userId);
        if (currentActive != null)
        {
            currentActive.IsActive = false;
            currentActive.CancelledAt = DateTime.UtcNow;
        }
        
        // Создаём новую
        var newSubscription = new UserSubscriptionHistory
        {
            UserId = userId,
            SubscriptionId = subscriptionId,
            StartedAt = DateTime.UtcNow,
            EndsAt = months > 0 ? DateTime.UtcNow.AddMonths(months) : null,
            IsActive = true,
            PricePaid = pricePaid
        };
        
        _context.UserSubscriptionHistory.Add(newSubscription);
        await _context.SaveChangesAsync();
        
        return newSubscription;
    }
    
    public async Task<bool> CancelSubscriptionAsync(int userId)
    {
        var active = await GetActiveSubscriptionAsync(userId);
        if (active == null || active.SubscriptionId == 1) // 1 = Free
            return false;
        
        active.IsActive = false;
        active.CancelledAt = DateTime.UtcNow;
        
        // Автоматически переключаем на Free
        await ActivateSubscriptionAsync(userId, 1, 0);
        
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<List<UserSubscriptionHistory>> GetSubscriptionHistoryAsync(int userId)
    {
        return await _context.UserSubscriptionHistory
            .Include(h => h.Subscription)
            .Where(h => h.UserId == userId)
            .OrderByDescending(h => h.StartedAt)
            .ToListAsync();
    }
    
    // ==================== Группы пользователя ====================
    
    public async Task<List<Group>> GetUserGroupsAsync(int userId)
    {
        return await _context.GroupMembers
            .Where(gm => gm.UserId == userId && gm.LeftAt == null)
            .Include(gm => gm.Group)
            .Select(gm => gm.Group)
            .Where(g => g.DeletedAt == null)
            .ToListAsync();
    }
    
    public async Task<List<User>> GetGroupMembersAsync(int groupId)
    {
        return await _context.GroupMembers
            .Where(gm => gm.GroupId == groupId && gm.LeftAt == null)
            .Include(gm => gm.User)
            .Select(gm => gm.User)
            .Where(u => !u.IsDeleted)
            .ToListAsync();
    }
    
    public async Task<bool> IsUserInGroupAsync(int userId, int groupId)
    {
        return await _context.GroupMembers
            .AnyAsync(gm => gm.UserId == userId && gm.GroupId == groupId && gm.LeftAt == null);
    }
    
    public async Task<bool> IsGroupAdminAsync(int userId, int groupId)
    {
        var member = await _context.GroupMembers
            .FirstOrDefaultAsync(gm => gm.UserId == userId && gm.GroupId == groupId && gm.LeftAt == null);
        
        return member != null && member.Rights == 1; // 1 = Admin
    }
}