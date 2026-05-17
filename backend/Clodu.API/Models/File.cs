using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clodu.API.Models;

[Table("files")]
public class File
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(100)]
    public string Type { get; set; } = string.Empty;  // mime type или расширение
    
    public long SizeBytes { get; set; }
    
    [MaxLength(64)]
    public string Hash { get; set; } = string.Empty;   // SHA256
    
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeletedAt { get; set; }
    
    // RS параметры (для кодирования)
    public int TotalShards { get; set; } = 16;
    public int DataShards { get; set; } = 10;
    public int ParityShards => TotalShards - DataShards;
    
    // Распределение шардов (JSON)
    [Column(TypeName = "jsonb")]
    public List<ShardLocation> ShardLocations { get; set; } = new();
    
    // Навигационные свойства
    public ICollection<FolderFile> FolderFiles { get; set; } = new List<FolderFile>();
    public ICollection<FileTag> FileTags { get; set; } = new List<FileTag>();
    
    // Удобные методы
    public bool IsDeleted() => DeletedAt != null;
    
    public void SoftDelete() => DeletedAt = DateTime.UtcNow;
    
    public void Restore() => DeletedAt = null;
    
    public string GetFormattedSize()
    {
        string[] sizes = { "B", "KB", "MB", "GB", "TB" };
        double len = SizeBytes;
        int order = 0;
        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len = len / 1024;
        }
        return $"{len:0.##} {sizes[order]}";
    }
    
    public bool CanRecover(int availableShards)
    {
        return availableShards >= DataShards;
    }
}

/// <summary>
/// Расположение шарда в хранилище
/// </summary>
public class ShardLocation
{
    public int Index { get; set; }
    public string Path { get; set; } = string.Empty;  // URL в MinIO
    public bool IsParity { get; set; }
    public long SizeBytes { get; set; }
    public string? NodeId { get; set; }  // для распределённого хранения
}