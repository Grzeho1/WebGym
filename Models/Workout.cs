using System.ComponentModel.DataAnnotations;

namespace NEWG.Models
{
    public class Workout
    {
        [Key]
        public int WorkoutId { get; set; }
        public DateTime Date { get; set; }
        public int Duration { get; set; }
        public string? Notes { get; set; }

        // Vytvoření vztahu mezi Workouts a ExerciseRecords
        public ICollection<ExerciseRecord> ExerciseRecords { get; set; }
        public List<Exercise> Exercises { get; set; }
    }
}
