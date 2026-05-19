using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Clodu.API.Models;

public class FileData
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(100)]
    public string Type { get; set; } = string.Empty;
    
    public long SizeBytes { get; set; }
    
    [MaxLength(64)]
    public string Hash { get; set; } = string.Empty;
    
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    
    public DateTime AddedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    
    public int TotalShards { get; set; } = 16;
    public int DataShards { get; set; } = 10;
    public int ParityShards => TotalShards - DataShards;
    
    // Хранится в БД как JSON строка
    [Column(TypeName = "jsonb")]
    public string ShardLocations { get; set; } = "[]";
    
    // Для удобной работы в коде (не хранится в БД)
    [NotMapped]
    public List<ShardLocation> ShardLocationsList
    {
        get => JsonSerializer.Deserialize<List<ShardLocation>>(ShardLocations) ?? new List<ShardLocation>();
        set => ShardLocations = JsonSerializer.Serialize(value);
    }
    
    // Навигация
    public ICollection<FolderFile> FolderFiles { get; set; } = new List<FolderFile>();
    public ICollection<FileTag> FileTags { get; set; } = new List<FileTag>();
    
    // Удобные методы
    public bool IsDeleted() => DeletedAt != null;
    public void SoftDelete() => DeletedAt = DateTime.UtcNow;
    public void Restore() => DeletedAt = null;
}