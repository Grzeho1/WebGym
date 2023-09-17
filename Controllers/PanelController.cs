using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NEWG.Models;
using Syncfusion.EJ2.PivotView;
using WebGym.Data;

namespace WebGym.Controllers
{
    public class PanelController : Controller
    {
        private readonly ApplicationDbContext _context;
        public PanelController(ApplicationDbContext context)
        {
            _context = context;
        }
             // Summary of 7 days 

        public async Task<ActionResult> Index()
        {
            DateTime startD7 = DateTime.Today.AddDays(-6);  // last 7 Days
            DateTime startD30 = DateTime.Today.AddDays(-30);  // Last 30 Days
            DateTime EndD = DateTime.Today;

            //List of Workouts ïn last 7 days

            List<Workout> Workouts = await _context.Workouts
                .Include(w => w.ExerciseRecords)
                .Where(y => y.Date >= startD7 && y.Date <= EndD).ToListAsync();

            //Total Weight lifted 

            int totalWeightLifted = Workouts
                  .SelectMany(w => w.ExerciseRecords)
                  .Sum(rw => rw.Weight ?? 0);
            ViewBag.TotalWeight = totalWeightLifted;

            // Total days lifted

            int daysLifted = Workouts
                .Select(w => w.Date.Date)
                .Distinct()
                .Count();
            ViewBag.DaysLifted = daysLifted;



            //Donut Chart control

            #region
            ViewBag.categoryCounts = _context.exerciseRecords
            .Include(er => er.Exercise) 
            .Where(er => er.Exercise != null) // Only existing exercises
            .GroupBy(er => er.Exercise.CategoryId) // Group by CategotyId
            .Select(g => new
            {
                CategoryId = g.Key,
                CategoryCount = g.Count(),
                CategoryName = g.First().Exercise.Name
            })
            .ToList();

                foreach (var categoryCount in ViewBag.categoryCounts)
                {
                    var category = _context.Categories.Find(categoryCount.CategoryId); // Load Category for name
                
                }

               return View();

            #endregion
        }
    }
}
