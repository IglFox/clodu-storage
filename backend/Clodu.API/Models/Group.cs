using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clodu.API.Models;

[Table("groups")]
public class Group
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Groupname { get; set; } = string.Empty;
    
    public int OwnerId { get; set; }
    public User Owner { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeletedAt { get; set; }
    
    // Навигационные свойства
    public ICollection<GroupMember> Members { get; set; } = new List<GroupMember>();
    public ICollection<GroupLog> Logs { get; set; } = new List<GroupLog>();
    
    // Удобные методы
    public bool IsDeleted() => DeletedAt != null;
    
    public void SoftDelete() => DeletedAt = DateTime.UtcNow;
    
    public bool IsOwner(int userId) => OwnerId == userId;
}