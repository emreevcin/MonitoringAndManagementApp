using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace LogoMockWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PingController : ControllerBase
    {
        // GET: api/ping
        [HttpGet("ping")]
        public IActionResult Ping()
        {
            string response = "Ping received.";
            Log.Information("Ping response: {Response}", response);
            return Ok(response);
        }
    }
}
