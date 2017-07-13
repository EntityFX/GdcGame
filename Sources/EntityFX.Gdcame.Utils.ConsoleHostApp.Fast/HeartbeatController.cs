using System.Web.Http;

namespace EntityFX.Gdcame.Utils.ConsoleHostApp.Fast.MainServer
{
    [RoutePrefix("api/server-info")]
    public class HeartbeatController : ApiController
    {
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