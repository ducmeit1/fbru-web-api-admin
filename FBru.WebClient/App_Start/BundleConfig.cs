using System.Web.Optimization;

namespace FBru.WebClient
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = false;

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/lib").Include(
                "~/Scripts/popper.js",
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js",
                "~/Scripts/imagesloaded.pkgd.min.js",
                "~/Scripts/jquery.waypoints.min.js",
                "~/Scripts/typeahead.bundle.js",
                "~/Scripts/owl.carousel.min.js",
                "~/Scripts/toastr.js",
                "~/Scripts/scripts.js"));

            bundles.Add(new StyleBundle("~/content/lib").Include(
                "~/Content/bootstrap.css",
                "~/Content/font-awesome.min.css",
                "~/Content/toastr.min.css",
                "~/Content/owl.carousel.min.css",
                "~/Content/owl.theme.default.min.css",
                "~/Content/animate.css",
            "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
                "~/Scripts/DataTables/jquery.datatables.js",
                "~/Scripts/DataTables/datatables.bootstrap.js",
                "~/Content/DataTables-Responsive/dataTables.responsive.js"));

            bundles.Add(new StyleBundle("~/content/datatables").Include(
                "~/Content/DataTables/css/datatables.bootstrap.css",
                "~/Content/DataTables-Responsive/dataTables.responsive.css"));
        }
    }
}