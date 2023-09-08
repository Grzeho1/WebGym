using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace NEWG.Models
{
    public class Exercise
    {
        [Key]
        public int ExerciseId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Select one")]
        public int CategoryId { get; set; }
        [JsonIgnore]
        public Category? Category { get; set; }
        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }
        

        [NotMapped]
        public string? CategoryName
        {
            get
            {
                return Category == null ? "" : Category.Name;
            }
        }


    }
}
