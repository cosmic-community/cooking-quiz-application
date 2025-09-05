using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QuizAPI.Models;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace QuizAPI.Services
{
    public class CosmicService : ICosmicService
    {
        private readonly string _bucketSlug;
        private readonly string _readKey;
        private readonly string _writeKey;
        private readonly RestClient _client;

        public CosmicService(IConfiguration configuration)
        {
            _bucketSlug = configuration["Cosmic:BucketSlug"] ?? "";
            _readKey = configuration["Cosmic:ReadKey"] ?? "";
            _writeKey = configuration["Cosmic:WriteKey"] ?? "";
            _client = new RestClient($"https://api.cosmicjs.com/v3/buckets/{_bucketSlug}");
        }

        public async Task SyncQuizToCosmicAsync(Quiz quiz)
        {
            try
            {
                var request = new RestRequest("objects", Method.Post);
                request.AddHeader("Authorization", $"Bearer {_writeKey}");
                request.AddJsonBody(new
                {
                    title = quiz.Title,
                    type = "quizzes",
                    slug = quiz.Slug,
                    metadata = new
                    {
                        description = quiz.Description,
                        difficulty = quiz.Difficulty.ToString().ToLower(),
                        time_limit = quiz.TimeLimit,
                        passing_score = quiz.PassingScore,
                        total_questions = quiz.Questions.Count
                    }
                });

                var response = await _client.ExecuteAsync(request);
                if (!response.IsSuccessful)
                {
                    Console.WriteLine($"Failed to sync quiz to Cosmic: {response.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error syncing quiz to Cosmic: {ex.Message}");
            }
        }

        public async Task SyncUserToCosmicAsync(User user)
        {
            try
            {
                var request = new RestRequest("objects", Method.Post);
                request.AddHeader("Authorization", $"Bearer {_writeKey}");
                request.AddJsonBody(new
                {
                    title = user.Username,
                    type = "users",
                    metadata = new
                    {
                        email = user.Email,
                        username = user.Username,
                        role = user.Role.ToString().ToLower(),
                        total_score = user.TotalScore,
                        quizzes_taken = user.QuizzesTaken
                    }
                });

                var response = await _client.ExecuteAsync(request);
                if (!response.IsSuccessful)
                {
                    Console.WriteLine($"Failed to sync user to Cosmic: {response.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error syncing user to Cosmic: {ex.Message}");
            }
        }

        public async Task SyncResultToCosmicAsync(Result result)
        {
            try
            {
                var request = new RestRequest("objects", Method.Post);
                request.AddHeader("Authorization", $"Bearer {_writeKey}");
                request.AddJsonBody(new
                {
                    title = $"Result - {result.CompletedAt:yyyy-MM-dd HH:mm:ss}",
                    type = "results",
                    metadata = new
                    {
                        score = result.Score,
                        total_questions = result.TotalQuestions,
                        correct_answers = result.CorrectAnswers,
                        time_taken = result.TimeTaken,
                        completed_at = result.CompletedAt.ToString("yyyy-MM-dd")
                    }
                });

                var response = await _client.ExecuteAsync(request);
                if (!response.IsSuccessful)
                {
                    Console.WriteLine($"Failed to sync result to Cosmic: {response.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error syncing result to Cosmic: {ex.Message}");
            }
        }

        public async Task<bool> ImportQuizzesFromCosmicAsync()
        {
            try
            {
                var request = new RestRequest($"objects?type=quizzes&read_key={_readKey}", Method.Get);
                var response = await _client.ExecuteAsync(request);

                if (response.IsSuccessful && response.Content != null)
                {
                    // Parse and import quizzes
                    // This would map Cosmic objects to local database
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error importing quizzes from Cosmic: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> ImportCategoriesFromCosmicAsync()
        {
            try
            {
                var request = new RestRequest($"objects?type=categories&read_key={_readKey}", Method.Get);
                var response = await _client.ExecuteAsync(request);

                if (response.IsSuccessful && response.Content != null)
                {
                    // Parse and import categories
                    // This would map Cosmic objects to local database
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error importing categories from Cosmic: {ex.Message}");
                return false;
            }
        }
    }
}