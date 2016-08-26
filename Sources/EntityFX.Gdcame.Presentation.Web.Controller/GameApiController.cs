using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using EntityFX.Gdcame.Common.Presentation.Model;
using EntityFX.Gdcame.Presentation.Web.Model;
using EntityFX.Gdcame.Presentation.Web.Providers.Providers;

namespace EntityFX.Gdcame.Presentation.Web.Controller
{
    [Authorize]
    [RoutePrefix("api/game")]
    public class GameApiController : ApiController, IGameApiController
    {
        private readonly IGameDataProvider _gameDataProvider;

        public GameApiController(IGameDataProvider gameDataProvider)
        {
            _gameDataProvider = gameDataProvider;
        }

        [HttpPost]
        [Route("perform-step")]
        public ManualStepResultModel PerformManualStep([FromBody] int? verificationNumber)
        {
            var res = _gameDataProvider.PerformManualStep(verificationNumber);
            return res;
        }

        [HttpPost]
        [Route("fight-inflation")]
        public bool FightAgainstInflation()
        {
            _gameDataProvider.FightAgainstInflation();
            return true;
        }

        [HttpPost]
        [Route("activate-delayed-counter/{counterId:int}")]
        public bool ActivateDelayedCounter([FromBody] int counterId)
        {
            _gameDataProvider.ActivateDelayedCounter(counterId);
            return true;
        }

        [HttpGet]
        [Route("game-data")]
        public async Task<GameDataModel> GetGameData()
        {
            var gameData = await Task.Factory.StartNew(() =>
                _gameDataProvider.GetGameData());
            return gameData;
        }

        [HttpGet]
        [Route("game-counters")]
        public CashModel GetCounters()
        {
            return _gameDataProvider.GetCounters();
        }

        [HttpPost]
        [Route("buy-item/{id:int}")]
        public BuyDriverModel BuyFundDriver([FromBody] int id)
        {
            return _gameDataProvider.BuyFundDriver(id);
        }

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);

            if (User == null)
            {
                return;
            }

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
    }
}