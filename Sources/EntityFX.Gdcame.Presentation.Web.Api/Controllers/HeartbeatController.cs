using Microsoft.AspNetCore.Mvc;

namespace EntityFX.Gdcame.Application.Api.MainServer.Controllers
{
    [Route("api/[controller]")]
    public class HeartbeatController : Controller
    {

        // Get api/Heartbeat
        public string Get()
        {
            return "value";
        }
    }
}