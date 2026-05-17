using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clodu.API.Models;

[Table("folder_folders")]
public class FolderFolder
{
    [Key]
    public int Id { get; set; }
    
    public int ParentId { get; set; }
    public Folder Parent { get; set; } = null!;
    
    public int ChildId { get; set; }
    public Folder Child { get; set; } = null!;
}