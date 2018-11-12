using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LibraryApplication
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("BookByReleaseMonth",
                            "Book/ReleaseMonth/{year}/{month}",
                            new { Controller = "book", Action = "ReleaseMonth" });

            routes.MapRoute(
                "BookByReleaseYearAndAuthor",
                "Book/ReleaseYearAndAuthor/{year}/{author}",
                new { Controller = "book", Action = "ReleaseYearAndAuthor" },
                constraints: new { year = @"\d{4}" }
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
