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
    //only an authorized user can go into the BookRentController
    [Authorize]
    public class BookRentalController : Controller
    {
        private ApplicationDbContext db;
        public BookRentalController()
        {
            db = ApplicationDbContext.Create();
        }


        //CREATE GET
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


        //CREATE POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookRentalViewModel bookRent)
        {
            if (ModelState.IsValid)
            {
                //retrieve email
                var email = bookRent.Email;

                //retrieve details
                var userDetails = from u in db.Users
                                  where u.Email.Equals(email)
                                  select new { u.Id, u.FirstName, u.LastName, u.BirthDate };

                //store isbn in variable
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

                //decreases availability when user requests a book
                bookSelected.Availability -= 1;
                db.BookRentals.Add(modelToAddToDb);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View();
        }



        //GET METHOD
        public ActionResult Index(int? pageNumber, string search = null)
        {
            string userid = User.Identity.GetUserId();

            //JOINS TABLES
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
                            PublicationDate = b.PublicationDate,
                            Publisher = b.Publisher,
                            RentalDuration = br.RentalDuration,
                            Status = br.Status.ToString(),
                            Title = b.Title,
                            UserID = u.Id

                        };


            //if the user is not an admin they can only see their rentals
            if (!User.IsInRole(StaticDetails.AdminUserRole))
            {
                model = model.Where(u => u.UserID.Equals(userid));
            }

            //pageNumber uses pagination so each page displays 5 rows
            return View(model.ToList().ToPagedList(pageNumber ?? 1, 5));
        }


        //POST METHOD
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
                //finds record from the database
                var bookInDb = db.Books.SingleOrDefault(c => c.ID == book.BookID);
                //update availability
                bookInDb.Availability -= 1;
                db.SaveChanges();
                return RedirectToAction("Index", "BookRental");

            }

            return View();
        }



        //GET DETAILS
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //will find rental record from the database
            BookRental bookRent = db.BookRentals.Find(id);

            //references private function getVMFromBookRent()
            var model = getVMFromBookRent(bookRent);

            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }


        //DECLINE GET
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

        //DECLINE POST
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

                //find book from database
                Book bookInDb = db.Books.Find(bookRent.BookID);
                //update the availability
                bookInDb.Availability += 1;
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }


        //APPROVE GET
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


        //APPROVE POST
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



        //PICKUP GET
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


        //PICKUP POST
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



        //RETURN GET
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


        //RETURN POST
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
                //allows admin to add additional charge
                bookRent.AdditionalCharge = model.AdditionalCharge;


                Book bookInDb = db.Books.Find(bookRent.BookID);
                //update book availability
                bookInDb.Availability += 1;

                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }



        //DELETE GET
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



        //DELETE POST
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
                //remove record from the database
                db.BookRentals.Remove(bookRent);

                //gets the book object
                var bookInDb = db.Books.Where(b => b.ID.Equals(bookRent.BookID)).FirstOrDefault();

                //if the status is rented update the availability
                if (bookRent.Status.ToString().ToLower().Equals("rented"))
                {
                    bookInDb.Availability += 1;
                }

                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }


        //converts bookrent object into a BookRentalViewModel object
        private BookRentalViewModel getVMFromBookRent(BookRental bookRent)
        {
            //retrieves book from the database
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