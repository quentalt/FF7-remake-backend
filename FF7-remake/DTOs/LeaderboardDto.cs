namespace FF7_remake.DTOs;

public class LeaderboardDto
{
    public int LeaderboardId { get; set; }
    public int UserId { get; set; }
    public int Score { get; set; }
    public int Ranking { get; set; }
    public DateTime AchievedAt { get; set; }
    public string? UserName { get; set; }
}