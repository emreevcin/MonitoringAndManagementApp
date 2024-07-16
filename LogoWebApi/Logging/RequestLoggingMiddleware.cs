using Serilog;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace LogoWebApi.Logging
{
    [ExcludeFromCodeCoverage]
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            Log.Information("Incoming request: {RequestMethod} {RequestPath}", context.Request.Method, context.Request.Path);

            var stopwatch = Stopwatch.StartNew();
            var originalBodyStream = context.Response.Body;

            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                await _next(context);

                var statusCode = context.Response.StatusCode;

                var response = await FormatResponseAsync(context.Response);

                if (statusCode >= 400)
                {
                    
                    Log.Error("Response: {StatusCode} ({ElapsedTime}ms) {ResponseBody}", statusCode, stopwatch.ElapsedMilliseconds, response);
                }
                else
                {
                    Log.Information("Outgoing response: {StatusCode} {ElapsedTimeMs}ms {ResponseBody}", context.Response.StatusCode, stopwatch.ElapsedMilliseconds, response);

                }
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        private async Task<string> FormatResponseAsync(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            string responseBody = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            return responseBody;
        }
    }
}
