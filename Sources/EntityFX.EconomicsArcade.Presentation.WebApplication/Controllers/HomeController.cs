using System.Web.Mvc;
using EntityFX.EconomicsArcade.Presentation.WebApplication.Providers;

namespace EntityFX.EconomicsArcade.Presentation.WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGameDataProvider _gameDataProvider;

        /// <summary>Called before the action method is invoked.</summary>
        /// <param name="filterContext">Information about the current request and action.</param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _gameDataProvider.InitializeSession(User.Identity.Name);
        }

        public HomeController(IGameDataProvider gameDataProvider)
        {
            _gameDataProvider = gameDataProvider;
        }

        public ActionResult Index()
        {
            return View("Index");
        }

        public ActionResult Rating()
        {
            return View("Rating");
        }
    }
}