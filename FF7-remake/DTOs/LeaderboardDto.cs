namespace FF7_remake.DTOs;

public class LeaderboardDto
{
    public int LeaderboardId { get; set; }
    public int UserId { get; set; }
    public int Score { get; set; }
    public string Ranking { get; set; } = string.Empty;
    public DateTime AchievedAt { get; set; }
    public string? UserName { get; set; }
}