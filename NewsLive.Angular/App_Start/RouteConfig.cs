namespace NewsLive.Angular
{
    using System.Web.Mvc;
    using System.Web.Routing;

    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            
            routes.MapRoute(
                name: "Partials",
                url: "Partials/{partial}",
                defaults: new { controller = "Partials", action = "Index", partial = "{partial}" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "GetAuthorPagedData",
                url: "api/{controller}/{authorid}/{numResultsPerPage}/{nextPageNum}"
            );
        }
    }
}
