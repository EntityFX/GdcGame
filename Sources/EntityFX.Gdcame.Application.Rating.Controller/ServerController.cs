using System.Threading.Tasks;
using EntityFX.Gdcame.Application.Contract.Controller.Common;
using EntityFX.Gdcame.Common.Application.Model;
using EntityFX.Gdcame.Manager.Contract.Common.ServerManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EntityFX.Gdcame.Application.Api.Common
{
    [Route("api/server-info")]
    public class ServerController : Controller, IServerController
    {
        private readonly IServerManager _serverManager;

        public ServerController(IServerManager serverManager)
        {
            _serverManager = serverManager;
        }

        [HttpGet("")]
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