using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace LogoMockWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculationController : ControllerBase
    {
        // GET: api/calculation/sum?x=5&y=10
        [HttpGet("sum")]
        public IActionResult Sum(int x, int y)
        {
            int result = x + y;
            Log.Information("Sum calculated: {X} + {Y} = {Result}", x, y, result);
            return Ok(result);
        }
    }
}
