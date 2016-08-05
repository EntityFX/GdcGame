using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Web.Http;
using System.Web.Http.Controllers;
using EntityFX.Gdcame.Common.Presentation.Model;
using EntityFX.Gdcame.Presentation.Web.Controller;
using EntityFX.Gdcame.Presentation.Web.Model;
using EntityFX.Gdcame.Presentation.Web.Providers.Providers;

namespace EntityFX.Gdcame.Presentation.Web.Controllers
{
    [Authorize]
    public class GameApiController : ApiController, IGameApiController
    {
        private readonly IGameDataProvider _gameDataProvider;

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            var windowsIdentity = User.Identity as WindowsIdentity;
            if (windowsIdentity != null)
            {
                _gameDataProvider.InitializeSession(User.Identity.Name);
                return;
            }
            var claimsIdentity = User.Identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                _gameDataProvider.InitializeGameContext(Guid.Parse(claimsIdentity.FindFirst("gameSession").Value));
            }

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
