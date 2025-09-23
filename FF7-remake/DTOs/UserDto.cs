namespace FF7_remake.DTOs;

public class UserDto
{
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    
    public string Password { get; set; } = string.Empty;
    public string Progress { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}