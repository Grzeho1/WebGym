using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NEWG.Models
{
    public class ExerciseRecord
    {
        [Key]
        public int ExerciseRecordId { get; set; }
        public int ExerciseId { get; set; }
        public int WorkoutId { get; set; }
        public int Repetitions { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Weight { get; set; }
        public string? ExecutionSpeed { get; set; }
        public string? Notes { get; set; }

        // Vytvoření vztahu mezi ExerciseRecords a Exercises
        public Exercise Exercise { get; set; }

        // Vytvoření vztahu mezi ExerciseRecords a Workouts
        public Workout Workout { get; set; }
    }
}
