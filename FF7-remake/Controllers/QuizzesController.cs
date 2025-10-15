using FF7_remake.DTOs;
using FF7_remake.Services;
using Microsoft.AspNetCore.Mvc;
namespace FF7_remake.Controllers;
[ApiController]

public class QuizzesController : ControllerBase
{
    private readonly IQuizService _quizService;

    public QuizzesController(IQuizService quizService)
    {
        _quizService = quizService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<QuizDto>>>> GetQuizzes()
    {
        try
        {
            var quizzes = await _quizService.GetAllQuizzes();
            return Ok(new ApiResponse<List<QuizDto>>
            {
                Success = true,
                Message = " Quizzes retrieved successfully",
                Data = quizzes

            });

        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<List<QuizDto>>
            {
                Success = false,
                Message = "An error occured while retrieving quizzes",
                Errors = [ex.Message]
            });
        }
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<QuizDto>>> GetQuiz(int id)
    {
        try
        {
            var quizzes = await _quizService.GetQuizByIdAsync(id);
            if (quizzes == null)
            {
                return NotFound(new ApiResponse<QuizDto>
                {
                    Success = false,
                    Message = "Quiz not found",

                });
                
            }

            return Ok(new ApiResponse<QuizDto>
                {
                    Success = true,
                    Message = " Quiz retrieved successfully",
                    Data = quizzes
                }
            );



        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<List<QuizDto>>
            {
                Success = false,
                Message = "An error occured while retrieving the quiz",
                Errors = [ex.Message]
            });
           
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<QuizDto>>> CreateQuiz(CreateQuizDto createQuizDto)
    {
        try
        {
            var quiz = await _quizService.CreateQuizDtoAsync(createQuizDto);
            return CreatedAtAction(nameof(GetQuiz),routeValues: new { id = quiz.QuizId }, new ApiResponse<QuizDto>

            {
                Success = true,
                Message = "Quiz created successfully",
                Data = quiz
            });

        }
        catch (Exception ex)
        {
           return StatusCode(500, new ApiResponse <QuizDto>
           {
               Success = false,
               Message = "An error occured while creating the quiz",
               Errors = [ex.Message]
           });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<QuizDto>>> UpdateQuiz(int id, CreateQuizDto updateQuizDto)
    {
        try
        {
            var quiz = await _quizService.UpdateQuizDtoAsync(id, updateQuizDto);
            if (quiz == null)
            {
                return NotFound(new ApiResponse<QuizDto>
                {
                    Success = false,
                    Message = "Quiz not found",

                });
                
            }

            return Ok(new ApiResponse<QuizDto>
                {
                    Success = true,
                    Message = " Quiz updated successfully",
                    Data = quiz
                }
            );



        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<List<QuizDto>>
            {
                Success = false,
                Message = "An error occured while updating the quiz",
                Errors = [ex.Message]
            });
           
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<QuizDto>>> DeleteQuiz(int id)
    {
        try
        {
            var quiz = await _quizService.DeleteQuizByIdAsync(id);
            if (!quiz)
            {
                return NotFound(new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Quiz not found",
                    Data = false

                });
                
            }

            return Ok(new ApiResponse<bool>
                {
                    Success = true,
                    Message = " Quiz deleted successfully",
                    Data = quiz
                }
            );



        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<List<bool>>
            {
                Success = false,
                Message = "An error occured while deleting the quiz",
                Errors = [ex.Message]
            });
           
        }
    }

    [HttpGet]

    public async Task<ActionResult<ApiResponse<List<QuizDto>>>> AnswerQuiz(int chapterId, Dictionary < int, string> userAnswers)
    {
         try
                {
                    var answer = await _quizService.checkAnswerAsync(chapterId, userAnswers);
                    if (answer == null)
                    {
                        return NotFound(new ApiResponse <List<QuizDto>>
                        {
                            Success = false,
                            Message = "Answer not found",
        
                        });
                        
                    }
        
                    return Ok(new ApiResponse<List<QuizDto>>
                        {
                            Success = true,
                            Message = "Answer retrieved successfully",
                            Data = answer
                        }
                    );
        
        
        
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new ApiResponse<List<QuizDto>>
                    {
                        Success = false,
                        Message = "An error occured while retrieving the quiz",
                        Errors = [ex.Message]
                    });
                   
                }
        
    }

    
    
}