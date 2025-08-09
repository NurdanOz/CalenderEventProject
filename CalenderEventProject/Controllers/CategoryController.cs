using CalenderEventProject.Context;
using CalenderEventProject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CalenderEventProject.Controllers
{
    public class CategoryController : Controller
    {
       private ProjectContext db = new ProjectContext();


        public ActionResult Index()
        {
            var categories = db.Categories.ToList();
            return View(categories);
        }

        [HttpGet]
        public ActionResult AddCategory()
        {
           return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] 
        public ActionResult AddCategory(Category category) 
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(category); 
                db.SaveChanges();           

               
                return RedirectToAction("Index"); 
            }

            return View(category);
        }

        [HttpGet] 
        public ActionResult Delete(int id)
        {
            var category = db.Categories.Find(id); 
            if (category == null)
            {
                return HttpNotFound(); 
            }
            db.Categories.Remove(category); 
            db.SaveChanges();                        
            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            var category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound(); 
            }
            return View(category); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken] 
        public ActionResult Edit(Category category) 
        {
            if (ModelState.IsValid) 
            {
                
                db.Entry(category).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();                
                return RedirectToAction("Index");
            }            
            return View(category);
        }

       

    }
}