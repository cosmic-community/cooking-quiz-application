using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuizAPI.Models
{
    public class Achievement
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; } = string.Empty;
        
        public string? Description { get; set; }
        
        public string? Icon { get; set; }
        
        public string? Criteria { get; set; }
        
        public int? PointsRequired { get; set; }
        
        public string? BadgeImageUrl { get; set; }
        
        public virtual ICollection<UserAchievement> UserAchievements { get; set; } = new List<UserAchievement>();
    }

    public class UserAchievement
    {
        public int UserId { get; set; }
        public virtual User? User { get; set; }
        
        public int AchievementId { get; set; }
        public virtual Achievement? Achievement { get; set; }
        
        public DateTime EarnedAt { get; set; }
    }
}