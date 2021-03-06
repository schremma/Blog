﻿using System.Web;
using System.Web.Optimization;

namespace Blog
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*", "~/Scripts/jquery.unobtrusive-ajax.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

           bundles.Add(new ScriptBundle("~/bundles/fineup").Include(
               "~/Scripts/fineuploader/fineuploader-4.4.0.js"
                ));


            bundles.Add(new StyleBundle("~/Content/style").Include(
                      "~/Content/stylesheet/coolblue.css",
                      "~/Content/fineuploader/fineuploader-4.4.0.css"
                      ));


            bundles.Add(new ScriptBundle("~/Scripts/jquery/mobile").Include("~/Scripts/jquery.mobile*"));
            bundles.Add(new StyleBundle("~/Content/mobile").Include("~/Content/jquery.mobile*"));
        }
    }
}
