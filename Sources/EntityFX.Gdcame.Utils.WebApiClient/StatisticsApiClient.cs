namespace EntityFX.Gdcame.Utils.WebApiClient
{
    using EntityFX.Gdcame.Application.Contract.Controller.Common;
    using EntityFX.Gdcame.Common.Application.Model;
    using EntityFX.Gdcame.Infrastructure.Api;

    public class StatisticsApiClient<TStatisticsInfoModel> : ApiClientBase, IStatisticsInfo<TStatisticsInfoModel>
        where TStatisticsInfoModel : ServerStatisticsInfoModel
    {
        public StatisticsApiClient(IApiClient authContext)
            : base(authContext)
        {
        }

        public TStatisticsInfoModel GetStatistics()
        {
            return this.ExecuteRequestAsync<TStatisticsInfoModel>("/api/admin/statistics").Result.Data;
        }
    }
}