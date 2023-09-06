using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using NEWG.Models;

namespace WebGym.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }

        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<ExerciseRecord> exerciseRecords { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Workout> Workouts { get; set; }

    }
}