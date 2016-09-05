using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using EntityFX.Gdcame.Common.Presentation.Model;
using EntityFX.Gdcame.Presentation.Contract.Controller;
using EntityFX.Gdcame.Presentation.Contract.Model;
using EntityFX.Gdcame.Utils.WebApiClient.Auth;
using RestSharp.Portable;

namespace EntityFX.Gdcame.Utils.WebApiClient
{
    public class GameApiClient : IGameApiController
    {
        private readonly IAuthContext<IAuthenticator> _authContext;

        public GameApiClient(IAuthContext<IAuthenticator> authContext)
        {
            _authContext = authContext;
        }

        public async Task<ManualStepResultModel> PerformManualStepAsync(int? id = null)
        {
            var response = await ExecuteGetRequestAsync<ManualStepResultModel>("/api/game/perform-step", Method.POST);
            return response.Data;
        }

        public async Task<bool> FightAgainstInflationAsync()
        {
            var response = await ExecuteGetRequestAsync<bool>("/api/game/fight-inflation", Method.POST);
            return response.Data;
        }

        public async Task<bool> ActivateDelayedCounterAsync(int counterId)
        {
            var response = await ExecuteGetRequestAsync<bool>("/api/game/activate-delayed-counter", Method.POST);
            return response.Data;
        }

        public async Task<GameDataModel> GetGameDataAsync()
        {
            var response = await ExecuteGetRequestAsync<GameDataModel>("/api/game/game-data");
            return response.Data;
        }

        public async Task<CashModel> GetCountersAsync()
        {
            var response = await ExecuteGetRequestAsync<CashModel>("/api/game/game-counters");
            return response.Data;
        }

        public async Task<BuyDriverModel> BuyFundDriverAsync(int id)
        {
            var response = await ExecuteGetRequestAsync<BuyDriverModel>(string.Format("/api/game/buy-item", id), Method.POST
                , new List<Parameter>() { new Parameter() {Type = ParameterType.RequestBody, Value = 1} });
            return response.Data;
        }

        private async Task<IRestResponse<TModel>> ExecuteGetRequestAsync<TModel>(string requestUriPath, Method method = Method.GET, IEnumerable<Parameter> parameters = null)
        {
            var clientFactory = new GameClientFactory(_authContext.BaseUri);
            var request = clientFactory.CreateRequest(requestUriPath);
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    request.Parameters.Add(parameter);
                }
            }

            request.Method = method;
            var client = clientFactory.CreateClient();
            client.Authenticator = _authContext.Context;
            try
            {
                return await client.Execute<TModel>(request);
            }
            catch (HttpRequestException exception)
            {
                ExceptionHandlerHelper.HandleHttpRequestException(exception);
            }
            return null;
        }
    }
}