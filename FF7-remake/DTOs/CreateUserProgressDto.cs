namespace FF7_remake.DTOs;

public class CreateUserProgressDto
{
    public int UserId { get; set; }
    public int ChapterId { get; set; }
    public string SavedState { get; set; } = string.Empty; 
    
}