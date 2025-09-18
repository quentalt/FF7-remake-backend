using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FF7_remake.Models;

public class Quiz
{
    [Key] public int QuizId { get; set; }
    [Required] public int ChapterId { get; set; }

    [Required] [StringLength(2000)] public string Question { get; set; }

    [Required] [StringLength(1000)] public string CorrectAnswer { get; set; }
    
    [StringLength(500)]
    public string Badges { get; set; }
    
    [ForeignKey("ChapterId")]
    public virtual Chapter Chapter { get; set; }

}