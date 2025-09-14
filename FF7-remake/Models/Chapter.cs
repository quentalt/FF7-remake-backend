using System.ComponentModel.DataAnnotations;

namespace FF7_remake.Models;

public class Chapter
{
    [Key]
    public int ChapterId { get; set; }

    [Required] 
    [StringLength(200)]
    public string Title { get; set ; }
    
    
    [StringLength(1000)]
    public string Summary { get; set; }
    
    [StringLength(500)]
    public string Quiz { get; set; }
    
    public virtual ICollection<Quiz> Quizzes { get; set; }
    public virtual ICollection<UserProgress> UserProgresses  { get; set; }
        
    
}