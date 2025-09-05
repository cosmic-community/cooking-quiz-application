using Microsoft.AspNetCore.Mvc;
using QuizAPI.DTOs;
using QuizAPI.Services;
using System.Threading.Tasks;

namespace QuizAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(registerDto);
            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new { token = result.Token, user = result.User });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LoginAsync(loginDto);
            if (!result.Success)
                return Unauthorized(new { message = result.Message });

            return Ok(new { token = result.Token, user = result.User });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto refreshDto)
        {
            var result = await _authService.RefreshTokenAsync(refreshDto.Token);
            if (!result.Success)
                return Unauthorized(new { message = result.Message });

            return Ok(new { token = result.Token });
        }
    }
}