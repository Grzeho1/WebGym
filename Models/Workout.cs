﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;

namespace NEWG.Models
{
    public class Workout
    {
        [Key]
        public int WorkoutId { get; set; }

        public IdentityUser? User { get; set; }
        public string? GymUserId { get; set; }
      
        public DateTime Date { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(10,2)")]
        public decimal Duration { get; set; }
        public string? Notes { get; set; }


        // Relation 
        [JsonIgnore]
        public ICollection<ExerciseRecord> ?ExerciseRecords { get; set; }
        public List<Exercise> ?Exercises { get; set; }

        
    }
}
