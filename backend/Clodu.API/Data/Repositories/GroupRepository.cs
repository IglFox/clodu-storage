using Microsoft.EntityFrameworkCore;
using Clodu.API.Models;

namespace Clodu.API.Data.Repositories;

public class GroupRepository : IGroupRepository
{
    private readonly AppDbContext _context;
    
    public GroupRepository(AppDbContext context)
    {
        _context = context;
    }
    
    // ==================== Базовые CRUD ====================
    
    public async Task<Group?> GetByIdAsync(int id)
    {
        return await _context.Groups
            .FirstOrDefaultAsync(g => g.Id == id && g.DeletedAt == null);
    }
    
    public async Task<Group?> GetByIdWithDetailsAsync(int id)
    {
        return await _context.Groups
            .Include(g => g.Owner)
            .Include(g => g.Members.Where(m => m.LeftAt == null))
                .ThenInclude(m => m.User)
            .Include(g => g.Logs)
            .FirstOrDefaultAsync(g => g.Id == id && g.DeletedAt == null);
    }
    
    public async Task<List<Group>> GetByOwnerIdAsync(int ownerId)
    {
        return await _context.Groups
            .Where(g => g.OwnerId == ownerId && g.DeletedAt == null)
            .OrderByDescending(g => g.CreatedAt)
            .ToListAsync();
    }
    
    public async Task<List<Group>> GetByUserIdAsync(int userId)
    {
        return await _context.GroupMembers
            .Where(gm => gm.UserId == userId && gm.LeftAt == null)
            .Include(gm => gm.Group)
            .Select(gm => gm.Group)
            .Where(g => g.DeletedAt == null)
            .ToListAsync();
    }
    
    public async Task<List<Group>> GetAllAsync(int skip = 0, int take = 50)
    {
        return await _context.Groups
            .Where(g => g.DeletedAt == null)
            .OrderByDescending(g => g.CreatedAt)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }
    
    public async Task<Group> CreateAsync(Group group)
    {
        group.CreatedAt = DateTime.UtcNow;
        _context.Groups.Add(group);
        await _context.SaveChangesAsync();
        return group;
    }
    
    public async Task<Group> UpdateAsync(Group group)
    {
        _context.Groups.Update(group);
        await _context.SaveChangesAsync();
        return group;
    }
    
    public async Task<bool> DeleteAsync(int id)
    {
        var group = await _context.Groups.FindAsync(id);
        if (group == null || group.DeletedAt != null) return false;
        
        group.DeletedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> HardDeleteAsync(int id)
    {
        var group = await _context.Groups
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(g => g.Id == id);
        
        if (group == null) return false;
        
        _context.Groups.Remove(group);
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> RestoreAsync(int id)
    {
        var group = await _context.Groups
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(g => g.Id == id && g.DeletedAt != null);
        
        if (group == null) return false;
        
        group.DeletedAt = null;
        await _context.SaveChangesAsync();
        return true;
    }
    
    // ==================== Управление участниками ====================
    
    public async Task<GroupMember?> GetMemberAsync(int groupId, int userId)
    {
        return await _context.GroupMembers
            .FirstOrDefaultAsync(gm => gm.GroupId == groupId && gm.UserId == userId);
    }
    
    public async Task<List<GroupMember>> GetMembersAsync(int groupId)
    {
        return await _context.GroupMembers
            .Where(gm => gm.GroupId == groupId && gm.LeftAt == null)
            .Include(gm => gm.User)
            .OrderBy(gm => gm.AddedAt)
            .ToListAsync();
    }
    
    public async Task<List<User>> GetMemberUsersAsync(int groupId)
    {
        return await _context.GroupMembers
            .Where(gm => gm.GroupId == groupId && gm.LeftAt == null)
            .Include(gm => gm.User)
            .Select(gm => gm.User)
            .ToListAsync();
    }
    
    public async Task<GroupMember> AddMemberAsync(int groupId, int userId, int rights = 3)
    {
        var existing = await GetMemberAsync(groupId, userId);
        if (existing != null)
        {
            if (existing.LeftAt != null)
            {
                existing.LeftAt = null;
                existing.Rights = rights;
                existing.AddedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                return existing;
            }
            return existing;
        }
        
        var member = new GroupMember
        {
            GroupId = groupId,
            UserId = userId,
            Rights = rights,
            AddedAt = DateTime.UtcNow
        };
        
        _context.GroupMembers.Add(member);
        await _context.SaveChangesAsync();
        return member;
    }
    
    public async Task<bool> UpdateMemberRightsAsync(int groupId, int userId, int newRights)
    {
        var member = await GetMemberAsync(groupId, userId);
        if (member == null || member.LeftAt != null) return false;
        
        member.Rights = newRights;
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> RemoveMemberAsync(int groupId, int userId)
    {
        var member = await GetMemberAsync(groupId, userId);
        if (member == null || member.LeftAt != null) return false;
        
        member.LeftAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> IsMemberAsync(int groupId, int userId)
    {
        return await _context.GroupMembers
            .AnyAsync(gm => gm.GroupId == groupId && gm.UserId == userId && gm.LeftAt == null);
    }
    
    public async Task<int> GetMembersCountAsync(int groupId)
    {
        return await _context.GroupMembers
            .CountAsync(gm => gm.GroupId == groupId && gm.LeftAt == null);
    }
    
    // ==================== Права и проверки ====================
    
    public async Task<bool> IsOwnerAsync(int groupId, int userId)
    {
        var group = await _context.Groups
            .FirstOrDefaultAsync(g => g.Id == groupId && g.DeletedAt == null);
        
        return group != null && group.OwnerId == userId;
    }
    
    public async Task<bool> IsAdminAsync(int groupId, int userId)
    {
        var member = await GetMemberAsync(groupId, userId);
        return member != null && member.LeftAt == null && member.Rights == 1;
    }
    
    public async Task<bool> HasPermissionAsync(int groupId, int userId, int requiredRights)
    {
        if (await IsOwnerAsync(groupId, userId)) return true;
        
        var member = await GetMemberAsync(groupId, userId);
        if (member == null || member.LeftAt != null) return false;
        
        return member.Rights <= requiredRights; // 1(Admin) имеет права 2 и 3
    }
    
    public async Task<int> GetUserRightsAsync(int groupId, int userId)
    {
        if (await IsOwnerAsync(groupId, userId)) return 1;
        
        var member = await GetMemberAsync(groupId, userId);
        if (member == null || member.LeftAt != null) return 0;
        
        return member.Rights;
    }
    
    // ==================== Логи ====================
    
    public async Task<GroupLog> AddLogAsync(int groupId, int userId, string description)
    {
        var log = new GroupLog
        {
            GroupId = groupId,
            UserId = userId,
            Description = description,
            EditedAt = DateTime.UtcNow
        };
        
        _context.GroupLogs.Add(log);
        await _context.SaveChangesAsync();
        return log;
    }
    
    public async Task<List<GroupLog>> GetLogsAsync(int groupId, int skip = 0, int take = 50)
    {
        return await _context.GroupLogs
            .Where(l => l.GroupId == groupId)
            .Include(l => l.User)
            .OrderByDescending(l => l.EditedAt)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }
    
    // ==================== Поиск ====================
    
    public async Task<List<Group>> SearchAsync(string? query, int skip = 0, int take = 50)
    {
        var groups = _context.Groups.Where(g => g.DeletedAt == null);
        
        if (!string.IsNullOrEmpty(query))
        {
            groups = groups.Where(g => g.Groupname.Contains(query));
        }
        
        return await groups
            .OrderBy(g => g.Groupname)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }
}