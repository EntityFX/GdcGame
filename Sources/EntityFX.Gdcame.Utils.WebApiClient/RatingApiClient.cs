using System.Threading.Tasks;
using EntityFX.Gdcame.Application.Api.Common;
using EntityFX.Gdcame.Application.Contract.Controller;
using EntityFX.Gdcame.Application.Contract.Controller.Common;
using EntityFX.Gdcame.Application.Contract.Controller.MainServer;
using EntityFX.Gdcame.Application.Contract.Model;
using EntityFX.Gdcame.Common.Application.Model;
using EntityFX.Gdcame.Infrastructure.Api;
using EntityFX.Gdcame.Infrastructure.Api.Auth;

namespace EntityFX.Gdcame.Utils.WebApiClient
{
    using EntityFX.Gdcame.Contract.Common.UserRating;

    public class RatingApiClient : ApiClientBase, IRatingController
    {
        public RatingApiClient(IApiClient apiAdapter) : base(apiAdapter)
        {
        }

        public async Task<TopRatingStatistics> GetRaiting(int top = 500)
        {
            var response = await ExecuteRequestAsync<TopRatingStatistics>("/api/rating");
            return response.Data;
        }
    }
}