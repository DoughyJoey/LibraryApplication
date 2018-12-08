using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LibraryApplication.Models;
using LibraryApplication.Utility;
using LibraryApplication.ViewModel;

namespace LibraryApplication.Controllers
{
    [Authorize(Roles = StaticDetails.AdminUserRole)]
    public class BookController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Book
        public ActionResult Index()
        {
            //uses entity to link Books and Genre tables and stores in books
            var books = db.Books.Include(b => b.Genre);
            return View(books.ToList());
        }

        // GET: Book/Details/5
        public ActionResult Details(int? ID)
        {
            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(ID);
            if (book == null)
            {
                return HttpNotFound();
            }

            var model = new BookViewModel
            {
                Book = book,
                Genres = db.Genres.ToList()
            };

            return View(model);
        }

        // GET: Book/Create
        public ActionResult Create()
        {
            var genre = db.Genres.ToList();
            var model = new BookViewModel
            {
                Genres = genre
            };
            return View(model);
        }

        // POST: Book/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookViewModel bookVM)
        {
            //convert BookViewModel into an object

            var book = new Book
            {
                Author = bookVM.Book.Author,
                Availability = bookVM.Book.Availability,
                DateAdded = bookVM.Book.DateAdded,
                Description = bookVM.Book.Description,
                Genre = bookVM.Book.Genre,
                GenreID = bookVM.Book.GenreID,
                ImageUrl = bookVM.Book.ImageUrl,
                ISBN = bookVM.Book.ISBN,
                Pages = bookVM.Book.Pages,
                Price = bookVM.Book.Price,
                Publisher = bookVM.Book.Publisher,
                //ProductDimensions = bookVM.Book.ProductDimensions,
                PublicationDate = bookVM.Book.PublicationDate,
                Title = bookVM.Book.Title
            };

            if (ModelState.IsValid)
            {
                db.Books.Add(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            bookVM.Genres = db.Genres.ToList();
            return View(bookVM);
        }

        // GET: Book/Edit/5
        public ActionResult Edit(int? ID)
        {
            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(ID);
            if (book == null)
            {
                return HttpNotFound();
            }
            //Create an new BookViewModel object called model
            var model = new BookViewModel
            {
                //pass in Book
                Book = book,
                //get genre from the database
                Genres = db.Genres.ToList()
            };
            //pass model to the view
            return View(model);
        }

        // POST: Book/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(BookViewModel bookVM)
        {
            var book = new Book
            {

                //convert BookViewModel into an object
                ID = bookVM.Book.ID,
                Author = bookVM.Book.Author,
                Availability = bookVM.Book.Availability,
                DateAdded = bookVM.Book.DateAdded,
                Description = bookVM.Book.Description,
                Genre = bookVM.Book.Genre,
                GenreID = bookVM.Book.GenreID,
                ImageUrl = bookVM.Book.ImageUrl,
                ISBN = bookVM.Book.ISBN,
                Pages = bookVM.Book.Pages,
                Price = bookVM.Book.Price,
                //ProductDimensions = bookVM.Book.ProductDimensions,
                PublicationDate = bookVM.Book.PublicationDate,
                Publisher = bookVM.Book.Publisher,
                Title = bookVM.Book.Title
            };
            if (ModelState.IsValid)
            {
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //only called if the model state is not valid
            bookVM.Genres = db.Genres.ToList();
            return View(bookVM);
        }

        // GET: Book/Delete/5
        public ActionResult Delete(int? ID)
        {
            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(ID);
            if (book == null)
            {
                return HttpNotFound();
            }

            var model = new BookViewModel
            {
                Book = book,
                Genres = db.Genres.ToList()
            };

            return View(model);
        }

        // POST: Book/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int ID)
        {
            //finds ID from the database
            Book book = db.Books.Find(ID);
            //removes book
            db.Books.Remove(book);
            db.SaveChanges();
            return RedirectToAction("Index");
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
