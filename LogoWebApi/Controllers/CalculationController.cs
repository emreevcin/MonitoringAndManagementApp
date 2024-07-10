using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace LogoMockWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculationController : ControllerBase
    {
        [HttpGet("sum")]
        public IActionResult Sum(int x, int y)
        {
            int result = x + y;
            Log.Information("Sum calculated: {X} + {Y} = {Result}", x, y, result);
            return Ok(result);
        }
    }
}
