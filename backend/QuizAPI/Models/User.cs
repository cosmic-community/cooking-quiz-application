using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuizAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        public string Username { get; set; } = string.Empty;
        
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        
        public UserRole Role { get; set; } = UserRole.User;
        
        public int TotalScore { get; set; } = 0;
        
        public int QuizzesTaken { get; set; } = 0;
        
        public string? AvatarUrl { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime UpdatedAt { get; set; }
        
        public virtual ICollection<Result> Results { get; set; } = new List<Result>();
        public virtual ICollection<UserAchievement> UserAchievements { get; set; } = new List<UserAchievement>();
    }

    public enum UserRole
    {
        User,
        Admin,
        Moderator
    }
}