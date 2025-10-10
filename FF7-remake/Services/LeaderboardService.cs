  using FF7_remake.DBContext;
  using FF7_remake.DTOs;
  using FF7_remake.Models;
  using Microsoft.EntityFrameworkCore;
  
  namespace FF7_remake.Services;

  public interface ILeaderboardService
    {
        Task<List<LeaderboardDto>> GetLeaderboardAsync(int limit = 100);
        Task<List<LeaderboardDto>> GetLeaderboardById(int userId);
        Task<LeaderboardDto> CreateLeaderboardEntryAsync(int userId, CreateLeaderboardDto createLeaderboardDto);
        Task<List<LeaderboardDto>> InitializeLeaderboardAsync(InitializeLeaderboardDto initializeLeaderboardDto);
        Task<bool> DeleteLeaderboardEntryAsync(int id);
        Task<int> GetUserRankAsync(int userId);
    }

  public class LeaderboardService : ILeaderboardService
  {
      private readonly Ff7DbContext _context;

      public LeaderboardService(Ff7DbContext context)
      {
          _context = context;
      }

      public async Task<List<LeaderboardDto>> GetLeaderboardAsync(int limit = 100)
      {
          return await _context.Leaderboards
              .Include(l => l.User)
              .OrderByDescending(l => l.Score)
              .Take(limit)
              .Select(l => new LeaderboardDto
              {
                  LeaderboardId = l.LeaderBoardId,
                  Score = l.Score,
                  Ranking = l.Ranking,
                  AchievedAt = l.AchievedAt,
                  UserName = l.User.Username
              }).ToListAsync();
      }

      public async Task<List<LeaderboardDto>> GetLeaderboardById(int userId)
      {
          return await _context.Leaderboards
              .Include(l => l.User)
              .Where(l => l.UserId == userId)
              .OrderByDescending(l => l.Score)
              .Select(l => new LeaderboardDto
              {
                  LeaderboardId = l.LeaderBoardId,
                  Score = l.Score,
                  Ranking = l.Ranking,
                  AchievedAt = l.AchievedAt,
                  UserName = l.User.Username
              }).ToListAsync();
      }

      public async Task<LeaderboardDto> CreateLeaderboardEntryAsync(int userId,
          CreateLeaderboardDto createLeaderboardDto)
      {
          // Vérifier si l'utilisateur existe
          var user = await _context.Users.FindAsync(userId);
          if (user == null)
          {
              throw new Exception($"User with ID {userId} not found");
          }

          var leaderboardEntry = new Leaderboard
          {
              UserId = userId,
              Score = createLeaderboardDto.Score,
              Ranking = 0, 
              AchievedAt = DateTime.UtcNow
          };

          _context.Leaderboards.Add(leaderboardEntry);
          await _context.SaveChangesAsync();

          var entry = await _context.Leaderboards
              .Include(l => l.User)
              .FirstOrDefaultAsync(l => l.LeaderBoardId == leaderboardEntry.LeaderBoardId);

          return new LeaderboardDto
          {
              LeaderboardId = entry!.LeaderBoardId,
              Score = entry.Score,
              Ranking = entry.Ranking,
              AchievedAt = entry.AchievedAt,
              UserName = entry.User.Username
          };
      }

      public async Task<List<LeaderboardDto>> InitializeLeaderboardAsync(
          InitializeLeaderboardDto initializeLeaderboardDto)
      {
          var createdEntries = new List<LeaderboardDto>();

          foreach (var userId in initializeLeaderboardDto.UserIds)
          {
              // Vérifier si l'utilisateur existe
              var user = await _context.Users.FindAsync(userId);
              if (user == null)
              {
                  continue; // Ignorer les utilisateurs qui n'existent pas
              }

              // Vérifier si l'utilisateur n'a pas déjà une entrée dans le leaderboard
              var existingEntry = await _context.Leaderboards
                  .FirstOrDefaultAsync(l => l.UserId == userId);

              if (existingEntry == null)
              {
                  // Créer une entrée vierge
                  var leaderboardEntry = new Leaderboard
                  {
                      UserId = userId,
                      Score = 0,
                      Ranking = 0,
                      AchievedAt = DateTime.UtcNow
                  };

                  _context.Leaderboards.Add(leaderboardEntry);
                  await _context.SaveChangesAsync();

                  createdEntries.Add(new LeaderboardDto
                  {
                      LeaderboardId = leaderboardEntry.LeaderBoardId,
                      Score = leaderboardEntry.Score,
                      Ranking = leaderboardEntry.Ranking,
                      AchievedAt = leaderboardEntry.AchievedAt,
                      UserName = user.Username
                  });
              }
              else
              {
                  // Retourner l'entrée existante
                  createdEntries.Add(new LeaderboardDto
                  {
                      LeaderboardId = existingEntry.LeaderBoardId,
                      Score = existingEntry.Score,
                      Ranking = existingEntry.Ranking,
                      AchievedAt = existingEntry.AchievedAt,
                      UserName = user.Username
                  });
              }
          }

          return createdEntries;
      }

      public async Task<bool> DeleteLeaderboardEntryAsync(int id)
      {
          var entry = await _context.Leaderboards.FindAsync(id);
          if (entry == null) return false;

          _context.Leaderboards.Remove(entry);
          await _context.SaveChangesAsync();
          return true;
      }

      public async Task<int> GetUserRankAsync(int userId)
      {
          var userEntry = await _context.Leaderboards
              .Where(l => l.UserId == userId)
              .OrderByDescending(l => l.Score)
              .FirstOrDefaultAsync();

          if (userEntry == null) return -1; 

          var rank = await _context.Leaderboards
              .CountAsync(l => l.Score > userEntry.Score) + 1;

          return rank;
      }
  }