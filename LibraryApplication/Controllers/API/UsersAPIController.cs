using LibraryApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LibraryApplication.Controllers.Api
{
    //UsersAPIController inherits from ApiController
    public class UsersAPIController : ApiController
    {

        private ApplicationDbContext db;
        public UsersAPIController()
        {
            db = ApplicationDbContext.Create();
        }

        //GET METHOD
        //RETRIEVES EMAIL, NAME, OR BIRTHDATE FROM THE DATABASE
        public IHttpActionResult Get(string type, string query = null)
        {

            if (type.Equals("email") && query != null)
            {
                //RETRIEVES AND RETURNS EMAIL FROM DATABASE
                var customerQuery = db.Users.Where(u => u.Email.ToLower().Contains(query.ToLower()));
                return Ok(customerQuery.ToList());
            }

            if (type.Equals("name") && query != null)
            {
                //RETRIEVES FIRST NAME, LAST NAME, AND DATE OF BIRTH AND RETURNS FROM DATABASE
                var customerQuery = from u in db.Users
                                    where u.Email.Contains(query)
                                    select new { u.FirstName, u.LastName, u.BirthDate };
                return Ok(customerQuery.ToList()[0].FirstName + " " + customerQuery.ToList()[0].LastName + ";" + customerQuery.ToList()[0].BirthDate);
            }
            return Ok();
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