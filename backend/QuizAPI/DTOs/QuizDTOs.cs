using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuizAPI.DTOs
{
    public class QuizDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string? Category { get; set; }
        public string? CategorySlug { get; set; }
        public string Difficulty { get; set; } = string.Empty;
        public int? TimeLimit { get; set; }
        public int TotalQuestions { get; set; }
        public string? FeaturedImageUrl { get; set; }
    }

    public class QuizDetailDto : QuizDto
    {
        public int? PassingScore { get; set; }
        public List<QuestionDto> Questions { get; set; } = new List<QuestionDto>();
    }

    public class CreateQuizDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        
        public string? Description { get; set; }
        
        [Required]
        public int CategoryId { get; set; }
        
        [Required]
        public string Difficulty { get; set; } = "Easy";
        
        public int? TimeLimit { get; set; }
        public int? PassingScore { get; set; }
        public string? FeaturedImageUrl { get; set; }
    }

    public class UpdateQuizDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Difficulty { get; set; }
        public int? TimeLimit { get; set; }
        public int? PassingScore { get; set; }
    }

    public class QuestionDto
    {
        public int Id { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public List<OptionDto> Options { get; set; } = new List<OptionDto>();
        public int Points { get; set; }
    }

    public class OptionDto
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public int Index { get; set; }
    }

    public class QuizSessionDto
    {
        public int QuizId { get; set; }
        public string QuizTitle { get; set; } = string.Empty;
        public int CurrentQuestionIndex { get; set; }
        public int TotalQuestions { get; set; }
        public int? TimeLimit { get; set; }
        public DateTime StartTime { get; set; }
    }

    public class SubmitAnswerDto
    {
        [Required]
        public int QuestionId { get; set; }
        
        [Required]
        public int SelectedOption { get; set; }
        
        public int? TimeSpent { get; set; }
    }

    public class SubmitAnswerResponseDto
    {
        public bool IsCorrect { get; set; }
        public int CorrectAnswer { get; set; }
        public string? Explanation { get; set; }
    }
}