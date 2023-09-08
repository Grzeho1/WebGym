using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NEWG.Models;
using WebGym.Data;
using Newtonsoft.Json;

namespace WebGym.Controllers
{
    public class ExercisesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExercisesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Exercises
        public async Task<IActionResult> Index()
        {
            var applicationContext = _context.Exercises.Include(e => e.Category);
            return View(await applicationContext.ToListAsync());
        }

        // GET: Exercises/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Exercises == null)
            {
                return NotFound();
            }

            var exercise = await _context.Exercises
                .FirstOrDefaultAsync(m => m.ExerciseId == id);
            if (exercise == null)
            {
                return NotFound();
            }

            return View(exercise);
        }

        // GET: Exercises/AddOrEdit
        public IActionResult AddOrEdit(int id=0)
        {

                CategoryList();
            if (id == 0)
                return View(new Exercise());
            else

                return View(_context.Exercises.Find(id));
           
            
        }

        // POST: Exercises/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddorEdit([Bind("ExerciseId,CategoryId,Name,Description")] Exercise exercise)
        {
            if (ModelState.IsValid)
            {
                if (exercise.ExerciseId == 0 )
                
                    _context.Add(exercise);
                
                else
                    _context.Update(exercise);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            CategoryList();
            return View(exercise);
        }

        //CategoryList 
        [NonAction]
        public void CategoryList()
        {
            var categoryList = _context.Categories.ToList();
            Category category = new Category() { CategoryId = 0, Name = "Select" };
            categoryList.Insert(0, category);
            ViewBag.Category = categoryList;
        }
      

        // POST: Exercises/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Exercises == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Exercises'  is null.");
            }
            var exercise = await _context.Exercises.FindAsync(id);
            if (exercise != null)
            {
                _context.Exercises.Remove(exercise);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExerciseExists(int id)
        {
          return (_context.Exercises?.Any(e => e.ExerciseId == id)).GetValueOrDefault();
        }
    }
}
