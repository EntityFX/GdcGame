using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using EntityFX.Gdcame.Manager.Contract.AdminManager;
using EntityFX.Gdcame.Application.Contract.Controller;
using EntityFX.Gdcame.Application.Contract.Model;

namespace EntityFX.Gdcame.Application.WebApi.Controller
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/admin")]
    public class AdminController : ApiController, IAdminController
    {
        private readonly IAdminManager _adminManager;

        public AdminController(IAdminManager adminManager)
        {
            _adminManager = adminManager;
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

        [HttpPost]
        [Route("reload-game")]
        public async void ReloadGame([FromBody] string username)
        {
            await Task.Run(() => _adminManager.ReloadGame(username));
        }
    }
}