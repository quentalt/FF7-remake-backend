using FF7_remake.DBContext;
using FF7_remake.DTOs;
using FF7_remake.Models;
using Microsoft.EntityFrameworkCore;

namespace FF7_remake.Services;

public interface ILeaderboardService
{
    Task<List<LeaderboardDto>> GetAllLeaderboard();
    Task<LeaderboardDto?> GetLeaderboardByIdAsync(int id);
    Task<LeaderboardDto> CreateLeaderboardDtoAsync(CreateLeaderboardDto createLeaderboardDto);
    Task<LeaderboardDto?> UpdateLeaderboardDtoAsync(int id, CreateLeaderboardDto updateLeaderboardDto);
    Task<bool> DeleteLeaderboardByIdAsync(int id);
}
public class LeaderboardService : ILeaderboardService
{

    private readonly Ff7DbContext context;
    public LeaderboardService(Ff7DbContext context)
    {
        this.context = context;
    }
    
    public async Task<List<LeaderboardDto>> GetAllLeaderboard()
    {
        return await context.Leaderboards
            .Include(l => l.User )
            .Select(l => new LeaderboardDto
            {
                LeaderboardId = l.LeaderBoardId,
                Score = l.Score,
                Ranking = l.Ranking,
                AchievedAt = l.AchievedAt,
                UserName = l.User.Username,
                UserId = l.UserId
                
            }).ToListAsync();
        
                    
          
        
    }
    
    public async Task<LeaderboardDto?> GetLeaderboardByIdAsync( int id)

    {
        var leaderboard = await context.Leaderboards
            .Include(l => l.UserId)
            .FirstOrDefaultAsync(l => l.LeaderBoardId == id);

        if (leaderboard == null)
        {
            return null;
        }

        return new LeaderboardDto
        {
            LeaderboardId = leaderboard.LeaderBoardId,
            Score = leaderboard.Score,
            Ranking = leaderboard.Ranking,
            AchievedAt = leaderboard.AchievedAt,
            UserName = leaderboard.User.Username,
            UserId = leaderboard.UserId

        };


    }

    public async Task<LeaderboardDto> CreateLeaderboardDtoAsync(CreateLeaderboardDto createLeaderboardDto)
    {
        var leaderboard = new Leaderboard
        {
            Score = createLeaderboardDto.Score,
            Ranking = createLeaderboardDto.Ranking,
            UserId = createLeaderboardDto.UserId,
        };

        context.Leaderboards.Add(leaderboard);
        await context.SaveChangesAsync();

        return new LeaderboardDto
        {
            LeaderboardId = leaderboard.LeaderBoardId,
            Score = leaderboard.Score,
            Ranking = leaderboard.Ranking,
            AchievedAt = leaderboard.AchievedAt,
            UserId = leaderboard.UserId

        };
        
    }

    public async Task<LeaderboardDto?> UpdateLeaderboardDtoAsync(int id, CreateLeaderboardDto updateLeaderboardDto)
    {
        
        var leaderboard = await context.Leaderboards.FindAsync(id);
           

        if (leaderboard == null) return null;

        leaderboard.Ranking = updateLeaderboardDto.Ranking;
        leaderboard.Score = updateLeaderboardDto.Score;


        await context.SaveChangesAsync();

        return new LeaderboardDto()
        {
            LeaderboardId = leaderboard.LeaderBoardId,
            Score = leaderboard.Score,
            Ranking = leaderboard.Ranking,
            AchievedAt = leaderboard.AchievedAt,

        };
        


        

    }

    public async Task<bool> DeleteLeaderboardByIdAsync(int id)
    {
        var leaderboard = await context.Leaderboards.FindAsync(id);

        if (leaderboard == null)
        {
            return false;
        }
        
        context.Leaderboards.Remove(leaderboard);

        await context.SaveChangesAsync();
        return true;
    }




}