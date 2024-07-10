using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Serilog;

namespace LogoMockWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PingController : ControllerBase
    {
        /// <summary>
        /// Checks if the server is responsive.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/ping
        ///
        /// </remarks>
        /// <returns>A string message indicating ping response.</returns>
        [HttpGet("ping")]
        [SwaggerOperation(Summary = "Ping endpoint", Description = "Check if the server is responsive.")]
        [SwaggerResponse(200, "Returns a string message indicating ping response.", typeof(string))]
        public IActionResult Ping()
        {
            string response = "Ping received.";
            Log.Information("Ping response: {Response}", response);
            return Ok(response);
        }
    }
}
