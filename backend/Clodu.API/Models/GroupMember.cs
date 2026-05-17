using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clodu.API.Models;

/// <summary>
/// Права доступа в группе
/// </summary>
public enum GroupRights
{
    Admin = 1,   // Полный доступ, может удалять группу
    Editor = 2,  // Может добавлять/удалять файлы
    Viewer = 3   // Только чтение
}

[Table("group_members")]
public class GroupMember
{
    [Key]
    public int Id { get; set; }
    
    public int GroupId { get; set; }
    public Group Group { get; set; } = null!;
    
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    
    public int Rights { get; set; }  // 1=Admin, 2=Editor, 3=Viewer
    
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LeftAt { get; set; }
    
    // Удобные свойства
    public GroupRights RightsLevel => (GroupRights)Rights;
    
    public bool IsActive() => LeftAt == null;
    
    public bool IsAdmin() => Rights == (int)GroupRights.Admin;
    
    public bool HasPermission(GroupRights requiredRight)
    {
        return RightsLevel <= requiredRight;  // Admin(1) имеет права Editor(2) и Viewer(3)
    }
    
    public void Leave() => LeftAt = DateTime.UtcNow;
    
    public void ChangeRights(GroupRights newRights)
    {
        Rights = (int)newRights;
    }
}