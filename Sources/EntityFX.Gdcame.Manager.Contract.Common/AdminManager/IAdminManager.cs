﻿
namespace EntityFX.Gdcame.Manager.Contract.Common.AdminManager
{
    using EntityFX.Gdcame.Contract.Common.Statistics;
    using EntityFX.Gdcame.Contract.MainServer.Statistics;

    public interface IAdminManager<out TStatistics> : IServerStatistics<TStatistics>
        where TStatistics: StatisticsInfo
    {
        
    }
}