using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Serilog;

namespace LogoMockWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculationController : ControllerBase
    {
        /// <summary>
        /// Calculates the sum of two integers.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/calculation/sum?x=5&y=10
        ///     {
        ///        "x": 5,
        ///        "y": 10
        ///     }
        ///
        /// </remarks>
        /// <param name="x">First integer.</param>
        /// <param name="y">Second integer.</param>
        /// <returns>The sum of x and y.</returns>
        [HttpGet("sum")]
        [SwaggerOperation(Summary = "Calculate sum of two integers", Description = "Calculates the sum of two integers.")]
        [SwaggerResponse(200, "Returns the sum of the two integers.", typeof(int))]
        public IActionResult Sum(int x, int y)
        {
            int result = x + y;
            Log.Information("Sum calculated: {X} + {Y} = {Result}", x, y, result);
            return Ok(result);
        }
    }
}
