using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace WebAPIWithJWTAndIdentity.MiddleWare;

public class CustomLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public CustomLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            Console.WriteLine("=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=");
            var method = context.Request.Method;
            var path = context.Request.Path;
            var userId = context.User.Identity?.IsAuthenticated == true
                ? context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                : "Anonymous";
            var clientIp = context.Connection.RemoteIpAddress?.ToString();
            var time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Console.WriteLine("==================================================================================");
            Console.WriteLine($"[{time}] Method: {method}, Path: {path}, UserId: {userId}, IP: {clientIp}");
            Console.WriteLine("==================================================================================");
            var stopwatch = Stopwatch.StartNew();
            await _next(context);
            stopwatch.Stop();
            Console.WriteLine($"Request duration: {stopwatch.ElapsedMilliseconds} ms");
            Console.WriteLine("=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=");
        }
        catch (Exception e)
        {
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine("Error: " + e.Message);
            Console.WriteLine("--------------------------------------------------------------------------------");
        }
    }
}