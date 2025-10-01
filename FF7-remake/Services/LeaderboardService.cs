using FF7_remake.DBContext;
using FF7_remake.DTOs;
using Microsoft.EntityFrameworkCore;

namespace FF7_remake.Services;

public interface ILeaderboardService
{
 Task<List<LeaderboardDto>> GetLeaderBoardAsync(int limit = 100);
 Task<List<LeaderboardDto>>GetUserLeaderboardEntriesAsync(int userId);

 Task<LeaderboardDto> CreateLeaderboardEntryAsync(CreateLeaderboardDto createLeaderboardDto);
 Task<bool> DeleteLeaderboardEntryAsync(int id);
 
 Task<int> GetUserRankAsync(int userId);
 
}
public class LeaderboardService
{

 public class LeaderboardService : ILeaderboardService
 {
  private readonly Ff7DbContext _context;

  public LeaderboardService(Ff7DbContext context)
  {
   _context = context;
  }

  public async Task<List<LeaderboardDto>> GetLeaderBoardAsync(int limit = 100)
  {
   return await _context.Leaderboards
    .Include(l => l.User)
    .OrderByDescending(l => l.Score)
    .Take(limit)
    .Select(l => new LeaderboardDto
    {
     LeaderboardId = l.LeaderBoardId,
     UserId = l.UserId,
     Score = l.Score,
     Ranking = l.Ranking,
     AchievedAt = l.AchievedAt,
     UserName = l.User.Username
    }).ToListAsync();
  }
  
  // continuer ici les endpoints
  
 }
    
}