using Microsoft.EntityFrameworkCore;
using Clodu.API.Models;

namespace Clodu.API.Data.Repositories;

public class SessionRepository : ISessionRepository
{
    private readonly AppDbContext _context;
    
    public SessionRepository(AppDbContext context)
    {
        _context = context;
    }
    
    // ==================== Базовые операции ====================
    
    public async Task<UserSession?> GetByIdAsync(int id)
    {
        return await _context.UserSessions
            .FindAsync(id);
    }
    
    public async Task<UserSession?> GetByTokenAsync(string token)
    {
        return await _context.UserSessions
            .FirstOrDefaultAsync(s => s.JwtToken == token);
    }
    
    public async Task<UserSession?> GetByTokenWithUserAsync(string token)
    {
        return await _context.UserSessions
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.JwtToken == token);
    }
    
    public async Task<List<UserSession>> GetByUserIdAsync(int userId)
    {
        return await _context.UserSessions
            .Where(s => s.UserId == userId)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
    }
    
    public async Task<List<UserSession>> GetActiveByUserIdAsync(int userId)
    {
        return await _context.UserSessions
            .Where(s => s.UserId == userId && !s.IsRevoked && s.ExpiresAt > DateTime.UtcNow)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
    }
    
    public async Task<UserSession> CreateAsync(UserSession session)
    {
        session.CreatedAt = DateTime.UtcNow;
        session.IsRevoked = false;
        
        _context.UserSessions.Add(session);
        await _context.SaveChangesAsync();
        return session;
    }
    
    public async Task<UserSession> UpdateAsync(UserSession session)
    {
        _context.UserSessions.Update(session);
        await _context.SaveChangesAsync();
        return session;
    }
    
    // ==================== Управление сессиями ====================
    
    public async Task<bool> RevokeAsync(int sessionId)
    {
        var session = await _context.UserSessions.FindAsync(sessionId);
        if (session == null || session.IsRevoked) return false;
        
        session.IsRevoked = true;
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> RevokeByTokenAsync(string token)
    {
        var session = await _context.UserSessions
            .FirstOrDefaultAsync(s => s.JwtToken == token);
        
        if (session == null || session.IsRevoked) return false;
        
        session.IsRevoked = true;
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<int> RevokeAllByUserIdAsync(int userId, string? exceptToken = null)
    {
        var query = _context.UserSessions
            .Where(s => s.UserId == userId && !s.IsRevoked);
        
        if (!string.IsNullOrEmpty(exceptToken))
        {
            query = query.Where(s => s.JwtToken != exceptToken);
        }
        
        var sessions = await query.ToListAsync();
        
        foreach (var session in sessions)
        {
            session.IsRevoked = true;
        }
        
        await _context.SaveChangesAsync();
        return sessions.Count;
    }
    
    public async Task<int> RevokeAllExpiredAsync()
    {
        var expiredSessions = await _context.UserSessions
            .Where(s => s.ExpiresAt < DateTime.UtcNow && !s.IsRevoked)
            .ToListAsync();
        
        foreach (var session in expiredSessions)
        {
            session.IsRevoked = true;
        }
        
        await _context.SaveChangesAsync();
        return expiredSessions.Count;
    }
    
    public async Task<int> RevokeOldByUserIdAsync(int userId, int keepCount = 5)
    {
        var sessions = await _context.UserSessions
            .Where(s => s.UserId == userId && !s.IsRevoked)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
        
        var toRevoke = sessions.Skip(keepCount).ToList();
        
        foreach (var session in toRevoke)
        {
            session.IsRevoked = true;
        }
        
        await _context.SaveChangesAsync();
        return toRevoke.Count;
    }
    
    // ==================== Проверки ====================
    
    public async Task<bool> IsValidAsync(string token)
    {
        var session = await _context.UserSessions
            .FirstOrDefaultAsync(s => s.JwtToken == token);
        
        if (session == null) return false;
        if (session.IsRevoked) return false;
        if (session.ExpiresAt < DateTime.UtcNow) return false;
        
        return true;
    }
    
    public async Task<bool> IsActiveAsync(int sessionId)
    {
        var session = await _context.UserSessions.FindAsync(sessionId);
        
        if (session == null) return false;
        if (session.IsRevoked) return false;
        if (session.ExpiresAt < DateTime.UtcNow) return false;
        
        return true;
    }
    
    public async Task<int> GetActiveCountAsync(int userId)
    {
        return await _context.UserSessions
            .CountAsync(s => s.UserId == userId && !s.IsRevoked && s.ExpiresAt > DateTime.UtcNow);
    }
    
    // ==================== Очистка ====================
    
    public async Task<int> CleanupExpiredAsync()
    {
        var expiredSessions = await _context.UserSessions
            .Where(s => s.ExpiresAt < DateTime.UtcNow && !s.IsRevoked)
            .ToListAsync();
        
        foreach (var session in expiredSessions)
        {
            session.IsRevoked = true;
        }
        
        await _context.SaveChangesAsync();
        return expiredSessions.Count;
    }
    
    public async Task<int> CleanupOldAsync(int daysToKeep = 30)
    {
        var cutoffDate = DateTime.UtcNow.AddDays(-daysToKeep);
        
        var oldSessions = await _context.UserSessions
            .Where(s => s.CreatedAt < cutoffDate && s.IsRevoked)
            .ToListAsync();
        
        _context.UserSessions.RemoveRange(oldSessions);
        await _context.SaveChangesAsync();
        return oldSessions.Count;
    }
}