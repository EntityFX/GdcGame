using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.Gdcame.Infrastructure.Api
{
    public interface IApiClient
    {
        Task<IApiResponse<TModel>> ExecuteRequestAsync<TModel>(string requestUriPath, ApiRequestMethod method = ApiRequestMethod.GET, IEnumerable<ApiParameter> parameters = null);

        Task<IApiResponse<TResponse>> ExecuteRequestAsync<TRequest, TResponse>(string requestUriPath, ApiRequestMethod method = ApiRequestMethod.GET, TRequest data = default(TRequest))
            where TRequest : class;
    }
}
