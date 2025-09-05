using QuizAPI.Models;
using System.Threading.Tasks;

namespace QuizAPI.Services
{
    public interface ICosmicService
    {
        Task SyncQuizToCosmicAsync(Quiz quiz);
        Task SyncUserToCosmicAsync(User user);
        Task SyncResultToCosmicAsync(Result result);
        Task<bool> ImportQuizzesFromCosmicAsync();
        Task<bool> ImportCategoriesFromCosmicAsync();
    }
}