namespace FF7_remake.DTOs;

public class CreateLeaderboardDto
{
    public int UserId { get; set; }
    public int Score { get; set; }
    public string Ranking { get; set; } = string.Empty;
}