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
            routes.IgnoreRoute("{controller}/Index");

            routes.MapRoute(
                 name: "SearchDefault",
                 url: "Search/{action}/{queryString}",
                 defaults: new { controller = "Search", action = "Index", queryString = UrlParameter.Optional }
             );

            //  routes.MapRoute(
            //      name: "PreviewTermssPartical",
            //      url: "Term/PreviewProjectsPartical/{terms}",
            //      defaults: new { controller = "Term", action = "PreviewTermsPartical", terms = UrlParameter.Optional }
            //);

            routes.MapRoute(
              name: "TermAll",
              url: "Term/",
              defaults: new { controller = "Term", action = "Index"}
              );

            routes.MapRoute(
             name: "TermSearch",
             url: "Term/Search/{queryString}",
             defaults: new { controller = "Term", action = "Search", queryString = UrlParameter.Optional }
             );

            routes.MapRoute(
               name: "TermDetail",
               url: "Term/{id}",
               defaults: new { controller = "Term", action = "Index", id = UrlParameter.Optional }
               );

            routes.MapRoute(
                name: "TermDefault",
                url: "Term/{action}/{id}",
                defaults: new { controller = "Term", action = "Index", id = UrlParameter.Optional }
                );

            routes.MapRoute(
             name: "ProjectAll",
             url: "Project/",
             defaults: new { controller = "Project", action = "Index" }
             );

            routes.MapRoute(
              name: "ProjectDetail",
              url: "Project/{id}",
              defaults: new { controller = "Project", action = "Index", id = UrlParameter.Optional }
              );

            routes.MapRoute(
                name: "ProjectDefault",
                url: "Project/{action}/{id}",
                defaults: new { controller = "Project", action = "Index", id = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Search", action = "Index" }
                );
        }
    }
}