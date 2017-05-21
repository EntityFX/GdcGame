using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using EntityFX.Gdcame.Manager.Contract.AdminManager;
using EntityFX.Gdcame.Application.Contract.Controller;
using EntityFX.Gdcame.Application.Contract.Model;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Manager.Contract.Workermanager;
using Newtonsoft.Json;

namespace EntityFX.Gdcame.Application.WebApi.Controller
{
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
        public ServerStatisticsInfoModel GetStatistics()
        {
            var statistics = _mapperFactory.Build<StatisticsInfo, ServerStatisticsInfoModel>().Map(_adminManager.GetStatisticsInfo());
            statistics.ActiveWorkers =
                _workerManager.GetWorkersStatus().Where(_ => _.IsRunning).Select(_ => _.Name).ToArray();
            return statistics;
        }

        [HttpGet]
        [Route("update_nodes_list")]
        public string UpdateNodesList([FromUri] string[] newServersList)
        {
            Console.WriteLine("---AdminController going to update Nodes List---");
            System.IO.StreamWriter file = new System.IO.StreamWriter("servers2.json");
            var jsonList = JsonConvert.SerializeObject(newServersList);
            file.WriteLine(jsonList);
            file.Close();
            return "Nodes List updated: " + jsonList;
        }

        [HttpDelete]
        [Route("sessions/guid")]
        public async void CloseSessionByGuid([FromBody] Guid guid)
        {
            await Task.Run(() =>_adminManager.CloseSessionByGuid(guid));
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