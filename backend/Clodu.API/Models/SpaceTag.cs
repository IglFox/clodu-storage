using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clodu.API.Models;

[Table("space_tags")]
public class SpaceTag
{
    [Key]
    public int Id { get; set; }
    
    public int SpaceId { get; set; }
    public Space Space { get; set; } = null!;
    
    public int TagId { get; set; }
    public Tag Tag { get; set; } = null!;
}