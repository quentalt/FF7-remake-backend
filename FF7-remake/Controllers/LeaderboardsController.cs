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
    public async Task<IActionResult> GetLeaderboard([FromQuery] int limit = 100)
    {
        var leaderboard = await _leaderboardService.GetLeaderboardAsync(limit);
        return Ok(leaderboard);
    }
    
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserLeaderboardEntries(int userId)
    {
        var entries = await _leaderboardService.GetUserLeaderboardEntriesAsync(userId);
        return Ok(entries);
    }
    
    [HttpPost("user/{userId}")]
    public async Task<IActionResult> CreateLeaderboardEntry(int userId, [FromBody] CreateLeaderboardDto createLeaderboardDto)
    {
        var entry = await _leaderboardService.CreateLeaderboardEntryAsync(userId, createLeaderboardDto);
        return CreatedAtAction(nameof(GetUserLeaderboardEntries), new { userId = userId }, entry);
    }
    
    [HttpPost("initialize")]
    public async Task<IActionResult> InitializeLeaderboard([FromBody] InitializeLeaderboardDto initializeLeaderboardDto)
    {
        var entries = await _leaderboardService.InitializeLeaderboardAsync(initializeLeaderboardDto);
        return Ok(entries);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLeaderboardEntry(int id)
    {
        var success = await _leaderboardService.DeleteLeaderboardEntryAsync(id);
        if (!success)
        {
            return NotFound();
        }
        return NoContent();
    }
    
    [HttpGet("user/{userId}/rank")]
    public async Task<IActionResult> GetUserRank(int userId)
    {
        var rank = await _leaderboardService.GetUserRankAsync(userId);
        return Ok(new { UserId = userId, Rank = rank });
    }
}