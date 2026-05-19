using System.Text.Json.Serialization;

namespace Clodu.API.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    
    [JsonIgnore]
    public string PasswordHash { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; }
    public bool IsDeleted { get; set; }
    
    // Навигационные свойства
    public ICollection<UserSession> Sessions { get; set; } = new List<UserSession>();
    public ICollection<FileData> Files { get; set; } = new List<FileData>();
    public ICollection<Group> OwnedGroups { get; set; } = new List<Group>();
    public ICollection<GroupMember> GroupMembers { get; set; } = new List<GroupMember>();
    public ICollection<Space> Spaces { get; set; } = new List<Space>();
    public ICollection<UserSubscriptionHistory> SubscriptionHistory { get; set; } = new List<UserSubscriptionHistory>();
}