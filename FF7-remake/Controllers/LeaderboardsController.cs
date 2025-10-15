using FF7_remake.DTOs;
using FF7_remake.Services;
using Microsoft.AspNetCore.Mvc;

namespace FF7_remake.Controllers;
[ApiController]
[Route("api/[controller]")]

public class LeaderboardsController : ControllerBase
{
    private readonly ILeaderboardService _leaderboardService;
    
    public LeaderboardsController(ILeaderboardService leaderboardService)
    {
        _leaderboardService = leaderboardService;
    }
    
    [HttpGet]
    public async Task<ActionResult> GetLeaderboard([FromQuery] int limit = 100)
    {
        try
        {
            var leaderboard = await _leaderboardService.GetLeaderboardAsync(limit);
            return Ok(new ApiResponse<List<LeaderboardDto>>
            {
                Success = true,
                Message = "Leaderboard retrieved successfully",
                Data = leaderboard
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<List<LeaderboardDto>>
            {
                Success = false,
                Message = "An error occurred while retrieving the leaderboard",
                Errors = [ex.Message]
            });
        }
    }
    
    [HttpGet("user/{userId}")]
    public async Task<ActionResult> GetUserLeaderboardEntries(int userId)
    {
      try 
      {
        var entries = await _leaderboardService.GetLeaderboardById(userId);
        if (entries == null || !entries.Any())
        {
            return NotFound(new ApiResponse<List<LeaderboardDto>>
            {
                Success = false,
                Message = "No leaderboard entries found for the user",
                Data = null
            });
        }
        
        return Ok(new ApiResponse<List<LeaderboardDto>>
        {
            Success = true,
            Message = "User leaderboard entries retrieved successfully",
            Data = entries
        });
      }
      catch (Exception ex)
      {
        return StatusCode(500, new ApiResponse<List<LeaderboardDto>>
        {
            Success = false,
            Message = "An error occurred while retrieving user leaderboard entries",
            Errors = [ex.Message]
        });
      }
    }
    
    [HttpPost("user/{userId}")]
    public async Task<ActionResult> CreateLeaderboardEntry(int userId, [FromBody] CreateLeaderboardDto createLeaderboardDto)
    {
        try
        {
            var entry = await _leaderboardService.CreateLeaderboardEntryAsync(userId, createLeaderboardDto);
            return CreatedAtAction(nameof(GetUserLeaderboardEntries), new { userId = userId }, entry);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<LeaderboardDto>
            {
                Success = false,
                Message = "An error occurred while creating the leaderboard entry",
                Errors = [ex.Message]
            });
        }
    }
    
    [HttpPost("initialize")]
    public async Task<ActionResult> InitializeLeaderboard([FromBody] InitializeLeaderboardDto initializeLeaderboardDto)
    {
        try
        {
            await _leaderboardService.InitializeLeaderboardAsync(initializeLeaderboardDto);
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Leaderboard initialized successfully",
                Data = null
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<string>
            {
                Success = false,
                Message = "An error occurred while initializing the leaderboard",
                Errors = [ex.Message]
            });
        }
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteLeaderboardEntry(int id)
    {
        try
        {
            var success = await _leaderboardService.DeleteLeaderboardEntryAsync(id);
            if (!success)
            {
                return NotFound(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Leaderboard entry not found",
                    Data = null
                });
            }

            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Leaderboard entry deleted successfully",
                Data = null
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<string>
            {
                Success = false,
                Message = "An error occurred while deleting the leaderboard entry",
                Errors = [ex.Message]
            });
        }
    }
    
    [HttpGet("user/{userId}/rank")]
    public async Task<ActionResult> GetUserRank(int userId)
    {
        try
        {
            var rank = await _leaderboardService.GetUserRankAsync(userId);
            if (rank == -1)
            {
                return NotFound(new ApiResponse<int>
                {
                    Success = false,
                    Message = "User leaderboard entry not found",
                    Data = -1
                });
            }

            return Ok(new ApiResponse<int>
            {
                Success = true,
                Message = "User rank retrieved successfully",
                Data = rank
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<int>
            {
                Success = false,
                Message = "An error occurred while retrieving the user rank",
                Errors = [ex.Message]
            });
        }
    }
}