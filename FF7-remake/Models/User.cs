using System.ComponentModel.DataAnnotations;

namespace FF7_remake.Models;

public class User
{
    [Key]
    public int UserId { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Username { get; set; }
    
    [Required]
    [StringLength(200)]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    [StringLength(255)]
    public string Password { get; set; }
    
    [StringLength(1000)]
    public string Progress { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public virtual ICollection<UserProgress>UserProgresses { get; set; }
    public virtual ICollection<Leaderboard> LeaderboardEntries { get; set; }
}