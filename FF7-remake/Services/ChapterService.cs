using FF7_remake.DBContext;
using FF7_remake.DTOs;
using FF7_remake.Models;
using Microsoft.EntityFrameworkCore;

namespace FF7_remake.Services;

public interface IChapterService
{
    Task<List<ChapterDto>> GetAllChaptersAsync();
    Task<ChapterDto?> GetChapterByIdAsync(int id);
    Task<ChapterDto> CreateChapterAsync(CreateChapterDto createChapterDto);
    Task<ChapterDto?> UpdateChapterAsync(int id, CreateChapterDto updateChapterDto);
    Task<bool> DeleteChapterAsync(int id);
}

public class ChapterService : IChapterService
{
    private readonly Ff7DbContext _context;
    
    public ChapterService(Ff7DbContext context)
    {
        _context = context;
    }

    public async Task<List<ChapterDto>> GetAllChaptersAsync()
    {
        return await _context.Chapters
            .Include(c => c.Quizzes)
            .Select(c => new ChapterDto
            {
                ChapterId = c.ChapterId,
                Title = c.Title,
                Summary = c.Summary,
                Quiz = c.Quiz,
                Quizzes = c.Quizzes.Select(q => new QuizDto
                {
                    QuizId = q.QuizId,
                    ChapterId = q.ChapterId,
                    Questions = q.Question,
                    CorrectAnswers = q.CorrectAnswer,
                    Badges = q.Badges
                }).ToList()
            }).ToListAsync();
    }

    public async Task<ChapterDto?> GetChapterByIdAsync(int id)
    {
        var chapter = await _context.Chapters
            .Include(c => c.Quizzes)
            .FirstOrDefaultAsync(c => c.ChapterId == id);

        if (chapter == null) return null;
        
        return new ChapterDto
        {
            ChapterId = chapter.ChapterId,
            Title = chapter.Title,
            Summary = chapter.Summary,
            Quiz = chapter.Quiz,
            Quizzes = chapter.Quizzes.Select(q => new QuizDto
            {
                QuizId = q.QuizId,
                ChapterId = q.ChapterId,
                Questions = q.Question,
                CorrectAnswers = q.CorrectAnswer,
                Badges = q.Badges
            }).ToList()
        };
        
    }

    public async Task<ChapterDto> CreateChapterAsync(CreateChapterDto createChapterDto)
    {
        var chapter = new Chapter
        {
            Title = createChapterDto.Title,
            Summary = createChapterDto.Summary,
            Quiz = createChapterDto.Quiz
        };

        _context.Chapters.Add(chapter);
        await _context.SaveChangesAsync();

        return new ChapterDto
        {
            ChapterId = chapter.ChapterId,
            Title = chapter.Title,
            Summary = chapter.Summary,
            Quiz = chapter.Quiz,
            Quizzes = []
        };
    }

    public async Task<ChapterDto?> UpdateChapterAsync(int id, CreateChapterDto updateChapterDto)
    {
     var chapter =  await _context.Chapters.FindAsync(id); 
     if (chapter == null) return null;
     
     chapter.Title = updateChapterDto.Title;
     chapter.Summary = updateChapterDto.Summary;
     chapter.Quiz = updateChapterDto.Quiz;
     
     await _context.SaveChangesAsync();

     return new ChapterDto
     {
         ChapterId = chapter.ChapterId,
         Title = chapter.Title,
         Summary = chapter.Summary,
         Quiz = chapter.Quiz
     };
    }

    public async Task<bool> DeleteChapterAsync(int id)
    {
        var chapter = await _context.Chapters.FindAsync(id);
        if (chapter == null) return false;

        _context.Chapters.Remove(chapter);
        await _context.SaveChangesAsync();
        return true;
    }
}
