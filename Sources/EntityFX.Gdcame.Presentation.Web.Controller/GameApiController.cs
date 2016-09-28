using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using EntityFX.Gdcame.Common.Presentation.Model;
using EntityFX.Gdcame.Presentation.Contract.Model;
using EntityFX.Gdcame.Presentation.Web.Providers.Providers;
using EntityFX.Gdcame.Presentation.Contract.Controller;

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
        public Task<ManualStepResultModel> PerformManualStepAsync([FromBody] int? verificationNumber)
        {
            return Task.Factory.StartNew(() =>_gameDataProvider.PerformManualStep(verificationNumber));
        }

        [HttpPost]
        [Route("fight-inflation")]
        public async Task<bool> FightAgainstInflationAsync()
        {
            await Task.Factory.StartNew(() => _gameDataProvider.FightAgainstInflation());
            return true;
        }

        [HttpPost]
        [Route("activate-delayed-counter")]
        public async Task<bool> ActivateDelayedCounterAsync([FromBody] int counterId)
        {
            await Task.Factory.StartNew(() => _gameDataProvider.ActivateDelayedCounter(counterId));
            return true;
        }

        [HttpGet]
        [Route("game-data")]
        public async Task<GameDataModel> GetGameDataAsync()
        {
            return await Task.Factory.StartNew(() =>
                _gameDataProvider.GetGameData());
        }

        [HttpGet]
        [Route("game-counters")]
        public async Task<CashModel> GetCountersAsync()
        {
            return await Task.Factory.StartNew(() => _gameDataProvider.GetCounters());
        }

        [HttpPost]
        [Route("buy-item")]
        public async Task<BuyItemModel> BuyFundDriverAsync([FromBody] int id)
        {
            return await Task.Factory.StartNew(() => _gameDataProvider.BuyFundDriver(id));
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