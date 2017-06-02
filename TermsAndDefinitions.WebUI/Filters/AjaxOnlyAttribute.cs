using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TermsAndDefinitions.WebUI.CustomAttributes
{
    public class AjaxOnlyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.HttpContext.Request.IsAjaxRequest())
            {
                UrlHelper urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
                RouteData routeData = urlHelper.RouteCollection.GetRouteData(filterContext.HttpContext);
                string action = routeData.Values["action"] as string;
                string controller = routeData.Values["controller"] as string;
                filterContext.HttpContext.Response.Redirect("/error/404");
            }
        }
    }
}