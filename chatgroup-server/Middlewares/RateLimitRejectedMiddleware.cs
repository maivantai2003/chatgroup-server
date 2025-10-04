using System.Text.Json;

namespace chatgroup_server.Middlewares
{
    public class RateLimitRejectedMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RateLimitRejectedMiddleware> _logger;
        public RateLimitRejectedMiddleware(RequestDelegate next,ILogger<RateLimitRejectedMiddleware> logger) {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Response.StatusCode == StatusCodes.Status429TooManyRequests &&
                !context.Response.HasStarted)
            {
                context.Response.ContentType = "application/json";

                var userId = context.User?.FindFirst("sub")?.Value
                             ?? context.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                var ip = context.Connection.RemoteIpAddress?.ToString();

                // log
                _logger.LogWarning("[RateLimit] User: {UserId} IP: {IP}", userId ?? "guest", ip);
                Console.WriteLine($"[RateLimit] User: {userId ?? "guest"} IP: {ip}");

                var result = new
                {
                    error = "Bạn đã gửi quá nhiều request, vui lòng thử lại sau."
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(result));
                return; // dừng tại đây
            }

            await _next(context); // cho request tiếp tục nếu không bị 429
        }
    }
}
