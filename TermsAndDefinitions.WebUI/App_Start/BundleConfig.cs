using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace TermsAndDefinitions.WebUI
{
    public class BundleConfig
    {
            public static void Register(BundleCollection bundles)
            {

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                         "~/Scripts/jquery-{version}.min.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                            "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                    "~/Scripts/bootstrap.min.js"));

            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
                                 "~/Content/bootstrap.min.css",
                                 "~/Content/bootstrap-theme.min.css"));


        }
        }
    
}