namespace EntityFX.Gdcame.Utils.WebApiClient
{
    using EntityFX.Gdcame.Application.Contract.Controller.Common;
    using EntityFX.Gdcame.Common.Application.Model;
    using EntityFX.Gdcame.Infrastructure.Api;
    using EntityFX.Gdcame.Infrastructure.Api.Auth;

    using RestSharp.Authenticators;

    public class StatisticsApiClient<TStatisticsInfoModel> : ApiClientBase, IStatisticsInfo<TStatisticsInfoModel>
        where TStatisticsInfoModel : ServerStatisticsInfoModel
    {
        public StatisticsApiClient(IAuthContext<IAuthenticator> authContext, int? timeout = null)
            : base(authContext, timeout)
        {
        }

        public TStatisticsInfoModel GetStatistics()
        {
            return this.ExecuteRequestAsync<TStatisticsInfoModel>("/api/admin/statistics").Result.Data;
        }
    }
}