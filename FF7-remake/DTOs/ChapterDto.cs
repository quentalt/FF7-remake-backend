namespace FF7_remake.DTOs;

public class ChapterDto
{
    public int ChapterId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string Quiz { get; set; } = string.Empty;
    public List<QuizDto>? Quizzes { get; set; }
}