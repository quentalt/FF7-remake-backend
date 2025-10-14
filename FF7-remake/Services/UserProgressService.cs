using FF7_remake.DBContext;
using FF7_remake.DTOs;
using FF7_remake.Models;
using Microsoft.EntityFrameworkCore;


namespace FF7_remake.Services;

public interface IUserProgressService
{
    Task<UserProgressDto?> GetUserProgressDtoAsync(int id);
    Task<List<UserProgressDto>> GetAllUserProgressDtoAsync();
    Task<UserProgressDto> CreateUserProgressDtoAsync(CreateUserProgressDto createUserProgressDto);
    Task<UserProgressDto?> UpdateUserProgressDtoAsync(int id, CreateUserProgressDto updateProgressDto);
    
    Task<bool> DeleteUserProgressDtoAsync(int id);
    Task<UserProgressDto> CalculateUserProgressAsync(int id);
}

public class UserProgressService : IUserProgressService
{
    private readonly Ff7DbContext context;
    public UserProgressService(Ff7DbContext context)
    {
        this.context = context;
    }
    public async Task<UserProgressDto?> GetUserProgressDtoAsync(int id)
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
            SavedState = userprog.SavedState,
        };

    }

    public async Task<List<UserProgressDto>> GetAllUserProgressDtoAsync()
    {
        return await context.UserProgresses
            .Include(u => u.User )
            .Select(u => new UserProgressDto
            {
               
                UserName = u.User.Username,
                UserId = u.UserId,
                ChapterId = u.ChapterId,
                ChapterTitle = u.Chapter.Title,
                LastUpdated = u.LastUpdated
                
                
            }).ToListAsync();
        
                    
          
        
    }



    public async Task<UserProgressDto> CreateUserProgressDtoAsync(CreateUserProgressDto createUserProgressDto)
    {
        var userProgress = new UserProgress
        {
             SavedState = createUserProgressDto.SavedState,
             UserId = createUserProgressDto.UserId,
             ChapterId = createUserProgressDto.ChapterId
        };

        context.UserProgresses.Add(userProgress);
        await context.SaveChangesAsync();

        return new UserProgressDto
        {
            ChapterId = userProgress.ChapterId,
            SavedState = userProgress.SavedState,
            LastUpdated = userProgress.LastUpdated,
            UserId = userProgress.UserId,
            ChapterTitle = userProgress.Chapter.Title
           

        };
        
    }

    
        
        
    

    public async Task<UserProgressDto?> UpdateUserProgressDtoAsync(int id, CreateUserProgressDto updateUserProgressDto)
    {
        
        var userprog = await context.UserProgresses.FindAsync(id);
           

        if (userprog == null) return null;
        userprog.SavedState = updateUserProgressDto.SavedState;

      


        await context.SaveChangesAsync();

        return new UserProgressDto
        {
            ChapterId = userprog.ChapterId,
            SavedState = userprog.SavedState,
            LastUpdated = userprog.LastUpdated,
            UserId = userprog.UserId,
            ChapterTitle = userprog.Chapter.Title
          

        };
        


        

    }

    public async Task<bool> DeleteUserProgressDtoAsync(int id)
    {
        var userprog = await context.UserProgresses.FindAsync(id);

        if (userprog == null)
        {
            return false;
        }
        
        context.UserProgresses.Remove(userprog);

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<UserProgressDto> CalculateUserProgressAsync(int id)
    {
        var totalsChapters = await context.Chapters.CountAsync();
        
       var CompletedChapters = await context.UserProgresses
           .CountAsync(u => u.UserId == id && u.SavedState == "completed");


       double calculate = CompletedChapters / totalsChapters * 100;

       return new UserProgressDto
       {
           UserId = id,
           SavedState = "completed",
           ProgressPercentage = calculate
       };


    }



}