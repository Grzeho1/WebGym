using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
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

        public async Task<ActionResult> Index()
        {
            DateTime startD7 = DateTime.Today.AddDays(-6);  // last 7 Days
            DateTime startD30 = DateTime.Today.AddDays(-30);  // Last 30 Days
            DateTime EndD = DateTime.Today;
            //Sum 7days
            //List of Workouts ïn last 7 days
            #region
            List<Workout> Workouts = await _context.Workouts
                .Include(w => w.ExerciseRecords)
                .Where(y => y.Date >= startD7 && y.Date <= EndD).ToListAsync();


            // List od Workouts in last  30 days

            List<Workout> Workouts30 = await _context.Workouts
               .Include(w => w.ExerciseRecords)
               .Where(y => y.Date >= startD30 && y.Date <= EndD).ToListAsync();

            //Total Weight lifted 

            decimal totalWeightLifted = Workouts
                  .SelectMany(w => w.ExerciseRecords)
                  .Sum(rw => rw.TotalWeight ?? 0);
            ViewBag.TotalWeight = totalWeightLifted;

            // Total days lifted

            int daysLifted = Workouts
                .Select(w => w.Date.Date)
                .Distinct()
                .Count();
            ViewBag.DaysLifted = daysLifted;
            #endregion

            //Sum Today
            // List of today workouts
            #region
            List<Workout> TodayWorkouts = await _context.Workouts
                .Include(w => w.ExerciseRecords)
                .Where(y => y.Date == EndD).ToListAsync();

            decimal liftedToday = TodayWorkouts
                .SelectMany(w => w.ExerciseRecords)
                .Sum(rw => rw.TotalWeight ?? 0);

            ViewBag.LiftedToday = liftedToday;

            #endregion

            //Charts control
            #region
            //Donut chart
            ViewBag.categoryCounts = _context.exerciseRecords
            .Include(er => er.Exercise)
            .Include(er => er.Workout)
            .Where(er => er.Exercise != null)       // Only existing exercises
            .Where(er => er.Workout != null && er.Workout.Date >= startD7 && er.Workout.Date <= EndD) // Filtered last 7days
            .GroupBy(er => er.Exercise.CategoryId)   // Group by CategotyId
            .Select(g => new
            {
                CategoryId = g.Key,
                CategoryCount = g.Count(),
                CategoryName = g.First().Exercise.Name,


            })
            .ToList();

            foreach (var categoryCount in ViewBag.categoryCounts)
            {
                var category = _context.Categories.Find(categoryCount.CategoryId); // Load Category for name

            }



            //Line Chart 
            List<LineData> lineWeight = Workouts30
            .GroupBy(j => j.Date)
            .Where(group => group.Key >= startD30 && group.Key <= EndD)
            .Select(k => new LineData()
            {
                day = k.First().Date.ToString("dd-MMM"),
                totalWeightByDay = k.Sum(w => w.ExerciseRecords.Sum(rw => rw.TotalWeight ?? 0))
            }).ToList();


            // Array for 30 days. For data day by day
            string[] last30d = Enumerable.Range(0, 30)
                .Select(i => startD30.AddDays(i).ToString("dd-MMM"))
                .ToArray();


            // ViewBag for Line Chart
            ViewBag.LineData = from day in last30d join totalWeightByDay in lineWeight on day equals totalWeightByDay.day into JoinedDay
                               from totalWeightByDay in JoinedDay.DefaultIfEmpty()
                               select new { day, totalWeightByDay = totalWeightByDay == null ? 0 : totalWeightByDay.totalWeightByDay };

            // ViewBag for candlechart
            #region
            // Average weight lifted by category 

            var categoryAverages = _context.exerciseRecords
            .Include(er => er.Exercise)
            .Include(er => er.Workout)
            .Where(er => er.Exercise != null)  // Only existing exercises
            .Where(er => er.Workout != null && er.Workout.Date >= startD30 && er.Workout.Date <= EndD) // Filtered last 30 days
            .GroupBy(er => er.Exercise.CategoryId) // Group by CategoryId
            .Select(g => new
            {
                CategoryId = g.Key,
                CategoryName = g.First().Exercise.Name,
                AverageWeight = g.Average(rw => rw.TotalWeight ?? 0) // Average
            })
            .ToList();

            
            ViewBag.CategoryAverages = categoryAverages;
            #endregion

            #endregion 
            return View();

            
        }
    }

    // Class for linechart
    public class LineData
    {
        public string? day;
        public decimal totalWeightByDay;
    }
}
