using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuizAPI.Models
{
    public class Question
    {
        public int Id { get; set; }
        
        [Required]
        public string QuestionText { get; set; } = string.Empty;
        
        public int QuizId { get; set; }
        public virtual Quiz? Quiz { get; set; }
        
        public int CorrectAnswer { get; set; } // Index of correct option
        
        public string? Explanation { get; set; }
        
        public int Points { get; set; } = 1;
        
        public virtual ICollection<QuestionOption> Options { get; set; } = new List<QuestionOption>();
    }

    public class QuestionOption
    {
        public int Id { get; set; }
        
        [Required]
        public string Text { get; set; } = string.Empty;
        
        public int QuestionId { get; set; }
        public virtual Question? Question { get; set; }
        
        public int OptionIndex { get; set; }
    }
}