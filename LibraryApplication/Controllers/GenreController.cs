using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LibraryApplication.Models;
using LibraryApplication.Utility;

namespace LibraryApplication.Controllers
{
    [Authorize(Roles = StaticDetails.AdminUserRole)]
    public class GenreController : Controller
    {
        //creates object for connecting to the database and accessing the Genre table
        private ApplicationDbContext db;

        public GenreController()
        {
            db = new ApplicationDbContext();
        }

        // GET: Genre
        public ActionResult Index()
        {
            //Retrieves Genres from the database, converts to list and passes it to the view
            return View(db.Genres.ToList());
        }

        //Get Action
        public ActionResult Create()
        {
            return View();
        }

        //POST Action
        [HttpPost]
        //validates AntiForgeryToken from genre view
        [ValidateAntiForgeryToken]
        public ActionResult Create(Genre genre)
        {
            //Validates whether required attributes are true
            if (ModelState.IsValid)
            {
                //Passes object from view to the database
                db.Genres.Add(genre);
                //Commit changes
                db.SaveChanges();

                //Display index view with new genre that was added
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Details(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //retrieves the genre object from the database
            Genre genre = db.Genres.Find(id);

            if(genre == null)
            {
                return HttpNotFound();
            }
            return View(genre);
        }

        //EDIT GET
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Genre genre = db.Genres.Find(id);
            if (genre == null)
            {
                return HttpNotFound();
            }
            return View(genre);
        }

        //EDIT POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Genre genre)
        {
            if (ModelState.IsValid)
            {
                //checks in the entitystate is modified for the genre object that is being passed
                //database checks genreid and if entity state is modified it will update all the details
                db.Entry(genre).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        //DELETE GET
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //gets the GenreID from database
            Genre genre = db.Genres.Find(id);
            if (genre == null)
            {
                return HttpNotFound();
            }
            return View(genre);
        }

        //DELETE POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            Genre genre = db.Genres.Find(id);
            db.Genres.Remove(genre);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
        }
    }
}