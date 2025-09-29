using FF7_remake.DBContext;
using FF7_remake.DTOs;
using FF7_remake.Models;
using Microsoft.EntityFrameworkCore;

namespace FF7_remake.Services;

public interface IUserProgressService
{
    Task<List<UserProgressDto>> GetAllUserProgressAsync();
    Task<UserProgressDto?> GetUserProgressByUserIdAsync(int id);
    Task<UserProgressDto> CreateOrUpdateUserProgressAsync(CreateUserProgressDto createUserProgressDto);
    Task<bool> DeleteUserProgressAsync(int id);
   
}

public class UserProgressService : IUserProgressService
{
    private readonly Ff7DbContext _context;

    public UserProgressService(Ff7DbContext context)
    {
        _context = context;
    }

    public async Task<List<UserProgressDto>> GetAllUserProgressAsync()
    {
        return await _context.UserProgresses
            .Include(up => up.User)
            .Include(up => up.Chapter)
            .Select(up => new UserProgressDto
            {
                Id = up.Id,
                UserId = up.UserId,
                ChapterId = up.ChapterId,
                SavedState = up.SavedState,
                LastUpdated = up.LastUpdated,
                UserName = up.User.Username,
                ChapterTitle = up.Chapter.Title
            }).ToListAsync();
    }

    public async Task<UserProgressDto?> GetUserProgressByUserIdAsync(int id)
    {
        var userProgress = await _context.UserProgresses
            .Include(up => up.User)
            .Include(up => up.Chapter)
            .FirstOrDefaultAsync(up => up.Id == id);
        if (userProgress == null) return null;

        return new UserProgressDto
        {
            Id = userProgress.Id,
            UserId = userProgress.UserId,
            ChapterId = userProgress.ChapterId,
            SavedState = userProgress.SavedState,
            LastUpdated = userProgress.LastUpdated,
            UserName = userProgress.User.Username,
            ChapterTitle = userProgress.Chapter.Title
        };
    }

    public async Task<UserProgressDto> CreateOrUpdateUserProgressAsync(CreateUserProgressDto createUserProgressDto)
    {
        var existingProgress = await _context.UserProgresses
            .FirstOrDefaultAsync(up =>
                up.UserId == createUserProgressDto.UserId && up.ChapterId == createUserProgressDto.ChapterId);

        if (existingProgress != null)
        {
            existingProgress.SavedState = createUserProgressDto.SavedState;
            existingProgress.LastUpdated = DateTime.Now;
            await _context.SaveChangesAsync();

            var updatedProgress = await _context.UserProgresses
                .Include(up => up.User)
                .Include(up => up.Chapter)
                .FirstOrDefaultAsync(up => up.Id == existingProgress.Id);

            return new UserProgressDto
            {
                Id = updatedProgress!.Id,
                UserId = updatedProgress.UserId,
                ChapterId = updatedProgress.ChapterId,
                SavedState = updatedProgress.SavedState,
                LastUpdated = updatedProgress.LastUpdated,
                UserName = updatedProgress.User.Username,
                ChapterTitle = updatedProgress.Chapter.Title
            };
        }
        else
        {
            var userProgress = new UserProgress
            {
                UserId = createUserProgressDto.UserId,
                ChapterId = createUserProgressDto.ChapterId,
                SavedState = createUserProgressDto.SavedState,
                LastUpdated = DateTime.UtcNow
            };

            _context.UserProgresses.Add(userProgress);
            await _context.SaveChangesAsync();

            var newProgress = await _context.UserProgresses
                .Include(up => up.User)
                .Include(up => up.Chapter)
                .FirstOrDefaultAsync(up => up.Id == userProgress.Id);

            return new UserProgressDto
            {
                Id = newProgress!.Id,
                UserId = newProgress.UserId,
                ChapterId = newProgress.ChapterId,
                SavedState = newProgress.SavedState,
                LastUpdated = newProgress.LastUpdated,
                UserName = newProgress.User.Username,
                ChapterTitle = newProgress.Chapter.Title
            };
        }
    }

    public async Task<bool> DeleteUserProgressAsync(int id)
    {
        var userProgress = await _context.UserProgresses.FindAsync(id);
        if (userProgress == null) return false;
        _context.UserProgresses.Remove(userProgress);
        await _context.SaveChangesAsync();
        return true;
    }
}
    
   