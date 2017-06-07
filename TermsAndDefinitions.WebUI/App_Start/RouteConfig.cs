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
              defaults: new { controller = "Term", action = "Index" }
              );


            routes.MapRoute(
              name: "TermAdd",
              url: "Term/Add/{term}",
              defaults: new { controller = "Term", action = "Add", term = UrlParameter.Optional }
              );


            routes.MapRoute(
              name: "TermEdit",
              url: "Term/Edit/{id}",
              defaults: new { controller = "Term", action = "Edit", id = UrlParameter.Optional }
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
                name: "ProjectAdd",
                url: "Project/Add/{project}",
                defaults: new { controller = "Project", action = "Add", project = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "ProjectEdit",
                url: "Project/Edit/{id}",
                defaults: new { controller = "Project", action = "Edit", id = UrlParameter.Optional }   
                );

            routes.MapRoute(
             name: "ProjectSearch",
             url: "Project/Search/{id}",
             defaults: new { controller = "Project", action = "Search", id = UrlParameter.Optional }
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