namespace NewsLive.Angular
{
    using Bmbsqd.JilMediaFormatter;
    using Handlers;
    using System.Web.Http;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Formatters.Add(new JilMediaTypeFormatter(Jil.Options.CamelCase));
            config.Formatters.Remove(config.Formatters.JsonFormatter);

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "ApiWithAction",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional });

            //http://localhost/NewsLive.Angular/api/membership/klwk/sss

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            config.MessageHandlers.Insert(0, new CompressionHandler());
        }
    }
}
