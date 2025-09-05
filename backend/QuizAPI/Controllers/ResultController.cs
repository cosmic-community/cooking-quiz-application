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
    [Authorize]
    public class ResultController : ControllerBase
    {
        private readonly IResultService _resultService;

        public ResultController(IResultService resultService)
        {
            _resultService = resultService;
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserResults(int userId)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            
            // Users can only view their own results unless they're admin
            if (currentUserId != userId && !User.IsInRole("Admin"))
                return Forbid();

            var results = await _resultService.GetUserResultsAsync(userId);
            return Ok(results);
        }

        [HttpGet("leaderboard")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLeaderboard([FromQuery] int limit = 10)
        {
            var leaderboard = await _resultService.GetLeaderboardAsync(limit);
            return Ok(leaderboard);
        }

        [HttpPost("submit")]
        public async Task<IActionResult> SubmitResult([FromBody] SubmitResultDto resultDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var result = await _resultService.SubmitResultAsync(userId, resultDto);
            
            return Ok(result);
        }

        [HttpGet("statistics/{userId}")]
        public async Task<IActionResult> GetUserStatistics(int userId)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            
            if (currentUserId != userId && !User.IsInRole("Admin"))
                return Forbid();

            var stats = await _resultService.GetUserStatisticsAsync(userId);
            return Ok(stats);
        }
    }
}