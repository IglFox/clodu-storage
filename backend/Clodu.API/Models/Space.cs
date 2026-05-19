using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clodu.API.Models;

[Table("spaces")]
public class Space
{
    [Key]
    public int Id { get; set; }
    
    public int OwnerId { get; set; }
    public User Owner { get; set; } = null!;
    
    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;
    
    // Навигационные свойства
    public ICollection<SpaceTag> SpaceTags { get; set; } = new List<SpaceTag>();
    public ICollection<FolderFile> FolderFiles { get; set; } = new List<FolderFile>();
    
    // Удобные методы
    public void AddTag(Tag tag)
    {
        SpaceTags.Add(new SpaceTag { SpaceId = this.Id, TagId = tag.Id });
    }
    
    public void RemoveTag(int tagId)
    {
        var spaceTag = SpaceTags.FirstOrDefault(st => st.TagId == tagId);
        if (spaceTag != null)
            SpaceTags.Remove(spaceTag);
    }
}