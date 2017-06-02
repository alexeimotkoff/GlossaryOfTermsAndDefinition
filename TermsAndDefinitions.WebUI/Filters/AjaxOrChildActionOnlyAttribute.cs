using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TermsAndDefinitions.WebUI.CustomAttributes
{
    public class AjaxOrChildActionOnlyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.HttpContext.Request.IsAjaxRequest() && !filterContext.IsChildAction)
            {
                UrlHelper urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
                RouteData routeData = urlHelper.RouteCollection.GetRouteData(filterContext.HttpContext);
                string controller = routeData.Values["controller"] as string;

                RedirectToRoute(filterContext,
                    new
                    {
                        controller = controller,
                        action = "Index"
                    });
            }
        }
            private void RedirectToRoute(ActionExecutingContext context, object routeValues)
            {
                var rc = new RequestContext(context.HttpContext, context.RouteData);
                string url = RouteTable.Routes.GetVirtualPath(rc,
                    new RouteValueDictionary(routeValues)).VirtualPath;
                context.HttpContext.Response.Redirect(url, true);
            }
        }
}