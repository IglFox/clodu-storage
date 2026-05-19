using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Clodu.API.Models;

namespace Clodu.API.Models;

[Table("folder_files")]
public class FolderFile
{
    [Key]
    public int Id { get; set; }
    
    public int FolderId { get; set; }
    public Folder Folder { get; set; } = null!;
    
    public int FileId { get; set; }
    public FileData File { get; set; } = null!;
    
    public int? SpaceId { get; set; }
    public Space? Space { get; set; }
}