using FF7_remake.DBContext;
using FF7_remake.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace FF7_remake.Services;

public interface IUserProgressService
{
    Task<UserProgressDto?> GetUserProgressAsync(int id);
}
public class UserProgressService : IUserProgressService
{
    private readonly Ff7DbContext context;
    public UserProgressService(Ff7DbContext context)
    {
        this.context = context;
    }

    public async Task<UserProgressDto?> GetUserProgressAsync(int id)
    {
        var userprog = await context.UserProgresses
            .Include(u => u.Chapter)
            .FirstOrDefaultAsync(u => u.UserId == id);

        if (userprog == null)
        {
            return null;
        }

        return new UserProgressDto
        {
            ChapterId = userprog.ChapterId,
            LastUpdated = userprog.LastUpdated,
            isCompleted = userprog.IsCompleted,
        };
    }
    public async Task<double> CalculateUserProgress(int userId)
    {
        var totalChapeters = await context.Chapters.CountAsync();
        var completedChapters = await context.UserProgresses.CountAsync(up => up.UserId == userId && up.IsCompleted);
        if (totalChapeters == 0) return 0;
        return (double)completedChapters / totalChapeters * 100;
    }
}