using GradLink.Repository.MSSQL.ORM.Context;
using GradLink.Repository.MSSQL.ORM.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace GradLink.Controllers
{
    public class EventsController : Controller
    {
        private readonly GradLinkDbContext _db;

        public EventsController(GradLinkDbContext dbContext)
        {
            _db = dbContext;
        }

        public IActionResult Index()
        {
            var events = _db.Events
                .OrderBy(e => e.EventDate)
                .ToList();

            return View(events);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Event @event)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid input. Please check the event details.";
                return RedirectToAction("Index");
            }

            var exists = _db.Events.Any(e =>
                e.Title == @event.Title &&
                e.EventDate == @event.EventDate);

            if (exists)
            {
                TempData["ErrorMessage"] = "An event with the same title and date already exists.";
                return RedirectToAction("Index");
            }

            _db.Events.Add(@event);
            _db.SaveChanges();

            TempData["SuccessMessage"] = "Event created successfully!";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
