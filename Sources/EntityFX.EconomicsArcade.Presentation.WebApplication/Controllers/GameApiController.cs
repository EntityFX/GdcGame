using System;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using EntityFX.EconomicsArcade.Model.Common.Model;
using EntityFX.EconomicsArcade.Presentation.Models;
using EntityFX.EconomicsArcade.Presentation.WebApplication.Providers;

namespace EntityFX.EconomicsArcade.Presentation.WebApplication.Controllers
{
    public class GameApiController : ApiController, IGameApiController
    {
        private readonly IGameDataProvider _gameDataProvider;

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            _gameDataProvider.InitializeSession(User.Identity.Name);
        }

        public GameApiController(IGameDataProvider gameDataProvider)
        {
            _gameDataProvider = gameDataProvider;
        }

        [System.Web.Http.HttpPost]
        public ManualStepResultModel PerformManualStep([FromBody]int? verificationNumber)
        {
            var res = _gameDataProvider.PerformManualStep(verificationNumber);
            return res;
        }

        [System.Web.Http.HttpPost]
        public bool FightAgainstInflation()
        {
            _gameDataProvider.FightAgainstInflation();
            return true;
        }

        [System.Web.Http.HttpPost]
        public bool ActivateDelayedCounter([FromBody]int counterId)
        {
            _gameDataProvider.ActivateDelayedCounter(counterId);
            //_gameDataProvider.FightAgainstInflation();
            return true;
        }

        [System.Web.Http.HttpGet]
        public GameDataModel GetGameData()
        {
            var gameData = _gameDataProvider.GetGameData();
            return gameData;
        }

        [System.Web.Http.HttpGet]
        public FundsCounterModel GetCounters()
        {
            return _gameDataProvider.GetCounters();
        }

        [System.Web.Http.HttpPost]
        public BuyDriverModel BuyFundDriver([FromBody]int id)
        {
            return _gameDataProvider.BuyFundDriver(id);
        }

    }

}
