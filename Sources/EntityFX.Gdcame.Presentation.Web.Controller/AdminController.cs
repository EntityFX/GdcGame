using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using EntityFX.Gdcame.Manager.Contract.AdminManager;
using EntityFX.Gdcame.Application.Contract.Controller;
using EntityFX.Gdcame.Application.Contract.Model;
using EntityFX.Gdcame.DataAccess.Contract.User;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Manager.Contract.Workermanager;
using EntityFX.Gdcame.Utils.Shared;
using Newtonsoft.Json;
using EntityFX.Gdcame.DataAccess.Service;
using System.Configuration;

namespace EntityFX.Gdcame.Application.WebApi.Controller
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/admin")]
    public class AdminController : ApiController, IAdminController
    {
        private readonly IAdminManager _adminManager;
        private readonly IWorkerManager _workerManager;
        private readonly IMapperFactory _mapperFactory;
        private readonly IUserDataAccessService _userDataAccessService;

        private readonly IHashHelper hashHelper;

        public AdminController(IAdminManager adminManager, IWorkerManager workerManager,
            IMapperFactory mapperFactory, IUserDataAccessService userDataAccessService)
        {
            _adminManager = adminManager;
            _workerManager = workerManager;
            _mapperFactory = mapperFactory;
            _userDataAccessService = userDataAccessService;
            hashHelper = new HashHelper();
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
            string[] oldServersList = ServerApiHelper.GetServers();
            string jsonServerList = UpdateNodesListInFile(newServersList);
            UpdateNodeData(oldServersList, newServersList);
            return "Nodes List updated: " + jsonServerList;
        }


        private void UpdateNodeData(string[] oldServersList, string[] newServersList)
        {
            User[] users = _userDataAccessService.FindAll();
            foreach (var user in users)
            {
                int oldNumber = hashHelper.GetServerNumberByRendezvousHashing(user.Login, oldServersList);
                int newNumber = hashHelper.GetServerNumberByRendezvousHashing(user.Login, newServersList);
                if (oldNumber!=newNumber)
                {
                    //add user to new server
                    int userId = new DatabasesProvider().addUser(newServersList[newNumber], user);

                    if (userId != 0)
                    {
                          //delete user form currrent server
                          _userDataAccessService.Delete(user.Id);
                    }
                    else
                    {
                        Console.WriteLine(String.Format("User {0} was not added.",
                            user));
                    }
                }
            }
        }

        

        private string UpdateNodesListInFile(string[] newServersList)
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter("servers.json");
            var jsonList = JsonConvert.SerializeObject(newServersList);
            file.WriteLine(jsonList);
            file.Close();
            return jsonList;
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