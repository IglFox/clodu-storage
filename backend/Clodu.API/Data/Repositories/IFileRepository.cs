using Clodu.API.Models;

namespace Clodu.API.Data.Repositories;

public interface IFileRepository
{
    // Базовые CRUD
    Task<FileData?> GetByIdAsync(int id);
    Task<FileData?> GetByIdWithDetailsAsync(int id);
    Task<List<FileData>> GetByUserIdAsync(int userId, int skip = 0, int take = 50);
    Task<List<FileData>> GetByFolderIdAsync(int folderId, int skip = 0, int take = 50);
    Task<FileData> CreateAsync(FileData file);
    Task<FileData> UpdateAsync(FileData file);
    Task<bool> DeleteAsync(int id);
    Task<bool> SoftDeleteAsync(int id);
    Task<bool> RestoreAsync(int id);
    
    // Статистика
    Task<long> GetTotalSizeByUserAsync(int userId);
    Task<int> GetCountByUserAsync(int userId);
    Task<long> GetTotalSizeByFolderAsync(int folderId);
    
    // Поиск
    Task<List<FileData>> SearchAsync(string? query, int userId, int skip = 0, int take = 50);
    Task<List<FileData>> GetRecentAsync(int userId, int days = 7, int take = 50);
    Task<List<FileData>> GetByTypeAsync(int userId, string fileType, int skip = 0, int take = 50);
    
    // Управление шардами
    Task<bool> UpdateShardLocationsAsync(int fileId, List<ShardLocation> locations);
    Task<bool> AddShardLocationAsync(int fileId, ShardLocation location);
    
    // Проверки
    Task<bool> ExistsAsync(int id);
    Task<bool> OwnedByUserAsync(int fileId, int userId);
    
    // Теги
    Task<List<Tag>> GetTagsAsync(int fileId);
    Task<bool> AddTagAsync(int fileId, int tagId);
    Task<bool> RemoveTagAsync(int fileId, int tagId);
    Task<bool> AddTagsAsync(int fileId, List<int> tagIds);
    Task<bool> RemoveAllTagsAsync(int fileId);
}