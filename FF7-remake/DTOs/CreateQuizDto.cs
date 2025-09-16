namespace FF7_remake.DTOs;

public class CreateQuizDto
{
    public int ChapterId { get; set; }
    public string Questions { get; set; } = string.Empty;
    public string CorrectAnswers { get; set; } = string.Empty;
    public string Badges { get; set; } = string.Empty;
}