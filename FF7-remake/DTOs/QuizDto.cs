namespace FF7_remake.DTOs;

public class QuizDto
{
    public int QuizId { get; set; }
    public int ChapterId { get; set; }
    public string Questions { get; set; } = string.Empty;
    public string CorrectAnswers { get; set; } = string.Empty;
    public string Badges { get; set; } = string.Empty;
    public string? ChapterTitle { get; set; }
}