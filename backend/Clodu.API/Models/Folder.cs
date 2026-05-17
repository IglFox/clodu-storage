using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clodu.API.Models;

[Table("folders")]
public class Folder
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;
    
    public int OwnerId { get; set; }      // может быть user_id или group_id
    public string OwnerType { get; set; } = string.Empty; // "user" или "group"
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeletedAt { get; set; }
    
    // Навигационные свойства
    public ICollection<FolderFile> FolderFiles { get; set; } = new List<FolderFile>();
    public ICollection<FolderFolder> ParentFolders { get; set; } = new List<FolderFolder>();
    public ICollection<FolderFolder> ChildFolders { get; set; } = new List<FolderFolder>();
    
    // Удобные методы
    public bool IsDeleted() => DeletedAt != null;
    
    public void SoftDelete() => DeletedAt = DateTime.UtcNow;
    
    public bool IsUserFolder() => OwnerType == "user";
    
    public bool IsGroupFolder() => OwnerType == "group";
    
    public void AddChild(Folder child)
    {
        ChildFolders.Add(new FolderFolder { ParentId = this.Id, ChildId = child.Id });
    }
    
    public void AddFile(File file, int? spaceId = null)
    {
        FolderFiles.Add(new FolderFile { FolderId = this.Id, FileId = file.Id, SpaceId = spaceId });
    }
}