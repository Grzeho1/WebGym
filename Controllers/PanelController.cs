using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NEWG.Models;
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
            DateTime startD = DateTime.Today.AddDays(-6);
            DateTime EndD = DateTime.Today;
            List<Workout> WeekWorkouts = await _context.Workouts
                .Include(w => w.Exercises)
                .Where(y => y.Date >= startD && y.Date <= EndD).ToListAsync();


            return View();
        }
    }
}
