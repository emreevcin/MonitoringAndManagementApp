using Serilog;
using System.Diagnostics;

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

            if (statusCode >= 400)
            {
                var response = await FormatResponseAsync(context.Response);
                Log.Error("Response: {StatusCode} ({ElapsedTime}ms) {ResponseBody}", statusCode, stopwatch.ElapsedMilliseconds, response);
            } else
            {
                Log.Information("Outgoing response: {StatusCode} {ElapsedTimeMs}ms", context.Response.StatusCode, stopwatch.ElapsedMilliseconds);

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
