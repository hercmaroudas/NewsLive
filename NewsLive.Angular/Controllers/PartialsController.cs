namespace NewsLive.Angular.Controllers
{
    using System.Web.Mvc;

    public class PartialsController : Controller
    {
        public ActionResult Index(string partial)
        {
            return PartialView(partial);
        }
    }
}