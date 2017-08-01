namespace EntityFX.Gdcame.Application.Contract.Controller.Common
{
    using EntityFX.Gdcame.Common.Application.Model;

    public interface IStatisticsInfo<out TStatisticsInfoModel> where TStatisticsInfoModel : ServerStatisticsInfoModel
    {
        TStatisticsInfoModel GetStatistics();
    }
}