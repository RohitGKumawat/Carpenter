using Carpenter.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Carpenter.Controllers
{
    public class WorkController : Controller
    {
        private readonly EverytDB _context;

        public WorkController(EverytDB context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Home");

            var works = _context.WorkProjects.Where(w => w.UserProfileId == userId).ToList();
            return View(works);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(WorkProject project)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Home");

            project.UserProfileId = userId.Value;
            _context.WorkProjects.Add(project);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
