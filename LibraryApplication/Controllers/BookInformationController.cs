using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using LibraryApplication.Models;
using Microsoft.AspNet.Identity;
using LibraryApplication.Utility;
using LibraryApplication.ViewModel;


namespace LibraryApplication.Controllers
{
    public class BookInformationController : Controller
    {
        private ApplicationDbContext db;

        public BookInformationController()
        {
            db = ApplicationDbContext.Create();
        }

        // GET: BOOK INFORMATION
        public ActionResult Index(int id)
        {
            var userid = User.Identity.GetUserId();
            var user = db.Users.FirstOrDefault(u => u.Id == userid);
            var bookModel = db.Books.Include(b => b.Genre).SingleOrDefault(b => b.ID == id);
            var rentalPrice = 0.0;
            var sixMonthRental = 0.0;
            var twelveMonthRental = 0.0;

            //if the user is not an admin
            if (userid != null && !User.IsInRole(StaticDetails.AdminUserRole))
            {
                //retrieves the charge rates for 6 months and 12 months
                var chargeRate = from u in db.Users
                                 join m in db.Memberships on u.MembershipID equals m.ID
                                 where u.Id.Equals(userid)
                                 select new { m.ChargeRateSixMonth, m.ChargeRateTwelveMonth };
                sixMonthRental = Convert.ToDouble(bookModel.Price) * Convert.ToDouble(chargeRate.ToList()[0].ChargeRateSixMonth) / 100;
                twelveMonthRental = Convert.ToDouble(bookModel.Price) * Convert.ToDouble(chargeRate.ToList()[0].ChargeRateTwelveMonth) / 100;
            }

            BookRentalViewModel model = new BookRentalViewModel
            {

                BookID = bookModel.ID,
                ISBN = bookModel.ISBN,
                Author = bookModel.Author,
                Availability = bookModel.Availability,
                DateAdded = bookModel.DateAdded,
                Description = bookModel.Description,
                Genre = db.Genres.FirstOrDefault(g => g.ID.Equals(bookModel.GenreID)),
                GenreID = bookModel.GenreID,
                ImageUrl = bookModel.ImageUrl,
                Pages = bookModel.Pages,
                Price = bookModel.Price,
                Publisher = bookModel.Publisher,
                PublicationDate = bookModel.PublicationDate,
                //ProductDimensions = bookModel.ProductDimensions,
                Title = bookModel.Title,
                UserID = userid,
                RentalPrice = rentalPrice,
                RentalPriceSixMonth = sixMonthRental,
                RentalPriceTwelveMonth = twelveMonthRental

            };
            return View(model);
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