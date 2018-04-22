using System.Web.Optimization;

namespace FBru.WebAdmin
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = false;

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/timepicker").Include(
                "~/Scripts/timepicki.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
                "~/Scripts/DataTables/jquery.datatables.js",
                "~/Scripts/DataTables/datatables.bootstrap.js",
                "~/Content/DataTables-Responsive/dataTables.responsive.js"));

            bundles.Add(new ScriptBundle("~/bundles/datepicker").Include(
                "~/Scripts/datepicker.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/ckeditor").Include(
                "~/Scripts/CkEditor/ckeditor.js"));

            bundles.Add(new ScriptBundle("~/bundles/typeahead").Include(
                "~/Scripts/typeahead.bundle.js"));

            bundles.Add(new ScriptBundle("~/bundles/lib").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js",
                "~/Scripts/bootbox.js",
                "~/Scripts/toastr.js",
                "~/Scripts/scripts.js"));


            bundles.Add(new StyleBundle("~/content/lib").Include(
                "~/Content/bootstrap.css",
                "~/Content/site.css",
                "~/Content/font-awesome.min.css",
                "~/Content/toastr.css"
            ));

            bundles.Add(new StyleBundle("~/content/datatables").Include(
                "~/Content/DataTables/css/datatables.bootstrap.css",
                "~/Content/DataTables-Responsive/dataTables.responsive.css"));

            bundles.Add(new StyleBundle("~/content/timepicker").Include(
                "~/Content/timepicki.min.css"));

            bundles.Add(new StyleBundle("~/content/datepicker").Include(
                "~/Content/datepicker.min.css"));
        }
    }
}