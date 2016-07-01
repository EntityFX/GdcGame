using System;
using System.Web.Http;
using System.Web.Http.Controllers;
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
    public class GameApiController : ApiController, IGameApiController
    {
        private readonly IGameDataProvider _gameDataProvider;

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            _gameDataProvider.Initialize(User.Identity.Name);
        }

        public GameApiController(IGameDataProvider gameDataProvider)
        {
            _gameDataProvider = gameDataProvider;
            //_gameDataProvider = new GameDataProvider(
            //    new GameClientFactory(UnityConfig.GetConfiguredContainer()),
            //    UnityConfig.GetConfiguredContainer().Resolve<ISimpleUserManager>(),
            //    UnityConfig.GetConfiguredContainer().Resolve<SessionManagerClient>(),
            //    UnityConfig.GetConfiguredContainer().Resolve<IMapper<GameData, GameDataModel>>()
            //    );
        }

        [System.Web.Http.HttpPost]
        public VerificationNumberModel PerformManualStep()
        {
            var res = _gameDataProvider.PerformManualStep(null);
            return res;
        }

        [System.Web.Http.HttpPost]
        public bool FightAgainstInflation()
        {
            _gameDataProvider.FightAgainstInflation();
            return true;
        }

        [System.Web.Http.HttpPost]
        public bool ActivateDelayedCounter()
        {
            _gameDataProvider.FightAgainstInflation();
            return true;
        }

        [System.Web.Http.HttpGet]
        public GameDataModel GetGameData()
        {
            return _gameDataProvider.GetGameData();
        }

        [System.Web.Http.HttpGet]
        public FundsCounterModel GetCounters()
        {
            return _gameDataProvider.GetCounters();
        }

        [System.Web.Http.HttpPost]
        public bool BuyFundDriver([FromBody]int id)
        {
            _gameDataProvider.BuyFundDriver(id);
            return true;
        }
    }

}
