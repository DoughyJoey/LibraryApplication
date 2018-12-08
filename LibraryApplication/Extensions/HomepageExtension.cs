using LibraryApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryApplication.Extensions
{
    public static class HomepageExtensions
    {
        //retruns a list of book thumbnails
        public static IEnumerable<HomepageModel> GetBookHomepage(this List<HomepageModel> homepages, ApplicationDbContext db = null, string search = null)
        {
            try
            {
                if (db == null) db = ApplicationDbContext.Create();

                //retrieves all books from the database
                homepages = (from b in db.Books
                              select new HomepageModel
                              {
                                  BookID = b.ID,
                                  Title = b.Title,
                                  Description = b.Description,
                                  ImageUrl = b.ImageUrl,
                                  //links to book information controller
                                  Link = "/BookInformation/Index/" + b.ID,
                              }).ToList();

                //if search is not null
                if (search != null)
                {
                    //returns the book according to title
                    return homepages.Where(t => t.Title.ToLower().Contains(search.ToLower())).OrderBy(t => t.Title);
                }
            }
            catch (Exception ex)
            {

            }
            //orders by the book title
            return homepages.OrderBy(t => t.Title);

        }
    }
}