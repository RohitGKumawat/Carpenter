using Carpenter.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Carpenter.Controllers
{
    public class ItemController : Controller
    {
        private readonly EverytDB _context;

        public ItemController(EverytDB context)
        {
            _context = context;
        }

        public IActionResult Index(int projectId)
        {
            var project = _context.WorkProjects.Include(p => p.UserProfile).FirstOrDefault(p => p.Id == projectId);
            if (project == null) return NotFound();

            ViewBag.ProjectId = projectId;
            ViewBag.ProjectName = project.ProjectName;

            var items = _context.ProjectItems.Where(i => i.WorkProjectId == projectId).ToList();
            return View(items);
        }

        [HttpGet]
        public IActionResult Add(int projectId)
        {
            ViewBag.ProjectId = projectId;
            return View();
        }

        [HttpPost]
        public IActionResult Add(ProjectItem item)
        {
            _context.ProjectItems.Add(item);
            _context.SaveChanges();
            return RedirectToAction("HowToBuild", "Home", new { itemId = item.Id });
            
        }
        // GET: Edit
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _context.ProjectItems.FindAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProjectItem item)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState)
                {
                    Console.WriteLine($"{error.Key}: {string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage))}");
                }

                ModelState.AddModelError("", "Unable to save changes. Try again.");
                return View(item); // Add ValidationSummary in view to see errors
            }

            try
            {
                _context.Update(item);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { projectId = item.WorkProjectId });
            }
            catch (DbUpdateException ex)
            {
                // Log error, return same view with error message
                ModelState.AddModelError("", "Unable to save changes. Try again.");
                return View(item);
            }
        }



    }
}
