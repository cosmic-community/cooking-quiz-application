using QuizAPI.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuizAPI.Services
{
    public interface IResultService
    {
        Task<ResultDto> SubmitResultAsync(int userId, SubmitResultDto resultDto);
        Task<IEnumerable<ResultDto>> GetUserResultsAsync(int userId);
        Task<IEnumerable<LeaderboardEntryDto>> GetLeaderboardAsync(int limit = 10);
        Task<UserStatisticsDto> GetUserStatisticsAsync(int userId);
    }
}