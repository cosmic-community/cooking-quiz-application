using Microsoft.EntityFrameworkCore;
using QuizAPI.Data;
using QuizAPI.DTOs;
using QuizAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizAPI.Services
{
    public class ResultService : IResultService
    {
        private readonly QuizDbContext _context;
        private readonly IUserService _userService;
        private readonly ICosmicService _cosmicService;

        public ResultService(QuizDbContext context, IUserService userService, ICosmicService cosmicService)
        {
            _context = context;
            _userService = userService;
            _cosmicService = cosmicService;
        }

        public async Task<ResultDto> SubmitResultAsync(int userId, SubmitResultDto resultDto)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            var quiz = await _context.Quizzes.FindAsync(resultDto.QuizId);
            if (quiz == null)
                throw new KeyNotFoundException("Quiz not found");

            var result = new Result
            {
                UserId = userId,
                QuizId = resultDto.QuizId,
                Score = resultDto.Score,
                TotalQuestions = resultDto.TotalQuestions,
                CorrectAnswers = resultDto.CorrectAnswers,
                TimeTaken = resultDto.TimeTaken,
                CompletedAt = DateTime.UtcNow
            };

            // Save answer records
            foreach (var answer in resultDto.Answers)
            {
                result.Answers.Add(new AnswerRecord
                {
                    QuestionId = answer.QuestionId,
                    SelectedOption = answer.SelectedOption,
                    IsCorrect = answer.IsCorrect,
                    TimeSpent = answer.TimeSpent
                });
            }

            _context.Results.Add(result);

            // Update user statistics
            user.TotalScore += resultDto.Score;
            user.QuizzesTaken++;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            // Sync to Cosmic
            await _cosmicService.SyncResultToCosmicAsync(result);

            return new ResultDto
            {
                Id = result.Id,
                Username = user.Username,
                QuizTitle = quiz.Title,
                Score = result.Score,
                TotalQuestions = result.TotalQuestions,
                CorrectAnswers = result.CorrectAnswers,
                TimeTaken = result.TimeTaken,
                CompletedAt = result.CompletedAt
            };
        }

        public async Task<IEnumerable<ResultDto>> GetUserResultsAsync(int userId)
        {
            var results = await _context.Results
                .Include(r => r.User)
                .Include(r => r.Quiz)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.CompletedAt)
                .ToListAsync();

            return results.Select(r => new ResultDto
            {
                Id = r.Id,
                Username = r.User?.Username ?? "Unknown",
                QuizTitle = r.Quiz?.Title ?? "Unknown Quiz",
                Score = r.Score,
                TotalQuestions = r.TotalQuestions,
                CorrectAnswers = r.CorrectAnswers,
                TimeTaken = r.TimeTaken,
                CompletedAt = r.CompletedAt
            });
        }

        public async Task<IEnumerable<LeaderboardEntryDto>> GetLeaderboardAsync(int limit = 10)
        {
            var topUsers = await _context.Users
                .Include(u => u.UserAchievements)
                    .ThenInclude(ua => ua.Achievement)
                .OrderByDescending(u => u.TotalScore)
                .Take(limit)
                .ToListAsync();

            var leaderboard = new List<LeaderboardEntryDto>();
            int rank = 1;

            foreach (var user in topUsers)
            {
                var averageScore = user.QuizzesTaken > 0 
                    ? (double)user.TotalScore / user.QuizzesTaken 
                    : 0;

                leaderboard.Add(new LeaderboardEntryDto
                {
                    Rank = rank++,
                    Username = user.Username,
                    AvatarUrl = user.AvatarUrl,
                    TotalScore = user.TotalScore,
                    QuizzesTaken = user.QuizzesTaken,
                    AverageScore = Math.Round(averageScore, 2),
                    Achievements = user.UserAchievements
                        .Select(ua => ua.Achievement?.Name ?? "")
                        .Where(a => !string.IsNullOrEmpty(a))
                        .ToList()
                });
            }

            return leaderboard;
        }

        public async Task<UserStatisticsDto> GetUserStatisticsAsync(int userId)
        {
            var user = await _context.Users
                .Include(u => u.Results)
                    .ThenInclude(r => r.Quiz)
                        .ThenInclude(q => q.Category)
                .Include(u => u.UserAchievements)
                    .ThenInclude(ua => ua.Achievement)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new KeyNotFoundException("User not found");

            var results = user.Results.ToList();
            var totalTimePlayed = results.Sum(r => r.TimeTaken ?? 0);
            var bestResult = results.OrderByDescending(r => r.Score).FirstOrDefault();
            var averageScore = results.Any() 
                ? results.Average(r => r.Score) 
                : 0;

            var quizzesByCategory = results
                .Where(r => r.Quiz?.Category != null)
                .GroupBy(r => r.Quiz.Category.Name)
                .ToDictionary(g => g.Key, g => g.Count());

            var recentAchievements = user.UserAchievements
                .OrderByDescending(ua => ua.UnlockedAt)
                .Take(5)
                .Select(ua => ua.Achievement?.Name ?? "")
                .Where(a => !string.IsNullOrEmpty(a))
                .ToList();

            return new UserStatisticsDto
            {
                TotalQuizzesTaken = user.QuizzesTaken,
                TotalScore = user.TotalScore,
                AverageScore = Math.Round(averageScore, 2),
                BestScore = bestResult?.Score ?? 0,
                BestQuiz = bestResult?.Quiz?.Title,
                TotalTimePlayed = totalTimePlayed,
                QuizzesByCategory = quizzesByCategory,
                RecentAchievements = recentAchievements
            };
        }
    }
}