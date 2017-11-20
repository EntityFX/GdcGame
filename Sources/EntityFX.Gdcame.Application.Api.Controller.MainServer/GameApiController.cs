using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using EntityFX.Gdcame.Application.Contract.Controller;
using EntityFX.Gdcame.Application.Contract.Controller.MainServer;
using EntityFX.Gdcame.Application.Contract.Model;
using EntityFX.Gdcame.Application.Contract.Model.MainServer;
using EntityFX.Gdcame.Application.Providers.MainServer;
using EntityFX.Gdcame.Common.Application.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EntityFX.Gdcame.Application.Api.Controller.MainServer
{
    [Authorize]
    [Route("api/game")]
    public class GameApiController : Microsoft.AspNetCore.Mvc.Controller, IGameApiController
    {
        private readonly IGameDataProvider _gameDataProvider;

        public GameApiController(IGameDataProvider gameDataProvider)
        {
            _gameDataProvider = gameDataProvider;

            var claimsIdentity = User.Identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                _gameDataProvider.InitializeGameContext(Guid.Parse(claimsIdentity.FindFirst("gameSession").Value));
            }
        }

        [HttpPost]
        [Route("perform-step")]
        public Task<ManualStepResultModel> PerformManualStepAsync([FromBody] int? verificationNumber)
        {
            return Task.Factory.StartNew(() =>_gameDataProvider.PerformManualStep(verificationNumber));
        }

        [HttpPost]
        [Route("fight-inflation")]
        public async Task<CashModel> FightAgainstInflationAsync()
        {
            await Task.Factory.StartNew(() => _gameDataProvider.FightAgainstInflation());
            return _gameDataProvider.GetCounters();
        }

        [HttpPost]
        [Route("activate-delayed-counter")]
        public async Task<CashModel> ActivateDelayedCounterAsync([FromBody] int counterId)
        {
            await Task.Factory.StartNew(() => _gameDataProvider.ActivateDelayedCounter(counterId));
            return _gameDataProvider.GetCounters();
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
    }
}