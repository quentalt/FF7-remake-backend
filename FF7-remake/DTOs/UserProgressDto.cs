namespace FF7_remake.DTOs;

public class UserProgressDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ChapterId { get; set; }
    public string SavedState { get; set; } = string.Empty;
    public DateTime LastUpdated { get; set; }
    public string? UserName { get; set; }
    public string? ChapterTitle { get; set; }
    public double ProgressPercentage { get; set; }

}