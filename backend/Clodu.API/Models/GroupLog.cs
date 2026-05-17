using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clodu.API.Models;

[Table("group_logs")]
public class GroupLog
{
    [Key]
    public int Id { get; set; }
    
    public int GroupId { get; set; }
    public Group Group { get; set; } = null!;
    
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    
    public string Description { get; set; } = string.Empty;
    
    public DateTime EditedAt { get; set; } = DateTime.UtcNow;
    
    // Удобные методы создания логов
    public static GroupLog CreateMemberAdded(Group group, User actor, User newMember)
    {
        return new GroupLog
        {
            GroupId = group.Id,
            UserId = actor.Id,
            Description = $"{actor.Username} added {newMember.Username} to the group"
        };
    }
    
    public static GroupLog CreateMemberRemoved(Group group, User actor, User removedMember)
    {
        return new GroupLog
        {
            GroupId = group.Id,
            UserId = actor.Id,
            Description = $"{actor.Username} removed {removedMember.Username} from the group"
        };
    }
    
    public static GroupLog CreateRightsChanged(Group group, User actor, User target, GroupRights oldRights, GroupRights newRights)
    {
        return new GroupLog
        {
            GroupId = group.Id,
            UserId = actor.Id,
            Description = $"{actor.Username} changed {target.Username}'s rights from {oldRights} to {newRights}"
        };
    }
}