using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace LogoWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PingController : ControllerBase
    {
        [HttpGet("ping")]
        public IActionResult Ping()
        {
            string response = "Ping received.";
            Log.Information("Ping response: {Response}", response);
            return Ok(response);
        }
    }
}

