using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clodu.API.Models;

[Table("encryption_keys")]
public class EncryptionKey
{
    [Key]
    public int Id { get; set; }
    
    public int FileId { get; set; }
    public FileData File { get; set; } = null!;
    
    public string KeyData { get; set; } = string.Empty;
    public string IvData { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
}