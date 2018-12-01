using LibraryApplication.Models;
using LibraryApplication.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LibraryApplication.Controllers.Api
{
    public class BookAPIController : ApiController
    {
        private ApplicationDbContext db;
        public BookAPIController()
        {
            db = ApplicationDbContext.Create();
        }

        public IHttpActionResult Get(string query = null)
        {
            var bookQuery = db.Books.Where(b => b.Title.ToLower().Contains(query.ToLower()));

            return Ok(bookQuery.ToList());
        }

        //Type could be price or avail
        public IHttpActionResult Get(string type, string isbn = null, string rentalDuration = null, string email = null)
        {
            if (type.Equals("price"))
            {
                Book BookQuery = db.Books.Where(b => b.ISBN.Equals(isbn)).SingleOrDefault();
                var chargeRate = from u in db.Users
                                 join m in db.Memberships on u.MembershipID equals m.ID
                                 where u.Email.Equals(email)
                                 select new { m.ChargeRateSixMonth, m.ChargeRateTwelveMonth };

                var price = Convert.ToDouble(BookQuery.Price) * Convert.ToDouble(chargeRate.ToList()[0].ChargeRateSixMonth) / 100;

                if (rentalDuration == StaticDetails.TwelveMonthCount)
                {
                    price = Convert.ToDouble(BookQuery.Price) * Convert.ToDouble(chargeRate.ToList()[0].ChargeRateTwelveMonth) / 100;
                }
                return Ok(price);
            }
            else
            {
                Book BookQuery = db.Books.Where(b => b.ISBN.Equals(isbn)).SingleOrDefault();
                return Ok(BookQuery.Availability);
            }

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