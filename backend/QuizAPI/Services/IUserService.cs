using QuizAPI.DTOs;
using QuizAPI.Models;
using System.Threading.Tasks;

namespace QuizAPI.Services
{
    public interface IUserService
    {
        Task<User?> GetUserByIdAsync(int userId);
        Task<User?> GetUserByUsernameAsync(string username);
        Task<User?> GetUserByEmailAsync(string email);
        Task<bool> UpdateUserAsync(int userId, UserDto userDto);
        Task<bool> UpdateUserScoreAsync(int userId, int scoreToAdd);
        Task<bool> IncrementQuizzesTakenAsync(int userId);
    }
}