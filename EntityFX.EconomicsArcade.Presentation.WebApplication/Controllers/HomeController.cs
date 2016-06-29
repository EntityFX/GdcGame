using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EntityFX.EconomicsArcade.Presentation.Models;
using EntityFX.EconomicsArcade.Utils.ClientProxy.Manager;

namespace EntityFX.EconomicsArcade.Presentation.WebApplication.Controllers
{
    public class HomeController : Controller
    {
      
        public ActionResult Index()
        {
            
            if (Session["SessionGuid"] == null)
            {
                var simpleUserManagerClient = new SimpleUserManagerClient(ConfigurationManager.AppSettings["ManagerEndpointAddress_UserManager"]);
                if (!simpleUserManagerClient.Exists(User.Identity.Name))
                {
                    simpleUserManagerClient.Create(User.Identity.Name);
                }

                var sessionManagerClient = new SessionManagerClient(ConfigurationManager.AppSettings["ManagerEndpointAddress_SessionManager"]);
                Session["SessionGuid"] = sessionManagerClient.AddSession(User.Identity.Name);
            }
            Guid sessionGuid = (Guid)Session["SessionGuid"];

            var gameClient = new GameManagerClient(ConfigurationManager.AppSettings["ManagerEndpointAddress_GameManager"], sessionGuid);
            var gameData = gameClient.GetGameData();
            var gameModel = (new GameDataModelMapper()).Map(gameData);
            return View("Index", gameModel);
        }

        public ActionResult BuyFundDriver(int id)
        {
            Guid sessionGuid = (Guid)Session["SessionGuid"];
            var gameClient = new GameManagerClient(ConfigurationManager.AppSettings["ManagerEndpointAddress_GameManager"], sessionGuid);
            gameClient.BuyFundDriver(id);
            return RedirectToAction("Index");
        }

        public ActionResult PerformManualStep()
        {
            Guid sessionGuid = (Guid)Session["SessionGuid"];
            var gameClient = new GameManagerClient(ConfigurationManager.AppSettings["ManagerEndpointAddress_GameManager"], sessionGuid);
            gameClient.PerformManualStep();
            return RedirectToAction("Index");
        }

        public ActionResult FightAgainstInflation()
        {
            Guid sessionGuid = (Guid)Session["SessionGuid"];
            var gameClient = new GameManagerClient(ConfigurationManager.AppSettings["ManagerEndpointAddress_GameManager"], sessionGuid);
            gameClient.FightAgainstInflation();
            return RedirectToAction("Index");
        }
    }
}