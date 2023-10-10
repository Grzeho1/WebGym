using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using NEWG.Models;
using Syncfusion.EJ2.PivotView;
using System.Security.Policy;
using WebGym.Data;

namespace WebGym.Controllers
{
    //Acces Only for logged users
    [Authorize(Policy = "RequireLoggedIn")]
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
            if (User.Identity.IsAuthenticated)

            {
                decimal totalWeightLifted = Workouts
                      .SelectMany(w => w.ExerciseRecords)
                      .Sum(rw => rw.TotalWeight ?? 0);
                ViewBag.TotalWeight = totalWeightLifted;
            }
            else return View();
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
            if (User.Identity.IsAuthenticated)
            {
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
                    CategoryName = _context.Categories.FirstOrDefault(cat => cat.CategoryId == g.Key).Name


                })
                .ToList();
            }
            else
                return View();

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

            // ViewBag for Line Chart 2

            



            #region
            // Average weight lifted by category 

            var categoryAverages = _context.exerciseRecords
            .Include(er => er.Exercise)
            .Include(er => er.Workout)
            .Where(er => er.Exercise != null)  // Only existing exercises
            .Where(er => er.Workout != null && er.Workout.Date >= startD30 && er.Workout.Date <= EndD) // Filtered last 30 days
            .GroupBy(er => er.Exercise.ExerciseId) // Group by ExId
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

        // Get data by choosen range  ***(JS)***
        [HttpGet]
        public IActionResult GetData(string selectedValue)
        {
            DateTime startDate;
            DateTime endDate = DateTime.Today;

            switch (selectedValue)
            {
                case "7Days":
                    startDate = endDate.AddDays(-6);
                    break;
                case "30Days":
                    startDate = endDate.AddDays(-29);
                    break;
                case "365Days":
                    startDate = endDate.AddDays(-360);
                    break;
                default:
                    startDate = endDate;
                    break;
            }

            // DropDown list data 7/30 days  Dynamic

            var categoryCounts = _context.exerciseRecords
                .Include(er => er.Exercise)
                .Include(er => er.Workout)
                .Where(er => er.Exercise != null)       // Only existing exercises
                .Where(er => er.Workout != null && er.Workout.Date >= startDate && er.Workout.Date <= endDate) // Filtered last 7days
                .GroupBy(er => er.Exercise.CategoryId)   // Group by CategotyId
                .Select(g => new
                {
                    CategoryId = g.Key,
                    CategoryCount = g.Count(),
                    CategoryName = _context.Categories.FirstOrDefault(cat => cat.CategoryId == g.Key).Name


                })
                .ToList();

         
                

                var workouts = _context.Workouts
               .Include(w => w.ExerciseRecords)
               .Where(y => y.Date >= startDate && y.Date <= endDate)
               .ToList();

            // Total Weight
                decimal totalWeight = workouts
                    .SelectMany(w => w.ExerciseRecords)
                    .Sum(rw => rw.TotalWeight ?? 0);

            // Days lifted
                int daysLifted = workouts
                    .Select(w => w.Date.Date)
                    .Distinct()
                    .Count();


            // Data for View

                var data = new
                {
                    totalWeight = totalWeight +  "kg",
                    daysLifted = daysLifted,
                    categoryCounts = categoryCounts,
                  
                };


                return Json(data);
        }
    }

  

    // Class for linechart
    public class LineData
    {
        public string? day;
        public decimal totalWeightByDay;
       
        
    }
}
