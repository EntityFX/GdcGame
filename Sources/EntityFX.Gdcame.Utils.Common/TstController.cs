using Microsoft.AspNetCore.Mvc;

namespace EntityFX.Gdcame.Utils.Common
{
    [Route("api/[controller]")]
    public class TstController : Controller
    {
        public TstController()
        {
            
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return Ok("1");
        }
    }
}