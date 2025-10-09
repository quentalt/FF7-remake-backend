namespace FF7_remake.DTOs;

public class LeaderboardDto
{
    public int LeaderboardId { get; set; }
    public int Score { get; set; }
    public int Ranking { get; set; } = 0;
    
    public string? UserName { get; set; }
    public DateTime AchievedAt { get; set; }
}