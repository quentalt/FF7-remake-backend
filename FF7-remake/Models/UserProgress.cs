using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FF7_remake.Models;

public class UserProgress
{
    [Key]
    
    [Required]
    public int Id { get; set; }
    
    [Required]
    public int UserId { get; set; }
    
    [Required]
    public int ChapterId { get; set; }
    
    public DateTime LastUpdated { get; set; }
    
    [ForeignKey("UserId")]
    public virtual User User { get; set; }
    
    [ForeignKey("ChapterId")]
    public virtual Chapter Chapter { get; set; }

    public bool IsCompleted { get; set; }
}