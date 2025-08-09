using CalenderEventProject.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using CalenderEventProject.Entities;

namespace CalenderEventProject.Controllers
{
    public class EventController : Controller
    {
       private ProjectContext db = new ProjectContext();


        public ActionResult EventList()
        {
            var events = db.Events.Include(x => x.Category).ToList();
            return View(events);
        }

        [HttpGet]
        public ActionResult AddEvent()
        {        
            ViewBag.Categories = new SelectList(db.Categories.ToList(), "CategoryId", "CategoryName");
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken] 
        public ActionResult AddEvent(Event newEvent) 
        {  
            if (ModelState.IsValid) 
            {
                db.Events.Add(newEvent); 
                db.SaveChanges();        

               
                return RedirectToAction("EventList"); 
            }          
            ViewBag.Categories = new SelectList(db.Categories.ToList(), "CategoryId", "CategoryName", newEvent.CategoryId);
            return View(newEvent);
        }

        [HttpGet] 
        public ActionResult Delete(int id)
        {
            var eventToDelete = db.Events.Find(id); 

            if (eventToDelete == null)
            {
                return HttpNotFound(); 
            }

            db.Events.Remove(eventToDelete); 
            db.SaveChanges();                           
            return RedirectToAction("EventList");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var eventToEdit = db.Events.Find(id);

            if (eventToEdit == null)
            {
                return HttpNotFound(); 
            }            
            ViewBag.Categories = new SelectList(db.Categories.ToList(), "CategoryId", "CategoryName", eventToEdit.CategoryId);
            return View(eventToEdit); 
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken] 
        public ActionResult Edit(Event editedEvent) 
        {
            if (ModelState.IsValid)
            {
                
                db.Entry(editedEvent).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();           
                return RedirectToAction("EventList");
            }            
            ViewBag.Categories = new SelectList(db.Categories.ToList(), "CategoryId", "CategoryName", editedEvent.CategoryId);
            return View(editedEvent);
        }

    }
}