using System.Web.Mvc;

namespace EntityFX.Gdcame.Presentation.Web.WebApp
{
    public class HomeController : System.Web.Mvc.Controller
    {
        public ActionResult Index()
        {
            return Content("Gdcame web api application");
        }
    }
}