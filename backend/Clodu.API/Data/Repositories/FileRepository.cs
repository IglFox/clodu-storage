using Microsoft.EntityFrameworkCore;
using Clodu.API.Models;

namespace Clodu.API.Data.Repositories;

public class FileRepository : IFileRepository
{
    private readonly AppDbContext _context;
    
    public FileRepository(AppDbContext context)
    {
        _context = context;
    }
    
    // ==================== Базовые CRUD ====================
    
    public async Task<FileData?> GetByIdAsync(int id)
    {
        return await _context.Files
            .FirstOrDefaultAsync(f => f.Id == id && f.DeletedAt == null);
    }
    
    public async Task<FileData?> GetByIdWithDetailsAsync(int id)
    {
        return await _context.Files
            .Include(f => f.User)
            .Include(f => f.FileTags)
                .ThenInclude(ft => ft.Tag)
            .Include(f => f.FolderFiles)
            .FirstOrDefaultAsync(f => f.Id == id && f.DeletedAt == null);
    }
    
    public async Task<List<FileData>> GetByUserIdAsync(int userId, int skip = 0, int take = 50)
    {
        return await _context.Files
            .Where(f => f.UserId == userId && f.DeletedAt == null)
            .OrderByDescending(f => f.AddedAt)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }
    
    public async Task<List<FileData>> GetByFolderIdAsync(int folderId, int skip = 0, int take = 50)
    {
        var fileIds = await _context.FolderFiles
            .Where(ff => ff.FolderId == folderId)
            .Select(ff => ff.FileId)
            .ToListAsync();
        
        return await _context.Files
            .Where(f => fileIds.Contains(f.Id) && f.DeletedAt == null)
            .OrderByDescending(f => f.AddedAt)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }
    
    public async Task<FileData> CreateAsync(FileData file)
    {
        file.AddedAt = DateTime.UtcNow;
        _context.Files.Add(file);
        await _context.SaveChangesAsync();
        return file;
    }
    
    public async Task<FileData> UpdateAsync(FileData file)
    {
        _context.Files.Update(file);
        await _context.SaveChangesAsync();
        return file;
    }
    
    public async Task<bool> DeleteAsync(int id)
    {
        var file = await _context.Files.FindAsync(id);
        if (file == null) return false;
        
        _context.Files.Remove(file);
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> SoftDeleteAsync(int id)
    {
        var file = await _context.Files.FindAsync(id);
        if (file == null || file.DeletedAt != null) return false;
        
        file.DeletedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> RestoreAsync(int id)
    {
        var file = await _context.Files
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(f => f.Id == id && f.DeletedAt != null);
        
        if (file == null) return false;
        
        file.DeletedAt = null;
        await _context.SaveChangesAsync();
        return true;
    }
    
    // ==================== Статистика ====================
    
    public async Task<long> GetTotalSizeByUserAsync(int userId)
    {
        return await _context.Files
            .Where(f => f.UserId == userId && f.DeletedAt == null)
            .SumAsync(f => f.SizeBytes);
    }
    
    public async Task<int> GetCountByUserAsync(int userId)
    {
        return await _context.Files
            .CountAsync(f => f.UserId == userId && f.DeletedAt == null);
    }
    
    public async Task<long> GetTotalSizeByFolderAsync(int folderId)
    {
        var fileIds = await _context.FolderFiles
            .Where(ff => ff.FolderId == folderId)
            .Select(ff => ff.FileId)
            .ToListAsync();
        
        return await _context.Files
            .Where(f => fileIds.Contains(f.Id) && f.DeletedAt == null)
            .SumAsync(f => f.SizeBytes);
    }
    
    // ==================== Поиск ====================
    
    public async Task<List<FileData>> SearchAsync(string? query, int userId, int skip = 0, int take = 50)
    {
        var files = _context.Files
            .Where(f => f.UserId == userId && f.DeletedAt == null);
        
        if (!string.IsNullOrEmpty(query))
        {
            files = files.Where(f => f.Name.Contains(query) || f.Type.Contains(query));
        }
        
        return await files
            .OrderByDescending(f => f.AddedAt)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }
    
    public async Task<List<FileData>> GetRecentAsync(int userId, int days = 7, int take = 50)
    {
        var cutoffDate = DateTime.UtcNow.AddDays(-days);
        
        return await _context.Files
            .Where(f => f.UserId == userId && f.AddedAt >= cutoffDate && f.DeletedAt == null)
            .OrderByDescending(f => f.AddedAt)
            .Take(take)
            .ToListAsync();
    }
    
    public async Task<List<FileData>> GetByTypeAsync(int userId, string fileType, int skip = 0, int take = 50)
    {
        return await _context.Files
            .Where(f => f.UserId == userId && f.Type == fileType && f.DeletedAt == null)
            .OrderByDescending(f => f.AddedAt)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }
    
    // ==================== Управление шардами ====================
    
    public async Task<bool> UpdateShardLocationsAsync(int fileId, List<ShardLocation> locations)
    {
        var file = await _context.Files.FindAsync(fileId);
        if (file == null) return false;
        
        file.ShardLocationsList = locations;
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> AddShardLocationAsync(int fileId, ShardLocation location)
    {
        var file = await _context.Files.FindAsync(fileId);
        if (file == null) return false;
        
        file.ShardLocationsList.Add(location);
        await _context.SaveChangesAsync();
        return true;
    }
    
    // ==================== Проверки ====================
    
    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Files
            .AnyAsync(f => f.Id == id && f.DeletedAt == null);
    }
    
    public async Task<bool> OwnedByUserAsync(int fileId, int userId)
    {
        return await _context.Files
            .AnyAsync(f => f.Id == fileId && f.UserId == userId && f.DeletedAt == null);
    }
    
    // ==================== Теги ====================
    
    public async Task<List<Tag>> GetTagsAsync(int fileId)
    {
        return await _context.FileTags
            .Where(ft => ft.FileId == fileId)
            .Include(ft => ft.Tag)
            .Select(ft => ft.Tag)
            .ToListAsync();
    }
    
    public async Task<bool> AddTagAsync(int fileId, int tagId)
    {
        if (await _context.FileTags.AnyAsync(ft => ft.FileId == fileId && ft.TagId == tagId))
            return false;
        
        var fileTag = new FileTag { FileId = fileId, TagId = tagId };
        _context.FileTags.Add(fileTag);
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> RemoveTagAsync(int fileId, int tagId)
    {
        var fileTag = await _context.FileTags
            .FirstOrDefaultAsync(ft => ft.FileId == fileId && ft.TagId == tagId);
        
        if (fileTag == null) return false;
        
        _context.FileTags.Remove(fileTag);
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> AddTagsAsync(int fileId, List<int> tagIds)
    {
        var existingTags = await _context.FileTags
            .Where(ft => ft.FileId == fileId)
            .Select(ft => ft.TagId)
            .ToListAsync();
        
        var newTags = tagIds.Where(t => !existingTags.Contains(t))
            .Select(t => new FileTag { FileId = fileId, TagId = t })
            .ToList();
        
        if (newTags.Any())
        {
            _context.FileTags.AddRange(newTags);
            await _context.SaveChangesAsync();
        }
        
        return true;
    }
    
    public async Task<bool> RemoveAllTagsAsync(int fileId)
    {
        var tags = await _context.FileTags
            .Where(ft => ft.FileId == fileId)
            .ToListAsync();
        
        if (tags.Any())
        {
            _context.FileTags.RemoveRange(tags);
            await _context.SaveChangesAsync();
        }
        
        return true;
    }
}