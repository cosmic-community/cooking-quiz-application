using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuizAPI.Models
{
    public class Category
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; } = string.Empty;
        
        public string Slug { get; set; } = string.Empty;
        
        public string? Description { get; set; }
        
        public string? Color { get; set; }
        
        public string? Icon { get; set; }
        
        public int? ParentCategoryId { get; set; }
        public virtual Category? ParentCategory { get; set; }
        
        public virtual ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
        public virtual ICollection<Category> SubCategories { get; set; } = new List<Category>();
    }
}