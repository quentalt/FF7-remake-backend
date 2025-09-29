using FF7_remake.DBContext;
using FF7_remake.DTOs;
using FF7_remake.Models;
using Microsoft.EntityFrameworkCore;

namespace FF7_remake.Services;

public interface IQuizService
{
    Task<List<QuizDto>> GetAllQuizzes();
    Task<QuizDto> GetQuizByIdAsync(int id);
    Task<QuizDto> CreateQuizDtoAsync(CreateQuizDto createQuizDto);
    Task<QuizDto> UpdateQuizDtoAsync(int id, CreateQuizDto updateQuizDto);
    Task<bool> DeleteQuizByIdAsync(int id);
    


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
        return await context.Quiz
            .Include(q => q.Chapter)
            .Select(q => new QuizDto
            {
                QuizId = q.QuizId,
                Question = q.Question,
                CorrectAnswer = q.CorrectAnswer,
                Chapters = q.Quizzes.Select
                (c => new QuizDto
                                           
                {
                    ChapterId = c.ChapterId,
                    Title = c.Title,
                    Summary = c.Summary,
                    Quiz = c.Quiz
                }).ToList()
                
            }).ToListAsync();
        
                    
          
        
    }
    
    public async Task<QuizDTO?> GetQuizByIdAsync( int id)

    {
        var chapter = await context.Quizzes
            .Include(q => q.Chapter)
            .FirstOrDefaultAsync(q => q.QuizId == id);

        if (quiz == null)
        {
            return null;
        }

        return new QuizDTO
        {
            QuizId = quiz.QuizId,
            ChapterId = quiz.ChapterId,
            Question = quiz.Question,
            CorrectAnswer = quiz.CorrectAnswer,
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
            Question = quiz.Question,
            CorrectAnswer = quiz.CorrectAnswer,
            Badges = quiz.Badges
        };
        

    }

    public async Task<QuizDto?> UpdateQuizDtoAsync(int id, CreateQuizDto updateQuizDto)
    {
        var chapter = await context.Quiz
            .Include(q => q.Quizzes)
            .FirstOrDefaultAsync(q => q.QuizId == id);

        if (quiz == null)
        {
            return null;
        }

        quiz.Question = UpdateQuizDtoAsync.Question;
        quiz.CorrectAnswer = UpdateQuizDtoAsync.CorrectAnswer;
        quiz.Badges = UpdateQuizDtoAsync.Badges;


        await context.SaveChangesAsync();

        return new QuizDto()
        {
            QuizId = Quiz.QuizId,
            Questions = Quiz.Questions,
            CorrectAnswers = Quiz.CorrectAnswers,
            Badges = Quiz.Badges
        };





    }

    public async Task<bool> DeleteQuizByIdAsync(int id)
    {
        var quiz = await context.Quiz.FindAsync(id);

        if (quiz == null)
        {
            return false;
        }
        
        context.Quiz.Remove(quiz);

        await context.SavesChangesAsync();
        return true;

    }

    public async Task<quizDTO?> findGoodAnswerByIdAsync(int id)
    {
        var quiz = await context.Quiz.FindAsync(id);

        if (qu)
        {
            
        }
        
        
        
              
        
        
        
        
    } 

            
    }