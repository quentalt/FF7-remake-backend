using FF7_remake.DTOs;
using FF7_remake.Services;
using Microsoft.AspNetCore.Mvc;

namespace FF7_remake.Controllers;
[ApiController]
[Route("api/[controller]")]


public class LeaderboardController: ControllerBase
{
    private readonly ILeaderboardService _leaderboardService;
    
    public LeaderboardController(ILeaderboardService leaderboardService)
    {
        _leaderboardService = leaderboardService;
    }
    
    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<LeaderboardDto>>>> GetLeaderboards()
    {
        try
        {
            var leaderboards = await _leaderboardService.GetAllLeaderboard();
            return Ok(new ApiResponse<List<LeaderboardDto>>
            {
                Success = true,
                Message = "Leaderboards retrieved successfully",
                Data = leaderboards
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<List<LeaderboardDto>>
            {
                Success = false,
                Message = "An error occurred while retrieving leaderboards",
                Errors = [ex.Message]
            });
        }
    }
    
    [HttpGet ("{id}")]
    public async Task<ActionResult<ApiResponse<LeaderboardDto>>> GetLeaderboard(int id)
    {
        try
        {
            var leaderboard = await _leaderboardService.GetLeaderboardByIdAsync(id);
            if (leaderboard == null)
            {
                return NotFound(new ApiResponse<LeaderboardDto>
                {
                    Success = false,
                    Message = "Leaderboard not found"
                });
            }

            return Ok(new ApiResponse<LeaderboardDto>
            {
                Success = true,
                Message = "Leaderboard retrieved successfully",
                Data = leaderboard
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<LeaderboardDto>
            {
                Success = false,
                Message = "An error occurred while retrieving the leaderboard",
                Errors = [ex.Message]
            });
        }
    }
}