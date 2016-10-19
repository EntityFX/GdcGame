using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.Common.Application.Model;
using EntityFX.Gdcame.Application.Contract.Controller;
using EntityFX.Gdcame.Application.Contract.Model;
using EntityFX.Gdcame.Utils.WebApiClient.Auth;
using Newtonsoft.Json.Linq;
using RestSharp.Portable;
using System;

namespace EntityFX.Gdcame.Utils.WebApiClient
{
    public class GameApiClient : ApiClientBase, IGameApiController
    {


        public async Task<ManualStepResultModel> PerformManualStepAsync(int? id = null)
        {
            var response = await ExecuteRequestAsync<ManualStepResultModel>("/api/game/perform-step", Method.POST);
            return response.Data;
        }

        public async Task<bool> FightAgainstInflationAsync()
        {
            var response = await ExecuteRequestAsync<bool>("/api/game/fight-inflation", Method.POST);
            return response.Data;
        }

        public async Task<bool> ActivateDelayedCounterAsync(int counterId)
        {
            var response = await ExecuteRequestAsync<bool>("/api/game/activate-delayed-counter", Method.POST);

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
            var response = await ExecuteRequestAsync<BuyItemModel>("/api/game/buy-item", Method.POST
                , new List<Parameter>() { new Parameter() {Type = ParameterType.RequestBody, Value = id} });
            return response.Data;
        }

        public GameApiClient(IAuthContext<IAuthenticator> authContext, TimeSpan? timeout = null) : base(authContext, timeout)
        {
        }
    }
}