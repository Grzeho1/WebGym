using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NEWG.Models;
using WebGym.Data;

namespace WebGym.Controllers
{
    public class ExerciseRecordsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExerciseRecordsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ExerciseRecords
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.exerciseRecords.Include(e => e.Exercise).Include(e => e.Workout);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ExerciseRecords/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.exerciseRecords == null)
            {
                return NotFound();
            }

            var exerciseRecord = await _context.exerciseRecords
                .Include(e => e.Exercise)
                .Include(e => e.Workout)
                .FirstOrDefaultAsync(m => m.ExerciseRecordId == id);
            if (exerciseRecord == null)
            {
                return NotFound();
            }

            return View(exerciseRecord);
        }

        // GET: ExerciseRecords/Create
        public IActionResult Create(int workoutId)
        {
            ViewData["ExerciseId"] = new SelectList(_context.Exercises, "ExerciseId", "Name");
            ViewData["WorkoutId"] = workoutId;
            return View();
        }

        // POST: ExerciseRecords/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ExerciseRecordId,ExerciseId,WorkoutId,Repetitions,Weight,ExecutionSpeed,Notes")] ExerciseRecord exerciseRecord)
        {
            if (ModelState.IsValid)
            {
                _context.Add(exerciseRecord);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ExerciseId"] = new SelectList(_context.Exercises, "ExerciseId", "Name", exerciseRecord.ExerciseId);
            ViewData["WorkoutId"] = new SelectList(_context.Workouts, "WorkoutId", "WorkoutId", exerciseRecord.WorkoutId);
            return View(exerciseRecord);
        }

        // GET: ExerciseRecords/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.exerciseRecords == null)
            {
                return NotFound();
            }

            var exerciseRecord = await _context.exerciseRecords.FindAsync(id);
            if (exerciseRecord == null)
            {
                return NotFound();
            }
            ViewData["ExerciseId"] = new SelectList(_context.Exercises, "ExerciseId", "Name", exerciseRecord.ExerciseId);
            ViewData["WorkoutId"] = new SelectList(_context.Workouts, "WorkoutId", "WorkoutId", exerciseRecord.WorkoutId);
            return View(exerciseRecord);
        }

        // POST: ExerciseRecords/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ExerciseRecordId,ExerciseId,WorkoutId,Repetitions,Weight,ExecutionSpeed,Notes")] ExerciseRecord exerciseRecord)
        {
            if (id != exerciseRecord.ExerciseRecordId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(exerciseRecord);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExerciseRecordExists(exerciseRecord.ExerciseRecordId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ExerciseId"] = new SelectList(_context.Exercises, "ExerciseId", "Name", exerciseRecord.ExerciseId);
            ViewData["WorkoutId"] = new SelectList(_context.Workouts, "WorkoutId", "WorkoutId", exerciseRecord.WorkoutId);
            return View(exerciseRecord);
        }

        // GET: ExerciseRecords/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.exerciseRecords == null)
            {
                return NotFound();
            }

            var exerciseRecord = await _context.exerciseRecords
                .Include(e => e.Exercise)
                .Include(e => e.Workout)
                .FirstOrDefaultAsync(m => m.ExerciseRecordId == id);
            if (exerciseRecord == null)
            {
                return NotFound();
            }

            return View(exerciseRecord);
        }

        // POST: ExerciseRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.exerciseRecords == null)
            {
                return Problem("Entity set 'ApplicationDbContext.exerciseRecords'  is null.");
            }
            var exerciseRecord = await _context.exerciseRecords.FindAsync(id);
            if (exerciseRecord != null)
            {
                _context.exerciseRecords.Remove(exerciseRecord);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExerciseRecordExists(int id)
        {
          return (_context.exerciseRecords?.Any(e => e.ExerciseRecordId == id)).GetValueOrDefault();
        }
    }
}
