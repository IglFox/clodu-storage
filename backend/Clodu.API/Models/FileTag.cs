using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clodu.API.Models;

[Table("file_tags")]
public class FileTag
{
    [Key]
    public int Id { get; set; }
    
    public int TagId { get; set; }
    public Tag Tag { get; set; } = null!;
    
    public int FileId { get; set; }
    public File File { get; set; } = null!;
}