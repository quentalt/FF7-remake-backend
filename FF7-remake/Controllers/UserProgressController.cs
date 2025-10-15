using FF7_remake.DTOs;
using FF7_remake.Services;
using Microsoft.AspNetCore.Mvc;

namespace FF7_remake.Controllers;
[ApiController]
public class UserProgressController : ControllerBase
{
    private readonly IUserProgressService _progressService;
    
    public UserProgressController(IUserProgressService progressService) 
    {
        _progressService = progressService;
    }
    
    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<UserProgressDto>>>> GetAllUserProgress()
    {
        try
        {
            var progress = await _progressService.GetAllUserProgressDtoAsync();
            return Ok(new ApiResponse<List<UserProgressDto>>
            {
                Success = true,
                Message = " User progress retrieved successfully",
                Data = progress

            });

        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<List<UserProgressDto>>
            {
                Success = false,
                Message = "An error occured while retrieving user progress",
                Errors = [ex.Message]
            });
        }
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<UserProgressDto>>> GetUserProgress(int id)
    {
        try
        {
            var progress = await _progressService.GetUserProgressDtoAsync(id);
            if (progress == null)
            {
                return NotFound(new ApiResponse<UserProgressDto>
                {
                    Success = false,
                    Message = "User progress not found",

                });
                
            }

            return Ok(new ApiResponse<UserProgressDto>
            {
                Success = true,
                Message = "User progress retrieved successfully",
                Data = progress
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<UserProgressDto>
            {
                Success = false,
                Message = "An error occured while retrieving user progress",
                Errors = [ex.Message]
            });
        }
    }
    
    [HttpPost]
    public async Task<ActionResult<ApiResponse<UserProgressDto>>> CreateUserProgress(CreateUserProgressDto createUserProgressDto)
    {
        try
        {
            var progress = await _progressService.CreateUserProgressDtoAsync(createUserProgressDto);
            return Ok(new ApiResponse<UserProgressDto>
            {
                Success = true,
                Message = " User progress created successfully",
                Data = progress

            });

        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<UserProgressDto>
            {
                Success = false,
                Message = "An error occured while creating user progress",
                Errors = [ex.Message]
            });
        }
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<UserProgressDto>>> UpdateUserProgress(int id, CreateUserProgressDto updateUserProgressDto)
    {
        try
        {
            var progress = await _progressService.UpdateUserProgressDtoAsync(id, updateUserProgressDto);
            if (progress == null)
            {
                return NotFound(new ApiResponse<UserProgressDto>
                {
                    Success = false,
                    Message = "User progress not found",

                });
                
            }

            return Ok(new ApiResponse<UserProgressDto>
            {
                Success = true,
                Message = "User progress updated successfully",
                Data = progress
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<UserProgressDto>
            {
                Success = false,
                Message = "An error occured while updating user progress",
                Errors = [ex.Message]
            });
        }
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<string>>> DeleteUserProgress(int id)
    {
        try
        {
            var deleted = await _progressService.DeleteUserProgressDtoAsync(id);
            if (!deleted)
            {
                return NotFound(new ApiResponse<string>
                {
                    Success = false,
                    Message = "User progress not found",

                });
                
            }

            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "User progress deleted successfully",
                Data = null
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<string>
            {
                Success = false,
                Message = "An error occured while deleting user progress",
                Errors = [ex.Message]
            });
        }
    }

}