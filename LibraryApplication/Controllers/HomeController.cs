using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LibraryApplication.ViewModel;
using LibraryApplication.Extensions;
using LibraryApplication.Models;

namespace LibraryApplication.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string search = null)
        {
            var homepages = new List<HomepageModel>().GetBookHomepage(ApplicationDbContext.Create(),search);
            //RETURNS TWO ROWS FROM THE DATABASE
            var count = homepages.Count() / 4;
            var model = new List<HomepageBoxViewModel>();

            for(int i = 0; i <= count; i++)
            {

                model.Add(new HomepageBoxViewModel
                {
                    //takes a count of 4 and adds it to Homepages then add to model
                    Homepages = homepages.Skip(i * 4).Take(4)
                });
            }
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}