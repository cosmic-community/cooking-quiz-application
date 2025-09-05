using QuizAPI.DTOs;
using System.Threading.Tasks;

namespace QuizAPI.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task<AuthResponseDto> RefreshTokenAsync(string token);
        string GenerateJwtToken(int userId, string username, string role);
    }
}