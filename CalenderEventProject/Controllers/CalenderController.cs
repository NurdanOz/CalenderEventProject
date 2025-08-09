using CalenderEventProject.Context;
using CalenderEventProject.Entities;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System;

namespace EventCalendar.Controllers
{
    public class CalendarController : Controller
    {
        private readonly ProjectContext db = new ProjectContext();

       
        public ActionResult Index()
        {
            return View();
        }


        // CalendarController.cs

        public JsonResult GetEvents()
        {
            var events = db.Events.Include(e => e.Category).ToList();

            var result = events.Select(e => new
            {
                id = e.EventId,
                title = e.Title,
                // Tarih formatını ISO 8601 formatına çevirerek gönderiyoruz.
                // FullCalendar bu formatı daha kolay tanır.
                start = e.StartDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                end = e.EndDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                allDay = e.AllDay,
                // Kategoriye ait rengi çekiyoruz.
                backgroundColor = e.Category != null ? e.Category.Color : "#3788d8",
                borderColor = e.Category != null ? e.Category.Color : "#3788d8"
            }).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetLastEvents()
        {
            var events = db.Events.Include(e => e.Category).OrderByDescending(e => e.EventId).Take(5).ToList();

            var result = events.Select(e => new
            {
                id = e.EventId,
                title = e.Title,
                start = e.StartDate.ToString("s"),
                end = e.EndDate.ToString("s"),
                categoryId = e.CategoryId,
                color = e.Category != null ? e.Category.Color : "#3788d8"
            }).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCategories()
        {
            var categories = db.Categories.ToList();

            var result = categories.Select(c => new
            {
                id = c.CategoryId,
                text = c.CategoryName,
                color = c.Color
            }).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult AddEvent(string title, DateTime start, DateTime end, int categoryId)
        {
            var newEvent = new Event
            {
                Title = title,
                StartDate = start,
                EndDate = end,
                CategoryId = categoryId
            };

            db.Events.Add(newEvent);
            db.SaveChanges();

            return Json(new { success = true, eventId = newEvent.EventId });
        }

        
        [HttpPost]
        public JsonResult UpdateEventDate(int id, string start, string end)
        {
            var existingEvent = db.Events.Find(id);

            if (existingEvent == null)
            {
                return Json(new { success = false, message = "Event not found" });
            }

            
            existingEvent.StartDate = DateTime.Parse(start);
            existingEvent.EndDate = DateTime.Parse(end);

            db.Entry(existingEvent).State = EntityState.Modified;
            db.SaveChanges();

            return Json(new { success = true });
        }

       
        [HttpPost]
        public JsonResult DeleteEvent(int id)
        {
            var existingEvent = db.Events.Find(id);

            if (existingEvent == null)
            {
                return Json(new { success = false, message = "Event not found" });
            }

            db.Events.Remove(existingEvent);
            db.SaveChanges();

            return Json(new { success = true });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


       
    }
}