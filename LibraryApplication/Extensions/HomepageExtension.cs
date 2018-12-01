using LibraryApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryApplication.Extensions
{
    public static class HomepageExtensions
    {
        public static IEnumerable<HomepageModel> GetBookHomepage(this List<HomepageModel> homepages, ApplicationDbContext db = null, string search = null)
        {
            try
            {
                if (db == null) db = ApplicationDbContext.Create();

                homepages = (from b in db.Books
                              select new HomepageModel
                              {
                                  BookID = b.ID,
                                  Title = b.Title,
                                  Description = b.Description,
                                  ImageUrl = b.ImageUrl,
                                  Link = "/BookInformation/Index/" + b.ID,
                              }).ToList();

                if (search != null)
                {
                    return homepages.Where(t => t.Title.ToLower().Contains(search.ToLower())).OrderBy(t => t.Title);
                }
            }
            catch (Exception ex)
            {

            }
            return homepages.OrderBy(t => t.Title);

        }
    }
}