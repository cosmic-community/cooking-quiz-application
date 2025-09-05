using Microsoft.EntityFrameworkCore;
using QuizAPI.Data;
using QuizAPI.DTOs;
using QuizAPI.Models;
using System;
using System.Threading.Tasks;

namespace QuizAPI.Services
{
    public class UserService : IUserService
    {
        private readonly QuizDbContext _context;

        public UserService(QuizDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _context.Users
                .Include(u => u.Results)
                .Include(u => u.UserAchievements)
                    .ThenInclude(ua => ua.Achievement)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> UpdateUserAsync(int userId, UserDto userDto)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return false;

            if (!string.IsNullOrEmpty(userDto.Email))
                user.Email = userDto.Email;
            
            if (!string.IsNullOrEmpty(userDto.Username))
                user.Username = userDto.Username;
            
            if (!string.IsNullOrEmpty(userDto.AvatarUrl))
                user.AvatarUrl = userDto.AvatarUrl;

            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateUserScoreAsync(int userId, int scoreToAdd)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return false;

            user.TotalScore += scoreToAdd;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IncrementQuizzesTakenAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return false;

            user.QuizzesTaken++;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}