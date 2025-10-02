using FF7_remake.DBContext;
using FF7_remake.DTOs;
using FF7_remake.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace FF7_remake.Services;

public interface IQuizService
{
    Task<List<QuizDto>> GetAllQuizzes();
    Task<QuizDto?> GetQuizByIdAsync(int id);
    Task<QuizDto> CreateQuizDtoAsync(CreateQuizDto createQuizDto);
    Task<QuizDto?> UpdateQuizDtoAsync(int id, CreateQuizDto updateQuizDto);
    Task<bool> DeleteQuizByIdAsync(int id);

    Task<List<QuizDto>> checkAnswerAsync(int id, Dictionary<int, string> userAnswers);



}

public class QuizService : IQuizService

{
    private readonly Ff7DbContext context;

    public QuizService(Ff7DbContext context)
    {
        this.context = context;
    }

    public async Task<List<QuizDto>> GetAllQuizzes()
    {
        return await context.Quizzes
            .Include(q => q.Chapter)
            .Select(q => new QuizDto
            {
                QuizId = q.QuizId,
                Questions = q.Question,
                CorrectAnswers = q.CorrectAnswer,
                ChapterTitle = q.Chapter.Title,
                ChapterId = q.ChapterId
                
            }).ToListAsync();
        
                    
          
        
    }
    
    public async Task<QuizDto?> GetQuizByIdAsync( int id)

    {
        var quiz = await context.Quizzes
            .Include(q => q.Chapter)
            .FirstOrDefaultAsync(q => q.QuizId == id);

        if (quiz == null)
        {
            return null;
        }

        return new QuizDto
        {
            QuizId = quiz.QuizId,
            ChapterId = quiz.ChapterId,
            Questions = quiz.Question,
            CorrectAnswers = quiz.CorrectAnswer,
            Badges = quiz.Badges

        };


    }

    public async Task<QuizDto> CreateQuizDtoAsync(CreateQuizDto createQuizDto)
    {
        var quiz = new Quiz
        {
            ChapterId = createQuizDto.ChapterId,
            Question = createQuizDto.Questions,
            CorrectAnswer = createQuizDto.CorrectAnswers,
            Badges = createQuizDto.Badges
        };

        context.Quizzes.Add(quiz);
        await context.SaveChangesAsync();

        return new QuizDto
        {
            QuizId = quiz.QuizId,
            ChapterId = quiz.ChapterId,
            Questions = quiz.Question,
            CorrectAnswers = quiz.CorrectAnswer,
            Badges = quiz.Badges
        };
        

    }

    public async Task<QuizDto?> UpdateQuizDtoAsync(int id, CreateQuizDto updateQuizDto)
    {
        var quiz = await context.Quizzes.FindAsync(id);
           

        if (quiz == null) return null;

        quiz.Question = updateQuizDto.Questions;
        quiz.CorrectAnswer = updateQuizDto.CorrectAnswers;
        quiz.Badges = updateQuizDto.Badges;


        await context.SaveChangesAsync();

        return new QuizDto()
        {
            QuizId = quiz.QuizId,
            Questions = quiz.Question,
            CorrectAnswers = quiz.CorrectAnswer,
            Badges = quiz.Badges
        };





    }

    public async Task<bool> DeleteQuizByIdAsync(int id)
    {
        var quiz = await context.Quizzes.FindAsync(id);

        if (quiz == null)
        {
            return false;
        }
        
        context.Quizzes.Remove(quiz);

        await context.SaveChangesAsync();
        return true;

    }

    public async Task<List<QuizDto>> checkAnswerAsync(int chapterId, Dictionary<int, string> userAnswers)
    {
        var quizzes = await context.Quizzes
            .Include(q => q.Chapter)
            .Where(q => q.ChapterId == chapterId)
            .ToListAsync();


        var results = new List<QuizDto>();


        foreach (var quiz in quizzes)
        {
            userAnswers.TryGetValue(quiz.QuizId, out string? userAnswer);
            bool isCorrect = quiz.CorrectAnswer == userAnswer;

            results.Add(new QuizDto
                {
                    QuizId = quiz.QuizId,
                    ChapterId = quiz.ChapterId,
                    ChapterTitle = quiz.Chapter.Title,
                    Questions = quiz.Question,
                    CorrectAnswers = isCorrect ? "Good Answer" : "Bad Answer"
                }

            );

        }

        return results;
    }




}