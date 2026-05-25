using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clodu.API.Models;

[Table("file_keys")]
public class FileKey
{
    [Key]
    public int Id { get; set; }

    public int FileId { get; set; }

    [Column(TypeName = "bytea")]
    public byte[] EncryptedKey { get; set; } = Array.Empty<byte>();

    [Column(TypeName = "bytea")]
    public byte[] IV { get; set; } = Array.Empty<byte>();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}