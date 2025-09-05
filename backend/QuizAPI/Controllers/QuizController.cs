using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizAPI.DTOs;
using QuizAPI.Services;
using System.Security.Claims;
using System.Threading.Tasks;

namespace QuizAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuizController : ControllerBase
    {
        private readonly IQuizService _quizService;

        public QuizController(IQuizService quizService)
        {
            _quizService = quizService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllQuizzes([FromQuery] string? category = null)
        {
            var quizzes = await _quizService.GetAllQuizzesAsync(category);
            return Ok(quizzes);
        }

        [HttpGet("{slug}")]
        public async Task<IActionResult> GetQuizBySlug(string slug)
        {
            var quiz = await _quizService.GetQuizBySlugAsync(slug);
            if (quiz == null)
                return NotFound();
            return Ok(quiz);
        }

        [HttpGet("featured")]
        public async Task<IActionResult> GetFeaturedQuizzes([FromQuery] int count = 3)
        {
            var quizzes = await _quizService.GetFeaturedQuizzesAsync(count);
            return Ok(quizzes);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateQuiz([FromBody] CreateQuizDto quizDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var quiz = await _quizService.CreateQuizAsync(quizDto);
            return CreatedAtAction(nameof(GetQuizBySlug), new { slug = quiz.Slug }, quiz);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateQuiz(int id, [FromBody] UpdateQuizDto quizDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var quiz = await _quizService.UpdateQuizAsync(id, quizDto);
                return Ok(quiz);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteQuiz(int id)
        {
            var result = await _quizService.DeleteQuizAsync(id);
            if (!result)
                return NotFound();
            return NoContent();
        }

        [HttpPost("{slug}/start")]
        [Authorize]
        public async Task<IActionResult> StartQuiz(string slug)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var session = await _quizService.StartQuizSessionAsync(slug, userId);
            return Ok(session);
        }

        [HttpPost("{slug}/submit-answer")]
        [Authorize]
        public async Task<IActionResult> SubmitAnswer(string slug, [FromBody] SubmitAnswerDto answerDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var response = await _quizService.SubmitAnswerAsync(userId, slug, answerDto);
            return Ok(response);
        }
    }
}