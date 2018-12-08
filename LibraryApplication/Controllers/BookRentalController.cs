using Microsoft.AspNet.Identity;
using LibraryApplication.Models;
using LibraryApplication.Utility;
using LibraryApplication.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Net;

namespace LibraryApplication.Controllers
{
    [Authorize]
    public class BookRentalController : Controller
    {
        private ApplicationDbContext db;
        public BookRentalController()
        {
            db = ApplicationDbContext.Create();
        }
        //Get Method
        public ActionResult Create(string title = null, string ISBN = null)
        {
            if (title != null && ISBN != null)
            {
                BookRentalViewModel model = new BookRentalViewModel
                {
                    Title = title,
                    ISBN = ISBN
                };
                return View(model);
            }
            return View(new BookRentalViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookRentalViewModel bookRent)
        {
            if (ModelState.IsValid)
            {
                var email = bookRent.Email;
                var userDetails = from u in db.Users
                                  where u.Email.Equals(email)
                                  select new { u.Id, u.FirstName, u.LastName, u.BirthDate };
                var ISBN = bookRent.ISBN;
                Book bookSelected = db.Books.Where(b => b.ISBN == ISBN).FirstOrDefault();
                var rentalDuration = bookRent.RentalDuration;
                var chargeRate = from u in db.Users
                                 join m in db.Memberships
                                 on u.MembershipID equals m.ID
                                 where u.Email.Equals(email)
                                 select new { m.ChargeRateSixMonth, m.ChargeRateTwelveMonth };

                var sixMonthRental = Convert.ToDouble(bookSelected.Price) * Convert.ToDouble(chargeRate.ToList()[0].ChargeRateSixMonth) / 100;
                var twelveMonthRental = Convert.ToDouble(bookSelected.Price) * Convert.ToDouble(chargeRate.ToList()[0].ChargeRateTwelveMonth) / 100;

                double rentalPr = 0;
                if (bookRent.RentalDuration == StaticDetails.TwelveMonthCount)
                {
                    rentalPr = twelveMonthRental;
                }
                else
                {
                    rentalPr = sixMonthRental;
                }

                BookRentalViewModel model = new BookRentalViewModel
                {
                    BookID = bookSelected.ID,
                    RentalPrice = rentalPr,
                    Price = bookSelected.Price,
                    Pages = bookSelected.Pages,
                    FirstName = userDetails.ToList()[0].FirstName,
                    LastName = userDetails.ToList()[0].LastName,
                    BirthDate = userDetails.ToList()[0].BirthDate,
                    ScheduledReturnDate = bookRent.ScheduledReturnDate,
                    Author = bookSelected.Author,
                    Availability = bookSelected.Availability,
                    DateAdded = bookSelected.DateAdded,
                    Description = bookSelected.Description,
                    Email = email,
                    GenreID = bookRent.GenreID,
                    Genre = db.Genres.Where(g => g.ID.Equals(bookSelected.GenreID)).First(),
                    ISBN = bookSelected.ISBN,
                    ImageUrl = bookSelected.ImageUrl,
                    //ProductDimensions = bookSelected.ProductDimensions,
                    PublicationDate = bookSelected.PublicationDate,
                    Publisher = bookSelected.Publisher,
                    RentalDuration = bookRent.RentalDuration,
                    Status = BookRental.StatusEnum.Requested.ToString(),
                    Title = bookSelected.Title,
                    UserID = userDetails.ToList()[0].Id,
                    RentalPriceSixMonth = sixMonthRental,
                    RentalPriceTwelveMonth = twelveMonthRental
                };
                BookRental modelToAddToDb = new BookRental
                {
                    BookID = bookSelected.ID,
                    RentalPrice = rentalPr,
                    ScheduledReturnDate = bookRent.ScheduledReturnDate,
                    RentalDuration = bookRent.RentalDuration,
                    Status = BookRental.StatusEnum.Approved,
                    UserID = userDetails.ToList()[0].Id
                };
                bookSelected.Availability -= 1;
                db.BookRentals.Add(modelToAddToDb);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: BookRent
        public ActionResult Index(int? pageNumber, string option = null, string search = null)
        {
            string userid = User.Identity.GetUserId();
            var model = from br in db.BookRentals
                        join b in db.Books on br.BookID equals b.ID
                        join u in db.Users on br.UserID equals u.Id

                        select new BookRentalViewModel
                        {
                            ID = br.ID,
                            BookID = b.ID,
                            RentalPrice = br.RentalPrice,
                            Price = b.Price,
                            Pages = b.Pages,
                            FirstName = u.FirstName,
                            LastName = u.LastName,
                            BirthDate = u.BirthDate,
                            ScheduledReturnDate = br.ScheduledReturnDate,
                            Author = b.Author,
                            Availability = b.Availability,
                            DateAdded = b.DateAdded,
                            Description = b.Description,
                            Email = u.Email,
                            StartDate = br.StartDate,
                            GenreID = b.GenreID,
                            Genre = db.Genres.Where(g => g.ID.Equals(b.GenreID)).FirstOrDefault(),
                            ISBN = b.ISBN,
                            ImageUrl = b.ImageUrl,
                            //ProductDimensions = b.ProductDimensions,
                            PublicationDate = b.PublicationDate,
                            Publisher = b.Publisher,
                            RentalDuration = br.RentalDuration,
                            Status = br.Status.ToString(),
                            Title = b.Title,
                            UserID = u.Id

                        };

            if (option == "email" && search.Length > 0)
            {
                model = model.Where(u => u.Email.Contains(search));
            }
            if (option == "name" && search.Length > 0)
            {
                model = model.Where(u => u.FirstName.Contains(search) || u.LastName.Contains(search));
            }
            if (option == "status" && search.Length > 0)
            {
                model = model.Where(u => u.Status.Contains(search));
            }

            if (!User.IsInRole(StaticDetails.AdminUserRole))
            {
                model = model.Where(u => u.UserID.Equals(userid));
            }
            return View(model.ToList().ToPagedList(pageNumber ?? 1, 5));
        }

        [HttpPost]
        public ActionResult Reserve(BookRentalViewModel book)
        {
            var userid = User.Identity.GetUserId();
            Book bookToRent = db.Books.Find(book.BookID);
            double rentalPr = 0;
            if (userid != null)
            {
                var chargeRate = from u in db.Users
                                 join m in db.Memberships
                                 on u.MembershipID equals m.ID
                                 where u.Id.Equals(userid)
                                 select new { m.ChargeRateSixMonth, m.ChargeRateTwelveMonth };
                if (book.RentalDuration == StaticDetails.TwelveMonthCount)
                {
                    rentalPr = Convert.ToDouble(bookToRent.Price) * Convert.ToDouble(chargeRate.ToList()[0].ChargeRateTwelveMonth) / 100;
                }
                else
                {
                    rentalPr = Convert.ToDouble(bookToRent.Price) * Convert.ToDouble(chargeRate.ToList()[0].ChargeRateSixMonth) / 100;
                }
                BookRental bookRent = new BookRental
                {
                    BookID = bookToRent.ID,
                    UserID = userid,
                    RentalDuration = book.RentalDuration,
                    RentalPrice = rentalPr,
                    Status = BookRental.StatusEnum.Requested,
                };
                db.BookRentals.Add(bookRent);
                var bookInDb = db.Books.SingleOrDefault(c => c.ID == book.BookID);
                bookInDb.Availability -= 1;
                db.SaveChanges();
                return RedirectToAction("Index", "BookRental");
            }
            return View();
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookRental bookRent = db.BookRentals.Find(id);
            var model = getVMFromBookRent(bookRent);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        public ActionResult Decline(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookRental bookRent = db.BookRentals.Find(id);
            var model = getVMFromBookRent(bookRent);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Decline(BookRentalViewModel model)
        {
            if (model.ID == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (ModelState.IsValid)
            {
                BookRental bookRent = db.BookRentals.Find(model.ID);
                bookRent.Status = BookRental.StatusEnum.Rejected;

                Book bookInDb = db.Books.Find(bookRent.BookID);
                bookInDb.Availability += 1;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        //APPROVE METHOD
        public ActionResult Approve(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookRental bookRent = db.BookRentals.Find(id);
            var model = getVMFromBookRent(bookRent);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View("Approve", model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Approve(BookRentalViewModel model)
        {
            if (model.ID == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (ModelState.IsValid)
            {
                BookRental bookRent = db.BookRentals.Find(model.ID);
                bookRent.Status = BookRental.StatusEnum.Approved;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        //PickUp Get Method
        public ActionResult PickUp(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookRental bookRent = db.BookRentals.Find(id);
            var model = getVMFromBookRent(bookRent);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View("Approve", model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PickUp(BookRentalViewModel model)
        {
            if (model.ID == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (ModelState.IsValid)
            {
                BookRental bookRent = db.BookRentals.Find(model.ID);
                bookRent.Status = BookRental.StatusEnum.Rented;
                bookRent.StartDate = DateTime.Now;
                if (bookRent.RentalDuration == StaticDetails.TwelveMonthCount)
                {
                    bookRent.ScheduledReturnDate = DateTime.Now.AddMonths(Convert.ToInt32(StaticDetails.TwelveMonthCount));
                }
                else
                {
                    bookRent.ScheduledReturnDate = DateTime.Now.AddMonths(Convert.ToInt32(StaticDetails.SixMonthCount));
                }
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        //Return Get Method
        public ActionResult Return(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookRental bookRent = db.BookRentals.Find(id);
            var model = getVMFromBookRent(bookRent);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View("Approve", model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Return(BookRentalViewModel model)
        {
            if (model.ID == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (ModelState.IsValid)
            {
                BookRental bookRent = db.BookRentals.Find(model.ID);
                bookRent.Status = BookRental.StatusEnum.Closed;
                bookRent.AdditionalCharge = model.AdditionalCharge;

                Book bookInDb = db.Books.Find(bookRent.BookID);
                bookInDb.Availability += 1;

                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        //DELETE GET METHOD
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookRental bookRent = db.BookRentals.Find(id);
            var model = getVMFromBookRent(bookRent);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        //DELETE POST METHOD
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int Id)
        {
            if (Id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (ModelState.IsValid)
            {
                BookRental bookRent = db.BookRentals.Find(Id);
                db.BookRentals.Remove(bookRent);
                var bookInDb = db.Books.Where(b => b.ID.Equals(bookRent.BookID)).FirstOrDefault();
                if (bookRent.Status.ToString().ToLower().Equals("rented"))
                {
                    bookInDb.Availability += 1;
                }
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        private BookRentalViewModel getVMFromBookRent(BookRental bookRent)
        {
            Book bookSelected = db.Books.Where(b => b.ID == bookRent.BookID).FirstOrDefault();
            var userDetails = from u in db.Users
                              where u.Id.Equals(bookRent.UserID)
                              select new { u.Id, u.FirstName, u.LastName, u.BirthDate, u.Email };
            BookRentalViewModel model = new BookRentalViewModel
            {
                ID = bookRent.ID,
                BookID = bookSelected.ID,
                RentalPrice = bookRent.RentalPrice,
                Price = bookSelected.Price,
                Pages = bookSelected.Pages,
                FirstName = userDetails.ToList()[0].FirstName,
                LastName = userDetails.ToList()[0].LastName,
                BirthDate = userDetails.ToList()[0].BirthDate,
                ScheduledReturnDate = bookRent.ScheduledReturnDate,
                Author = bookSelected.Author,
                StartDate = bookRent.StartDate,
                Availability = bookSelected.Availability,
                AdditionalCharge = bookRent.AdditionalCharge,
                DateAdded = bookSelected.DateAdded,
                Description = bookSelected.Description,
                Email = userDetails.ToList()[0].Email,
                GenreID = bookSelected.GenreID,
                Genre = db.Genres.FirstOrDefault(g => g.ID.Equals(bookSelected.GenreID)),
                ISBN = bookSelected.ISBN,
                ImageUrl = bookSelected.ImageUrl,
                //ProductDimensions = bookSelected.ProductDimensions,
                PublicationDate = bookSelected.PublicationDate,
                Publisher = bookSelected.Publisher,
                RentalDuration = bookRent.RentalDuration,
                Status = bookRent.Status.ToString(),
                Title = bookSelected.Title,
                UserID = userDetails.ToList()[0].Id

            };
            return model;
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
        }
    }
}