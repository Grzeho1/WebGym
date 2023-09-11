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
        public Exercise? Exercise { get; set; }
        public int WorkoutId { get; set; }
        public Workout? Workout { get; set; }
        public int Repetitions { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public int? Weight { get; set; }
        public string? ExecutionSpeed { get; set; }
        public string? Notes { get; set; }

    }
       
}
