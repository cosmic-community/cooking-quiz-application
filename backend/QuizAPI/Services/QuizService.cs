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
    public class QuizService : IQuizService
    {
        private readonly QuizDbContext _context;
        private readonly ICosmicService _cosmicService;

        public QuizService(QuizDbContext context, ICosmicService cosmicService)
        {
            _context = context;
            _cosmicService = cosmicService;
        }

        public async Task<IEnumerable<QuizDto>> GetAllQuizzesAsync(string? category = null)
        {
            var query = _context.Quizzes
                .Include(q => q.Category)
                .Include(q => q.Questions)
                .AsQueryable();

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(q => q.Category != null && q.Category.Slug == category);
            }

            var quizzes = await query.ToListAsync();

            return quizzes.Select(q => new QuizDto
            {
                Id = q.Id,
                Title = q.Title,
                Description = q.Description,
                Slug = q.Slug,
                Category = q.Category?.Name,
                CategorySlug = q.Category?.Slug,
                Difficulty = q.Difficulty.ToString(),
                TimeLimit = q.TimeLimit,
                TotalQuestions = q.Questions.Count,
                FeaturedImageUrl = q.FeaturedImageUrl
            });
        }

        public async Task<QuizDetailDto?> GetQuizBySlugAsync(string slug)
        {
            var quiz = await _context.Quizzes
                .Include(q => q.Category)
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(q => q.Slug == slug);

            if (quiz == null)
                return null;

            return new QuizDetailDto
            {
                Id = quiz.Id,
                Title = quiz.Title,
                Description = quiz.Description,
                Slug = quiz.Slug,
                Category = quiz.Category?.Name,
                Difficulty = quiz.Difficulty.ToString(),
                TimeLimit = quiz.TimeLimit,
                PassingScore = quiz.PassingScore,
                Questions = quiz.Questions.Select(q => new QuestionDto
                {
                    Id = q.Id,
                    QuestionText = q.QuestionText,
                    Options = q.Options.OrderBy(o => o.OptionIndex).Select(o => new OptionDto
                    {
                        Id = o.Id,
                        Text = o.Text,
                        Index = o.OptionIndex
                    }).ToList(),
                    Points = q.Points
                }).ToList()
            };
        }

        public async Task<QuizDto> CreateQuizAsync(CreateQuizDto quizDto)
        {
            var category = await _context.Categories.FindAsync(quizDto.CategoryId);
            if (category == null)
                throw new ArgumentException("Invalid category");

            var quiz = new Quiz
            {
                Title = quizDto.Title,
                Description = quizDto.Description,
                Slug = GenerateSlug(quizDto.Title),
                CategoryId = quizDto.CategoryId,
                Difficulty = Enum.Parse<DifficultyLevel>(quizDto.Difficulty),
                TimeLimit = quizDto.TimeLimit,
                PassingScore = quizDto.PassingScore,
                FeaturedImageUrl = quizDto.FeaturedImageUrl
            };

            _context.Quizzes.Add(quiz);
            await _context.SaveChangesAsync();

            // Sync with Cosmic if needed
            await _cosmicService.SyncQuizToCosmicAsync(quiz);

            return new QuizDto
            {
                Id = quiz.Id,
                Title = quiz.Title,
                Description = quiz.Description,
                Slug = quiz.Slug,
                Category = category.Name,
                CategorySlug = category.Slug,
                Difficulty = quiz.Difficulty.ToString(),
                TimeLimit = quiz.TimeLimit
            };
        }

        public async Task<QuizDto> UpdateQuizAsync(int id, UpdateQuizDto quizDto)
        {
            var quiz = await _context.Quizzes
                .Include(q => q.Category)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (quiz == null)
                throw new KeyNotFoundException("Quiz not found");

            quiz.Title = quizDto.Title ?? quiz.Title;
            quiz.Description = quizDto.Description ?? quiz.Description;
            quiz.TimeLimit = quizDto.TimeLimit ?? quiz.TimeLimit;
            quiz.PassingScore = quizDto.PassingScore ?? quiz.PassingScore;

            if (quizDto.Difficulty != null)
                quiz.Difficulty = Enum.Parse<DifficultyLevel>(quizDto.Difficulty);

            quiz.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            // Sync with Cosmic
            await _cosmicService.SyncQuizToCosmicAsync(quiz);

            return new QuizDto
            {
                Id = quiz.Id,
                Title = quiz.Title,
                Description = quiz.Description,
                Slug = quiz.Slug,
                Category = quiz.Category?.Name,
                CategorySlug = quiz.Category?.Slug,
                Difficulty = quiz.Difficulty.ToString(),
                TimeLimit = quiz.TimeLimit
            };
        }

        public async Task<bool> DeleteQuizAsync(int id)
        {
            var quiz = await _context.Quizzes.FindAsync(id);
            if (quiz == null)
                return false;

            _context.Quizzes.Remove(quiz);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<QuizDto>> GetFeaturedQuizzesAsync(int count = 3)
        {
            var quizzes = await _context.Quizzes
                .Include(q => q.Category)
                .Include(q => q.Questions)
                .OrderByDescending(q => q.CreatedAt)
                .Take(count)
                .ToListAsync();

            return quizzes.Select(q => new QuizDto
            {
                Id = q.Id,
                Title = q.Title,
                Description = q.Description,
                Slug = q.Slug,
                Category = q.Category?.Name,
                CategorySlug = q.Category?.Slug,
                Difficulty = q.Difficulty.ToString(),
                TimeLimit = q.TimeLimit,
                TotalQuestions = q.Questions.Count,
                FeaturedImageUrl = q.FeaturedImageUrl
            });
        }

        public async Task<QuizSessionDto> StartQuizSessionAsync(string slug, int userId)
        {
            var quiz = await GetQuizBySlugAsync(slug);
            if (quiz == null)
                throw new KeyNotFoundException("Quiz not found");

            return new QuizSessionDto
            {
                QuizId = quiz.Id,
                QuizTitle = quiz.Title,
                CurrentQuestionIndex = 0,
                TotalQuestions = quiz.Questions.Count,
                TimeLimit = quiz.TimeLimit,
                StartTime = DateTime.UtcNow
            };
        }

        public async Task<SubmitAnswerResponseDto> SubmitAnswerAsync(int userId, string quizSlug, SubmitAnswerDto answerDto)
        {
            var quiz = await _context.Quizzes
                .Include(q => q.Questions)
                .FirstOrDefaultAsync(q => q.Slug == quizSlug);

            if (quiz == null)
                throw new KeyNotFoundException("Quiz not found");

            var question = quiz.Questions.FirstOrDefault(q => q.Id == answerDto.QuestionId);
            if (question == null)
                throw new ArgumentException("Invalid question");

            var isCorrect = answerDto.SelectedOption == question.CorrectAnswer;

            return new SubmitAnswerResponseDto
            {
                IsCorrect = isCorrect,
                CorrectAnswer = question.CorrectAnswer,
                Explanation = question.Explanation
            };
        }

        private string GenerateSlug(string title)
        {
            return title.ToLower()
                .Replace(" ", "-")
                .Replace("'", "")
                .Replace(".", "")
                .Replace(",", "");
        }
    }
}