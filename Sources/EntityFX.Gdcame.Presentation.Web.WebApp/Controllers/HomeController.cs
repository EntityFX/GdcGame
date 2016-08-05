using System.Web.Mvc;

namespace EntityFX.Gdcame.Presentation.Web.WebApp.Controllers
{
    [Authorize]
    public class HomeController : System.Web.Mvc.Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = User.Identity.Name;

            return View();
        }
    }
}
