using FF7_remake.DTOs;
using FF7_remake.Services;
using Microsoft.AspNetCore.Mvc;

namespace FF7_remake.Controllers;
[ApiController]
[Route("api/[controller]")]

public class QuizController : ControllerBase
{
    private readonly IQuizService _quizService;

    public QuizController(IUserService userService)
    {
        _userS
    }
}