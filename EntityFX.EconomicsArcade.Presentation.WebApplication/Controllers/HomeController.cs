using System.Configuration;
using System.Web.Http.Results;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using EntityFX.EconomicsArcade.Contract.Common;
using EntityFX.EconomicsArcade.Contract.Manager.UserManager;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Presentation.Models;
using EntityFX.EconomicsArcade.Presentation.WebApplication.App_Start;
using EntityFX.EconomicsArcade.Presentation.WebApplication.Factories;
using EntityFX.EconomicsArcade.Presentation.WebApplication.Providers;
using EntityFX.EconomicsArcade.Utils.ClientProxy.Manager;
using Microsoft.Practices.Unity;

namespace EntityFX.EconomicsArcade.Presentation.WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGameDataProvider _gameDataProvider;

        /// <summary>Called before the action method is invoked.</summary>
        /// <param name="filterContext">Information about the current request and action.</param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _gameDataProvider.Initialize(User.Identity.Name);
        }

        public HomeController(IGameDataProvider gameDataProvider)
        {
            _gameDataProvider = gameDataProvider;

        }

        public ActionResult Index()
        {
            return View("Index");
        }
    }
}