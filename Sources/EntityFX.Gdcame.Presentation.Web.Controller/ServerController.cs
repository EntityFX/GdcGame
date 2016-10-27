using EntityFX.Gdcame.Application.Contract.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using EntityFX.Gdcame.Application.Contract.Model;
using EntityFX.Gdcame.Manager.Contract.ServerManager;

namespace EntityFX.Gdcame.Application.WebApi.Controller
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
