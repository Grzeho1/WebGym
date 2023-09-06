using System.ComponentModel.DataAnnotations;

namespace NEWG.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }

        // Vytvoření vztahu mezi Category a Exercises
        public ICollection<Exercise>? Exercises { get; set; }
    }
}
