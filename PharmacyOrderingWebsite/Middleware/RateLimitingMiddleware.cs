using System.Collections.Concurrent;
using System.Text.Json;

namespace PharmacyOrderingWebsite.Middleware
{
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;

        // Thread-safe store
        private static readonly ConcurrentDictionary<string, List<DateTime>> _requests = new();

        private const int LIMIT = 100; // max requests per window
        private const int WINDOW_SECONDS = 60; // time window

        public RateLimitingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

            // ✅ Skip localhost (IMPORTANT for development)
            if (ip == "127.0.0.1" || ip == "::1")
            {
                await _next(context);
                return;
            }

            var now = DateTime.UtcNow;

            var requests = _requests.GetOrAdd(ip, _ => new List<DateTime>());

            lock (requests)
            {
                // ✅ Remove old requests
                requests.RemoveAll(t => (now - t).TotalSeconds > WINDOW_SECONDS);

                if (requests.Count >= LIMIT)
                {
                    context.Response.StatusCode = 429;
                    context.Response.ContentType = "application/json";

                    var response = JsonSerializer.Serialize(new
                    {
                        message = "Too many requests. Please try again later."
                    });

                    context.Response.WriteAsync(response).Wait(); // ✅ no async error

                    return;
                }

                // ✅ Add current request
                requests.Add(now);
            }

            await _next(context);
        }
    }
}