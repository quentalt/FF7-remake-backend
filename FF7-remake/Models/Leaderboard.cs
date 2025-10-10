using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FF7_remake.Models;

public class Leaderboard
{
    [Key]
    public int LeaderBoardId { get; set; }
    
    [Required]
    public int UserId { get; set; }
    
    [Required]
    public int Score { get; set; }
    
    [StringLength(50)]
    public int Ranking { get; set; }
    
    public DateTime AchievedAt { get; set; }
    
    [ForeignKey("UserId")]
    public virtual User User { get; set; }
    
}