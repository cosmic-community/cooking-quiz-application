using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuizAPI.Models
{
    public class Quiz
    {
        public int Id { get; set; }
        
        [Required]
        public string Title { get; set; } = string.Empty;
        
        public string? Description { get; set; }
        
        public string Slug { get; set; } = string.Empty;
        
        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }
        
        [Required]
        public DifficultyLevel Difficulty { get; set; }
        
        public int? TimeLimit { get; set; } // in minutes
        
        public int? PassingScore { get; set; }
        
        public string? FeaturedImageUrl { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime UpdatedAt { get; set; }
        
        public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
        public virtual ICollection<Result> Results { get; set; } = new List<Result>();
    }

    public enum DifficultyLevel
    {
        Easy,
        Medium,
        Hard,
        Expert
    }
}