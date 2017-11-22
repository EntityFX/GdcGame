using Microsoft.AspNetCore.Mvc;

namespace EntityFX.Gdcame.Utils.ConsoleHostApp.AllInOne.MainServerNetCore
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        public TestController()
        {
            
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return Ok("1");
        }
    }
}