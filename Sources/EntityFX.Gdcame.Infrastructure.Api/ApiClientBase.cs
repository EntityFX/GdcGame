using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using EntityFX.Gdcame.Infrastructure.Api.Auth;

namespace EntityFX.Gdcame.Infrastructure.Api
{
    using EntityFX.Gdcame.Infrastructure.Api.Exceptions;

    public abstract class ApiClientBase : IApiClient
    {
        private readonly IApiClient apiAdapter;


        protected ApiClientBase(IApiClient apiAdapter)
        {
            this.apiAdapter = apiAdapter;
        }

       

        public Task<IApiResponse<TModel>> ExecuteRequestAsync<TModel>(string requestUriPath, ApiRequestMethod method = ApiRequestMethod.GET, IEnumerable<ApiParameter> parameters = null)
        {
            return apiAdapter.ExecuteRequestAsync<TModel>(requestUriPath, method, parameters);
        }

        public Task<IApiResponse<TResponse>> ExecuteRequestAsync<TRequest, TResponse>(string requestUriPath, ApiRequestMethod method = ApiRequestMethod.GET, TRequest data = default(TRequest))
            where TRequest : class
        {
            return apiAdapter.ExecuteRequestAsync<TRequest, TResponse>(requestUriPath, method, data);
        }
    }

}
