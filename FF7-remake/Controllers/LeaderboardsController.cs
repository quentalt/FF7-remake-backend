using FF7_remake.DTOs;
using FF7_remake.Services;
using Microsoft.AspNetCore.Mvc;

namespace FF7_remake.Controllers;
[ApiController]

public class LeaderboardsController : Controller
{
    private readonly ILeaderboardService _leaderboardService;
    // GET
    public async Task<ActionResult<ApiResponse<LeaderboardDto>>> CreateLeaderboard(CreateLeaderboardDto createLeaderboardDto)
    {
        try
        {
            var leaderboard = await _leaderboardService.CreateLeaderboardDtoAsync(createLeaderboardDto);
            return CreatedAtAction(nameof(GetLeaderboard), routeValues: new { id = leaderboard.LeaderboardId }, new ApiResponse<LeaderboardDto>
            {
                Success ="true",
                Message =" leaderboard created successfully",
                Data = leaderboard     
            });

        }
        catch (Exception ex )
        {
            return StatusCode(500, new ApiResponse<LeaderboardDto>()
            {
                Success = false,
                Message = "Error creating leaderboard",
                Errors = [ex.Message]

            });
            Console.WriteLine();
            throw;
        }
    }


    [HttpPut(template: "{id }")]

    public async Task<ActionResult<ApiResponse<LeaderboardDto>>> UpdateLeaderboard(int id,
        CreateLeaderboardDto updateLeaderboardDto)
    {
        try
        {
            var leaderboard =  await _leaderboardService.UpdateLeaderboardDtoAsync(id, updateLeaderboardDto);
            if (leaderboard == null)
            {
                return NotFound(new ApiResponse<LeaderboardDto>
                {
                    Success = false,
                    Message = "Error updating leaderboard",

                });

            }

            return Ok(new ApiResponse<LeaderboardDto>
                {
                    Success = true,
                    Message = "leaderboard updated successfully",
                    Data = leaderboard

                }
            );

        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<LeaderboardDto>
                {
                    Success = false,
                    Message = "An error occured while updating leaderboard",
                    Errors = [ex.Message]
                }
            );

        }
    }
}