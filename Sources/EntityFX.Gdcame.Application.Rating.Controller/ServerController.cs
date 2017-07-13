using System.Threading.Tasks;
using System.Web.Http;
using EntityFX.Gdcame.Application.Contract.Controller.Common;
using EntityFX.Gdcame.Common.Application.Model;
using EntityFX.Gdcame.Manager.Contract.Common.ServerManager;

namespace EntityFX.Gdcame.Application.Api.Common
{
    [RoutePrefix("api/server-info")]
    public class ServerController : ApiController, IServerController
    {
        private readonly IServerManager _serverManager;

        public ServerController(IServerManager serverManager)
        {
            _serverManager = serverManager;
        }

        [HttpGet]
        [Route("")]
        public async Task<ServerInfoModel> GetServersInfo()
        {
            return await Task.Run(() => new ServerInfoModel() { ServerList = _serverManager.GetServers() });
        }

        [HttpGet]
        [Route("echo")]
        public string Echo(string text)
        {
            return string.Format("OK: {0}", text);
        }

        [HttpGet]
        [Authorize(Roles = "GenericUser")]
        [Route("echo-auth")]
        public string EchoAuth(string text)
        {
            return string.Format("OK: {0}", text);
        }
    }
}