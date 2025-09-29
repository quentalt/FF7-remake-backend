using FF7_remake.DTOs;
using FF7_remake.Services;
using Microsoft.AspNetCore.Mvc;

namespace FF7_remake.Controllers;
[ApiController]
[Route("api/[controller]")]

public class UserProgressController:ControllerBase
{
    private readonly IUserProgressService _userProgressService;
    
    public UserProgressController(IUserProgressService userProgressService)
    {
        _userProgressService = userProgressService;
    }
    
    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<UserProgressDto>>>> GetAllUserProgress()
    {
        try
        {
            var userProgressList = await _userProgressService.GetAllUserProgressAsync();
            return Ok(new ApiResponse<List<UserProgressDto>>
            {
                Success = true,
                Message = "User progress records retrieved successfully",
                Data = userProgressList
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<List<UserProgressDto>>
            {
                Success = false,
                Message = "An error occurred while retrieving user progress records",
                Errors = [ex.Message]
            });
        }
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<UserProgressDto>>> GetUserProgressByUserId(int id)
    {
        try
        {
            var userProgress = await _userProgressService.GetUserProgressByUserIdAsync(id);
            if (userProgress == null)
            {
                return NotFound(new ApiResponse<UserProgressDto>
                {
                    Success = false,
                    Message = "User progress not found"
                });
            }

            return Ok(new ApiResponse<UserProgressDto>
            {
                Success = true,
                Message = "User progress retrieved successfully",
                Data = userProgress
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<UserProgressDto>
            {
                Success = false,
                Message = "An error occurred while retrieving user progress",
                Errors = [ex.Message]
            });
        }
    }
    
    /*
    [HttpGet("user/{userId}/chapter/{chapterId}")]
    public async Task<ActionResult<ApiResponse<UserProgressDto>>> GetUserProgressByUserAndChapter(int userId, int chapterId)
    {
        try
        {
            var userProgress = await _userProgressService.GetUserProgressByUserAndChapterAsync(userId, chapterId);
            if (userProgress == null)
            {
                return NotFound(new ApiResponse<UserProgressDto>
                {
                    Success = false,
                    Message = "User progress not found for the specified user and chapter"
                });
            }

            return Ok(new ApiResponse<UserProgressDto>
            {
                Success = true,
                Message = "User progress retrieved successfully",
                Data = userProgress
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<UserProgressDto>
            {
                Success = false,
                Message = "An error occurred while retrieving user progress",
                Errors = [ex.Message]
            });
        }
    }
    */
    
    [HttpPost]
    public async Task<ActionResult<ApiResponse<UserProgressDto>>> CreateOrUpdateUserProgress([FromBody] CreateUserProgressDto createUserProgressDto)
    {
        try
        {
            var userProgress = await _userProgressService.CreateOrUpdateUserProgressAsync(createUserProgressDto);
            return Ok(new ApiResponse<UserProgressDto>
            {
                Success = true,
                Message = "User progress created/updated successfully",
                Data = userProgress
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<UserProgressDto>
            {
                Success = false,
                Message = "An error occurred while creating/updating user progress",
                Errors = [ex.Message]
            });
        }
    }
    
    
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteUserProgress(int id)
    {
        try
        {
            var success = await _userProgressService.DeleteUserProgressAsync(id);
            if (!success)
            {
                return NotFound(new ApiResponse<bool>
                {
                    Success = false,
                    Message = "User progress not found"
                });
            }

            return Ok(new ApiResponse<bool>
            {
                Success = true,
                Message = "User progress deleted successfully",
                Data = true
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<bool>
            {
                Success = false,
                Message = "An error occurred while deleting user progress",
                Errors = [ex.Message]
            });
        }
    }
}