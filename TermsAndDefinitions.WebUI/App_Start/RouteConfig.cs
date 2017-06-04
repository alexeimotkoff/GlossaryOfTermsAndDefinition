using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TermsAndDefinitions.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                 name: "SearchRoute",
                 url: "Search/{action}/{*catchall}",
                 defaults: new { controller = "Search", action = "Index" }
             );
            //routes.MapRoute(
            //       name: "SearchRoute",
            //       url: "Search/{*catchall}",
            //       defaults: new { controller = "Search", action = "All" }
            //   );

            routes.MapRoute(
               name: "RouteToTerms",
               url: "Term/",
               defaults: new { controller = "Term", action = "IndexTerms" }
            );

            routes.MapRoute(
                  name: "RouteToTerm",
                  url: "Term/{name}/",
                  defaults: new { controller = "Term", action = "IndexTerm", name = UrlParameter.Optional }
               );

            routes.MapRoute(
                  name: "Default",
                  url: "{controller}/{action}/{id}",
                  defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
               );



        }
    }
}