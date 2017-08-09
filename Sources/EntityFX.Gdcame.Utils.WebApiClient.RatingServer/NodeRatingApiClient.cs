namespace EntityFX.Gdcame.Utils.WebApiClient.RatingServer
{
    using System.Threading.Tasks;
    using EntityFX.Gdcame.Contract.Common.UserRating;
    using EntityFX.Gdcame.Infrastructure.Api;
    using EntityFX.Gdcame.Infrastructure.Api.Auth;

    using RestSharp.Authenticators;

    public class NodeRatingApiClient : ApiClientBase, IRatingDataRetrieve
    {
        public NodeRatingApiClient(IAuthContext<IAuthenticator> authContext, int? timeout = null) : base(authContext, timeout)
        {
        }

        public async Task<TopRatingStatistics> GetRaiting(int top = 500)
        {
            var response = await ExecuteRequestAsync<TopRatingStatistics>("/api/rating");
            return response.Data;
        }
    }
}