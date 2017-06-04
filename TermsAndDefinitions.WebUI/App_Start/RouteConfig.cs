﻿using System;
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
               defaults: new { controller = "Home", action = "Terms"}
            );

            routes.MapRoute(
                  name: "RouteToTerm",
                  url: "Term/{name}/{*catchall}",
                  defaults: new { controller = "Home", action = "Term", name = UrlParameter.Optional }
               );

            routes.MapRoute(
                  name: "Default",
                  url: "{controller}/{action}/{id}",
                  defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
               );



        }
    }
}