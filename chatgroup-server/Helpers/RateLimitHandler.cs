using Microsoft.AspNetCore.RateLimiting;

namespace chatgroup_server.Helpers
{
    public static class RateLimitHandler
    {
        public static async Task HandleRejectedAsync(OnRejectedContext context, CancellationToken token)
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            context.HttpContext.Response.ContentType = "application/json";

            await context.HttpContext.Response.WriteAsync(
                "{\"error\": \"Bạn đã gửi quá nhiều request, vui lòng thử lại sau.\"}",
                token);

            var userId = context.HttpContext.User?.FindFirst("sub")?.Value
                         ?? context.HttpContext.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var ip = context.HttpContext.Connection.RemoteIpAddress?.ToString();

            Console.WriteLine($"[RateLimit] User: {userId ?? "guest"} IP: {ip}");
        }
    }
}
