using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NEWG.Models;
using WebGym.Data;
using Microsoft.IdentityModel;
using Microsoft.AspNetCore.Identity;

namespace WebGym.Controllers
{
    public class WorkoutsController : Controller
         
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser>? _userManager;
       

        public WorkoutsController(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
            

        }

        // GET: Workouts
        public async Task<IActionResult> Index()
        {
            // Show only records where UserId match loged User
            var loggedUser = await _userManager.GetUserAsync(User);

            if (loggedUser != null)
            {
                var workouts = await _context.Workouts
                    .Where(e => e.User.Id == loggedUser.Id)
                    .ToListAsync();
                return View(workouts);
            }

            // Or return blank grid

            else
            {
                return View();
            }
            
        }

        // GET: Workouts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Workouts == null)
            {
                return NotFound();
            }

            var workout = await _context.Workouts
                .FirstOrDefaultAsync(m => m.WorkoutId == id);
            if (workout == null)
            {
                return NotFound();
            }

            return View(workout);
        }

        // GET: Workouts/AddOrEdit
        public IActionResult AddOrEdit(int id=0)
        {
            if (id == 0)
                return View(new Workout());
            else

                return View(_context.Workouts.Find(id));

        }

        // POST: Workouts/AddOrEdit
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("WorkoutId,GymUserId,Date,Duration,Notes")] Workout workout)
        {
            if (ModelState.IsValid)
            {
                if (workout.WorkoutId == 0)
                {
                    var loggedUser = await _userManager.GetUserAsync(User);
                    workout.GymUserId = loggedUser.Id;
                    workout.User = loggedUser;
                    _context.Add(workout);
                }

                else
                {
                    _context.Workouts.Update(workout);
                }
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                
            }
            
            return View(workout);
        }

       
        // GET: Workouts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Workouts == null)
            {
                return NotFound();
            }

            var workout = await _context.Workouts
                .FirstOrDefaultAsync(m => m.WorkoutId == id);
            if (workout == null)
            {
                return NotFound();
            }

            return View(workout);
        }

        // POST: Workouts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Workouts == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Workouts'  is null.");
            }
            var workout = await _context.Workouts.FindAsync(id);
            if (workout != null)
            {
                _context.Workouts.Remove(workout);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkoutExists(int id)
        {
          return (_context.Workouts?.Any(e => e.WorkoutId == id)).GetValueOrDefault();
        }


        

        

    }
}
