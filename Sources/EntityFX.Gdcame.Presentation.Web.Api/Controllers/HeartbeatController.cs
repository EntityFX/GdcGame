using System.Web.Http;
using System.Web.Http.Controllers;

namespace EntityFX.Gdcame.Application.Api.MainServer.Controllers
{
    public class HeartbeatController : ApiController
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
        }

        // Get api/Heartbeat
        public string Get()
        {
            return "value";
        }
    }
}