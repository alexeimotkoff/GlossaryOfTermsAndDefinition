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
                 url: "Search/{action}/{queryString}",
                 defaults: new { controller = "Search", action = "Index", queryString = UrlParameter.Optional }
             );

            routes.MapRoute(
                name: "PreviewTermssPartical",
                url: "Term/PreviewProjectsPartical/{terms}",
                defaults: new { controller = "Term", action = "PreviewTermsPartical", terms = UrlParameter.Optional }
          );

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
            name: "AddProject",
            url: "Project/AddProject/{project}",
            defaults: new { controller = "Project", action = "AddProject", project = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "PreviewProjectsPartical",
                url: "Project/PreviewProjectsPartical/{id}",
                defaults: new { controller = "Project", action = "PreviewProjectsPartical", id = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "RouteToProjects",
                url: "Project/",
                defaults: new { controller = "Project", action = "IndexProjects" }
                );

            routes.MapRoute(
                name: "RouteToProject",
                url: "Project/{name}/",
                defaults: new { controller = "Project", action = "IndexProject", name = UrlParameter.Optional }
                );
            
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/",
                defaults: new { controller = "Search", action = "Index" }
                );
        }
    }
}