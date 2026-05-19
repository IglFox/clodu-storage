using Clodu.API.Models;

namespace Clodu.API.Data.Repositories;

public interface IGroupRepository
{
    // Базовые CRUD
    Task<Group?> GetByIdAsync(int id);
    Task<Group?> GetByIdWithDetailsAsync(int id);
    Task<List<Group>> GetByOwnerIdAsync(int ownerId);
    Task<List<Group>> GetByUserIdAsync(int userId); // Где пользователь состоит
    Task<List<Group>> GetAllAsync(int skip = 0, int take = 50);
    Task<Group> CreateAsync(Group group);
    Task<Group> UpdateAsync(Group group);
    Task<bool> DeleteAsync(int id); // Soft delete
    Task<bool> HardDeleteAsync(int id);
    Task<bool> RestoreAsync(int id);
    
    // Управление участниками
    Task<GroupMember?> GetMemberAsync(int groupId, int userId);
    Task<List<GroupMember>> GetMembersAsync(int groupId);
    Task<List<User>> GetMemberUsersAsync(int groupId);
    Task<GroupMember> AddMemberAsync(int groupId, int userId, int rights = 3);
    Task<bool> UpdateMemberRightsAsync(int groupId, int userId, int newRights);
    Task<bool> RemoveMemberAsync(int groupId, int userId);
    Task<bool> IsMemberAsync(int groupId, int userId);
    Task<int> GetMembersCountAsync(int groupId);
    
    // Права и проверки
    Task<bool> IsOwnerAsync(int groupId, int userId);
    Task<bool> IsAdminAsync(int groupId, int userId);
    Task<bool> HasPermissionAsync(int groupId, int userId, int requiredRights);
    Task<int> GetUserRightsAsync(int groupId, int userId);
    
    // Логи
    Task<GroupLog> AddLogAsync(int groupId, int userId, string description);
    Task<List<GroupLog>> GetLogsAsync(int groupId, int skip = 0, int take = 50);
    
    // Поиск
    Task<List<Group>> SearchAsync(string? query, int skip = 0, int take = 50);
}