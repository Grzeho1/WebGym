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
        // Total workouts Last 7 days
        public async Task<ActionResult> Index()
        {
            DateTime startD7 = DateTime.Today.AddDays(-6);
            DateTime startD30 = DateTime.Today.AddDays(-30);
            DateTime EndD = DateTime.Today;

            List<Workout> Workouts = await _context.Workouts
                .Include(w => w.ExerciseRecords)
                .Where(y => y.Date >= startD7 && y.Date <= EndD).ToListAsync();

            int totalWeightLifted = Workouts
                  .SelectMany(w => w.ExerciseRecords)
                  .Sum(rw => rw.Weight ?? 0);
            ViewBag.TotalWeight = totalWeightLifted;

            int daysLifted = Workouts
                .Select(w => w.Date.Date)
                .Distinct()
                .Count();
            ViewBag.DaysLifted = daysLifted;


            ViewBag.categoryCounts = _context.exerciseRecords
        .Include(er => er.Exercise) // Načtení vazby na Exercise
        .Where(er => er.Exercise != null) // Filtrujte pouze záznamy s existujícím Exercise
        .GroupBy(er => er.Exercise.CategoryId) // Zgrupujte podle CategoryId
        .Select(g => new
        {
            CategoryId = g.Key,
            CategoryCount = g.Count(),
            CategoryName = g.First().Exercise.Name
        })
        .ToList();

            foreach (var categoryCount in ViewBag.categoryCounts)
            {
                var category = _context.Categories.Find(categoryCount.CategoryId); // Načtení kategorie pro získání názvu
                
            }




            //DONAT






            return View();
        }
    }
}
