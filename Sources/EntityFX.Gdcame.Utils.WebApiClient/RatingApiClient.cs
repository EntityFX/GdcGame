using System.Threading.Tasks;
using EntityFX.Gdcame.Application.Contract.Controller;
using EntityFX.Gdcame.Application.Contract.Model;
using EntityFX.Gdcame.Common.Application.Model;
using EntityFX.Gdcame.Utils.WebApiClient.Auth;
using RestSharp.Authenticators;

namespace EntityFX.Gdcame.Utils.WebApiClient
{
    public class RatingApiClient : ApiClientBase, IRatingController
    {
        public RatingApiClient(IAuthContext<IAuthenticator> authContext, int? timeout = null) : base(authContext, timeout)
        {
        }

        public async Task<RatingStatisticsModel[]> GetRaiting(int top = 500)
        {
            var response = await ExecuteRequestAsync<RatingStatisticsModel[]>("/api/rating");
            return response.Data;
        }
    }
}