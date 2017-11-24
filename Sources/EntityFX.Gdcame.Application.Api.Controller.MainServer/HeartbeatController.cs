using Microsoft.AspNetCore.Mvc;

namespace EntityFX.Gdcame.Application.Api.Controller.MainServer
{
    [Route("api/[controller]")]
    public class HeartbeatController : Microsoft.AspNetCore.Mvc.Controller
    {

        // Get api/Heartbeat
        public string Get()
        {
            return "value";
        }
    }
}