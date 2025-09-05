using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuizAPI.Models
{
    public class Result
    {
        public int Id { get; set; }
        
        public int UserId { get; set; }
        public virtual User? User { get; set; }
        
        public int QuizId { get; set; }
        public virtual Quiz? Quiz { get; set; }
        
        public int Score { get; set; }
        
        public int TotalQuestions { get; set; }
        
        public int CorrectAnswers { get; set; }
        
        public int? TimeTaken { get; set; } // in seconds
        
        public DateTime CompletedAt { get; set; }
        
        public virtual ICollection<AnswerRecord> Answers { get; set; } = new List<AnswerRecord>();
    }

    public class AnswerRecord
    {
        public int Id { get; set; }
        
        public int ResultId { get; set; }
        public virtual Result? Result { get; set; }
        
        public int QuestionId { get; set; }
        public virtual Question? Question { get; set; }
        
        public int SelectedOption { get; set; }
        
        public bool IsCorrect { get; set; }
        
        public int? TimeSpent { get; set; } // in seconds
    }
}