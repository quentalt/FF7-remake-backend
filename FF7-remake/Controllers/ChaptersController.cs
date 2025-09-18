using FF7_remake.DTOs;
using FF7_remake.Models;
using FF7_remake.Services;
using Microsoft.AspNetCore.Mvc;

namespace FF7_remake.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChaptersController : ControllerBase
{

    private readonly IChapterService _chapterService;

    public ChaptersController(IChapterService chapterService)
    {
        _chapterService = chapterService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<ChapterDto>>>> GetChapters()
    {

        try
        {
            var chapters = await _chapterService.GetAllChaptersAsync();
            return Ok(new ApiResponse<List<ChapterDto>>
            {
                Success = true,
                Message = "Chapters retrieved successfully",
                Data = chapters
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<List<ChapterDto>>
            {
                Success = false,
                Message = "An error occurred while retrieving chapters",
                Errors = [ex.Message]
            });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<ChapterDto>>> GetChapter(int id)
    {
        try
        {
            var chapter = await _chapterService.GetChapterByIdAsync(id);

            if (chapter == null)
            {
                return NotFound(new ApiResponse<ChapterDto>
                {
                    Success = false,
                    Message = "Chapter not found"
                });
            }

            return Ok(new ApiResponse<ChapterDto>
            {
                Success = true,
                Message = "Chapter retrieved successfully",
                Data = chapter
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<ChapterDto>
            {
                Success = false,
                Message = "An error occurred while retrieving the chapter",
                Errors = [ex.Message]
            });
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<ChapterDto>>> CreateChapter(CreateChapterDto createChapterDto)
    {
        try
        {
            var chapter = await _chapterService.CreateChapterAsync(createChapterDto);
            return CreatedAtAction(nameof(GetChapter), new { id = chapter.ChapterId }, new ApiResponse<ChapterDto>
            {
                Success = true,
                Message = "Chapter created successfully",
                Data = chapter
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<ChapterDto>
            {
                Success = false,
                Message = "An error occurred while creating the chapter",
                Errors = [ex.Message]
            });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<ChapterDto>>> UpdateChapter(int id,
        CreateChapterDto updateChapterDto)
    {
        try
        {
            var chapter = await _chapterService.UpdateChapterAsync(id, updateChapterDto);
            if (chapter == null)
            {
                return NotFound(new ApiResponse<ChapterDto>
                {
                    Success = false,
                    Message = "Chapter not found"
                });
            }

            return Ok(new ApiResponse<ChapterDto>
            {
                Success = true,
                Message = "Chapter updated successfully",
                Data = chapter
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<ChapterDto>
            {
                Success = false,
                Message = "An error occurred while updating the chapter",
                Errors = [ex.Message]
            });
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteChapter(int id)
    {
        try
        {
            var chapter = await _chapterService.DeleteChapterAsync(id);
            if (!chapter)
            {
                return NotFound(new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Chapter not found",
                    Data = false
                });
            }



            return Ok(new ApiResponse<bool>
            {
                Success = true,
                Message = "Chapter deleted successfully",
                Data = true
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<bool>
            {
                Success = false,
                Message = "An error occurred while deleting the chapter",
                Errors = [ex.Message],
                Data = false
            });
        }
    }
}