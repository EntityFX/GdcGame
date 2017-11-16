using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.Common.Application.Model;
using EntityFX.Gdcame.Application.Contract.Controller;
using EntityFX.Gdcame.Application.Contract.Model;
using System;
using EntityFX.Gdcame.Application.Contract.Controller.MainServer;
using EntityFX.Gdcame.Application.Contract.Model.MainServer;
using EntityFX.Gdcame.Infrastructure.Api;
using EntityFX.Gdcame.Infrastructure.Api.Auth;

namespace EntityFX.Gdcame.Utils.WebApiClient
{
    public class GameApiClient : ApiClientBase, IGameApiController
    {


        public async Task<ManualStepResultModel> PerformManualStepAsync(int? id = null)
        {
            var response = await ExecuteRequestAsync<ManualStepResultModel>("/api/game/perform-step", ApiRequestMethod.POST);
            return response.Data;
        }

        public async Task<CashModel> FightAgainstInflationAsync()
        {
            var response = await ExecuteRequestAsync<CashModel>("/api/game/fight-inflation", ApiRequestMethod.POST);
            return response.Data;
        }

        public async Task<CashModel> ActivateDelayedCounterAsync(int counterId)
        {
            var response = await ExecuteRequestAsync<CashModel>("/api/game/activate-delayed-counter", ApiRequestMethod.POST);

            return response.Data;
        }

        public async Task<GameDataModel> GetGameDataAsync()
        {
            var response = await ExecuteRequestAsync<GameDataModel>("/api/game/game-data");
            return response.Data;
        }

        public async Task<CashModel> GetCountersAsync()
        {
            var response = await ExecuteRequestAsync<CashModel>("/api/game/game-counters");
            return response.Data;
        }

        public async Task<BuyItemModel> BuyFundDriverAsync(int id)
        {
            var response = await ExecuteRequestAsync<BuyItemModel>("/api/game/buy-item", ApiRequestMethod.POST
                , new List<ApiParameter>() { new ApiParameter() { Type = ApiParameterType.Body, Value = id, Name = "", ContentType = "application/json" }});
            return response.Data;
        }

        public GameApiClient(IApiClient apiclient) : base(apiclient)
        {
        }
    }
}