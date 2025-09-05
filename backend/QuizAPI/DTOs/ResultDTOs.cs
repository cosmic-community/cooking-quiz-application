using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuizAPI.DTOs
{
    public class SubmitResultDto
    {
        [Required]
        public int QuizId { get; set; }
        
        [Required]
        public int Score { get; set; }
        
        [Required]
        public int TotalQuestions { get; set; }
        
        [Required]
        public int CorrectAnswers { get; set; }
        
        public int? TimeTaken { get; set; }
        
        public List<AnswerDto> Answers { get; set; } = new List<AnswerDto>();
    }

    public class AnswerDto
    {
        public int QuestionId { get; set; }
        public int SelectedOption { get; set; }
        public bool IsCorrect { get; set; }
        public int? TimeSpent { get; set; }
    }

    public class ResultDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string QuizTitle { get; set; } = string.Empty;
        public int Score { get; set; }
        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; }
        public int? TimeTaken { get; set; }
        public DateTime CompletedAt { get; set; }
    }

    public class LeaderboardEntryDto
    {
        public int Rank { get; set; }
        public string Username { get; set; } = string.Empty;
        public string? AvatarUrl { get; set; }
        public int TotalScore { get; set; }
        public int QuizzesTaken { get; set; }
        public double AverageScore { get; set; }
        public List<string> Achievements { get; set; } = new List<string>();
    }

    public class UserStatisticsDto
    {
        public int TotalQuizzesTaken { get; set; }
        public int TotalScore { get; set; }
        public double AverageScore { get; set; }
        public int BestScore { get; set; }
        public string? BestQuiz { get; set; }
        public int TotalTimePlayed { get; set; }
        public Dictionary<string, int> QuizzesByCategory { get; set; } = new Dictionary<string, int>();
        public List<string> RecentAchievements { get; set; } = new List<string>();
    }
}