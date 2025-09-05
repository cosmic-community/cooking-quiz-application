using QuizAPI.Models;
using QuizAPI.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuizAPI.Services
{
    public interface IQuizService
    {
        Task<IEnumerable<QuizDto>> GetAllQuizzesAsync(string? category = null);
        Task<QuizDetailDto?> GetQuizBySlugAsync(string slug);
        Task<QuizDto> CreateQuizAsync(CreateQuizDto quizDto);
        Task<QuizDto> UpdateQuizAsync(int id, UpdateQuizDto quizDto);
        Task<bool> DeleteQuizAsync(int id);
        Task<IEnumerable<QuizDto>> GetFeaturedQuizzesAsync(int count = 3);
        Task<QuizSessionDto> StartQuizSessionAsync(string slug, int userId);
        Task<SubmitAnswerResponseDto> SubmitAnswerAsync(int userId, string quizSlug, SubmitAnswerDto answerDto);
    }
}