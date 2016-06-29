using System.Configuration;
using System.Web.Mvc;
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

        public HomeController()
        {
            _gameDataProvider = new GameDataProvider(
                new GameClientFactory(UnityConfig.GetConfiguredContainer()),
                //new SimpleUserManagerClient(ConfigurationManager.AppSettings["ManagerEndpointAddress_UserManager"]),
                UnityConfig.GetConfiguredContainer().Resolve<ISimpleUserManager>(),
                UnityConfig.GetConfiguredContainer().Resolve<SessionManagerClient>(),
                //new SessionManagerClient(ConfigurationManager.AppSettings["ManagerEndpointAddress_SessionManager"]),
                UnityConfig.GetConfiguredContainer().Resolve<IMapper<GameData, GameDataModel>>()
                //new GameDataModelMapper()
                );

        }

        public ActionResult Index()
        {
            _gameDataProvider.Initialize(User.Identity.Name);
            var gameModel = _gameDataProvider.GetGameData();
            //if (Session["SessionGuid"] == null)
            //{
            //    var simpleUserManagerClient = new SimpleUserManagerClient(ConfigurationManager.AppSettings["ManagerEndpointAddress_UserManager"]);
            //    if (!simpleUserManagerClient.Exists(User.Identity.Name))
            //    {
            //        simpleUserManagerClient.Create(User.Identity.Name);
            //    }

            //    var sessionManagerClient = new SessionManagerClient(ConfigurationManager.AppSettings["ManagerEndpointAddress_SessionManager"]);
            //    Session["SessionGuid"] = sessionManagerClient.AddSession(User.Identity.Name);
            //}
            //Guid sessionGuid = (Guid)Session["SessionGuid"];

            //var gameClient = new GameManagerClient(ConfigurationManager.AppSettings["ManagerEndpointAddress_GameManager"], sessionGuid);
            //var gameData = gameClient.GetGameData();
            //var gameModel = (new GameDataModelMapper()).Map(gameData);
            //return View("Index", gameModel);
            return View("IndexAngular", gameModel);
        }

        public ActionResult BuyFundDriver(int id)
        {
            _gameDataProvider.Initialize(User.Identity.Name);
            _gameDataProvider.BuyFundDriver(id);
            //Guid sessionGuid = (Guid)Session["SessionGuid"];
            //var gameClient = new GameManagerClient(ConfigurationManager.AppSettings["ManagerEndpointAddress_GameManager"], sessionGuid);
            //gameClient.BuyFundDriver(id);
            return RedirectToAction("Index");
        }

        public ActionResult PerformManualStep()
        {
            _gameDataProvider.Initialize(User.Identity.Name);
            _gameDataProvider.PerformManualStep();
            //Guid sessionGuid = (Guid)Session["SessionGuid"];
            //var gameClient = new GameManagerClient(ConfigurationManager.AppSettings["ManagerEndpointAddress_GameManager"], sessionGuid);
            //gameClient.PerformManualStep();
            return RedirectToAction("Index");
        }

        public ActionResult FightAgainstInflation()
        {
            _gameDataProvider.Initialize(User.Identity.Name);
            _gameDataProvider.FightAgainstInflation();
            //Guid sessionGuid = (Guid)Session["SessionGuid"];
            //var gameClient = new GameManagerClient(ConfigurationManager.AppSettings["ManagerEndpointAddress_GameManager"], sessionGuid);
            //gameClient.FightAgainstInflation();
            return RedirectToAction("Index");
        }
    }
}