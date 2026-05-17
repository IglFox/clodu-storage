using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clodu.API.Models;

[Table("tags")]
public class Tag
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;
    
    // Навигационные свойства
    public ICollection<FileTag> FileTags { get; set; } = new List<FileTag>();
    public ICollection<SpaceTag> SpaceTags { get; set; } = new List<SpaceTag>();
}