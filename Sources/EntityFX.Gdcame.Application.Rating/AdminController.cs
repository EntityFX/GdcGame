using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EntityFX.Gdcame.Application.Api.Controller.RatingServer
{
    using System.Linq;

    using EntityFX.Gdcame.Application.Contract.Controller.Common;
    using EntityFX.Gdcame.Common.Application.Model;
    using EntityFX.Gdcame.Contract.Common.Statistics;
    using EntityFX.Gdcame.Infrastructure.Common;
    using EntityFX.Gdcame.Manager.Contract.Common.AdminManager;
    using EntityFX.Gdcame.Manager.Contract.Common.WorkerManager;

    [Authorize(Roles = "Admin")]
    [Route("api/admin")]
    public class AdminController : Microsoft.AspNetCore.Mvc.Controller, IStatisticsInfo<ServerStatisticsInfoModel>
    {
        private readonly IAdminManager<StatisticsInfo> _adminManager;
        private readonly IWorkerManager _workerManager;
        private readonly IMapperFactory _mapperFactory;

        public AdminController(IAdminManager<StatisticsInfo> adminManager, IWorkerManager workerManager,
            IMapperFactory mapperFactory)
        {
            _adminManager = adminManager;
            _workerManager = workerManager;
            _mapperFactory = mapperFactory;
        }

        [HttpGet]
        [Route("statistics")]
        public ServerStatisticsInfoModel GetStatistics()
        {
            var statistics = _mapperFactory.Build<StatisticsInfo, ServerStatisticsInfoModel>().Map(_adminManager.GetStatisticsInfo());
            statistics.ActiveWorkers = this._workerManager.GetWorkersStatus().Select(
                    _ => new WorkerStatusModel()
                    {
                        Name = _.Name,
                        IsRunning = _.IsRunning,
                        Ticks = _.Ticks,
                        PerfomanceCounters = _.PerfomanceCounters
                    }).ToArray();
            return statistics;
        }
    }
}