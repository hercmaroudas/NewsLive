using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace NewsLive.Angular
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            CouchbaseConfig.Initialize();

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        /// <summary>
        /// Disposes of resources when the application stops. 
        /// This includes closing the connection to the Couchbase Cluster and releasing memory.
        /// </summary>
        protected void Application_End()
        {
            CouchbaseConfig.Close();
        }
    }
}
