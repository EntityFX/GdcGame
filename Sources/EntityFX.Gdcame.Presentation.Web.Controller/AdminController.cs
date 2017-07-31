using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using EntityFX.Gdcame.Application.Contract.Controller;
using EntityFX.Gdcame.Application.Contract.Controller.MainServer;
using EntityFX.Gdcame.Application.Contract.Model;
using EntityFX.Gdcame.Application.Contract.Model.MainServer;
using EntityFX.Gdcame.Common.Application.Model;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Manager.Contract.Common.WorkerManager;
using EntityFX.Gdcame.Manager.Contract.MainServer.AdminManager;
using Newtonsoft.Json;
using SessionInfoModel = EntityFX.Gdcame.Application.Contract.Model.MainServer.SessionInfoModel;

namespace EntityFX.Gdcame.Application.Api.Controller.MainServer
{
    using EntityFX.Gdcame.Contract.Common.Statistics;

    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/admin")]
    public class AdminController : ApiController, IAdminController
    {
        private readonly IAdminManager _adminManager;
        private readonly IWorkerManager _workerManager;
        private readonly IMapperFactory _mapperFactory;

        public AdminController(IAdminManager adminManager, IWorkerManager workerManager,
            IMapperFactory mapperFactory)
        {
            _adminManager = adminManager;
            _workerManager = workerManager;
            _mapperFactory = mapperFactory;
        }

        [HttpGet]
        [Route("sessions")]
        public async Task<UserSessionsModel[]> GetActiveSessions()
        {
            return await Task.Run(() => _adminManager.GetActiveSessions().Select(_ => new UserSessionsModel()
            {
                Login = _.UserName,
                Sessions =
                        _.UserSessions.Select(
                            s =>
                                new SessionInfoModel()
                                {
                                    SessionIdentifier = s.SessionIdentifier,
                                    LastActivity = s.LastActivity
                                }).ToArray()
            }).ToArray());
        }

        [HttpGet]
        [Route("statistics")]
        public MainServerStatisticsInfoModel GetStatistics()
        {
            var statistics = _mapperFactory.Build<MainServerStatisticsInfo, MainServerStatisticsInfoModel>().Map(_adminManager.GetStatisticsInfo());
            statistics.ActiveWorkers =
                _workerManager.GetWorkersStatus().Where(_ => _.IsRunning).Select(

                _ => string.Format("Name: {0}, IsRunning: {1}, Ticks: {2}", _.Name, _.IsRunning, _.Ticks)

            ).ToArray();
            return statistics;
        }

        [HttpPost]
        [Route("servers")]
        public  void UpdateServersList(string[] serversList)
        {
            this._workerManager.Start("NodeDataTransferWorker", serversList);
        }


        [HttpDelete]
        [Route("sessions/guid")]
        public async void CloseSessionByGuid([FromBody] Guid guid)
        {
            await Task.Run(() => _adminManager.CloseSessionByGuid(guid));
        }

        [HttpDelete]
        [Route("sessions/user")]
        public async void CloseAllUserSessions(string username)
        {
            await Task.Run(() => _adminManager.CloseAllUserSessions(username));
        }

        [HttpDelete]
        [Route("sessions/all")]
        public async void CloseAllSessions()
        {
            await Task.Run(() => _adminManager.CloseAllSessions());
        }

        [HttpDelete]
        [Route("sessions/exclude")]
        public async void CloseAllSessionsExcludeThis([FromBody] Guid guid)
        {
            await Task.Run(() => _adminManager.CloseAllSessionsExcludeThis(guid));
        }

        [HttpPost]
        [Route("wipe-user")]
        public async void WipeUser([FromBody] string username)
        {
            await Task.Run(() => _adminManager.WipeUser(username));
        }

        [HttpDelete]
        [Route("reload-game")]
        public async void ReloadGame([FromBody] string username)
        {
            await Task.Run(() => _adminManager.ReloadGame(username));
        }

        [HttpDelete]
        [Route("games/user")]
        public void StopGame(string username)
        {
            _adminManager.StopGame(username);
        }

        [HttpDelete]
        [Route("games/all")]
        public void StopAllGames()
        {
            _adminManager.StopAllGames();
        }
    }
}