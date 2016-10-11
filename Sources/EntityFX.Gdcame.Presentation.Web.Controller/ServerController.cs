using EntityFX.Gdcame.Application.Contract.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using EntityFX.Gdcame.Application.Contract.Model;

namespace EntityFX.Gdcame.Application.WebApi.Controller
{
    [RoutePrefix("api/server-info")]
    public class ServerController : ApiController, IServerController
    {
        [HttpGet]
        [Route("")]
        public async Task<ServerInfoModel> GetServersInfo()
        {
            return await Task.Run(() => new ServerInfoModel() { CountServers = 4 });
        }
    }
}
