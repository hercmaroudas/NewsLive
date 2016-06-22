namespace NewsLive.Angular.Controllers
{
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Welcome to News Live!";

            return View();
        }
    }
}
