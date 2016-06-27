namespace NewsLive.Angular
{
    using System.Web.Optimization;

    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/app")
                .Include(
                    "~/Scripts/angular.js",
                    "~/Scripts/angular-route.js")
                .IncludeDirectory("~/Scripts/app", "*.js", true)
                .IncludeDirectory("~/Scripts/directives", "*.js", true)
                .IncludeDirectory("~/Scripts/services", "*.js", true));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                                    "~/Scripts/bootstrap.*"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));


            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-theme.css",
                      "~/Content/site.css",
                      "~/Content/cover.css",
                      "~/Content/news.css"));
        }
    }
}
