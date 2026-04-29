using System.Threading.RateLimiting;

namespace Apphia_Website_API.Extension;

public static class RateLimitingExtension {
    public const string PublicStrict = "public-strict";

    public static IServiceCollection AddAppRateLimiting(this IServiceCollection services) {
        services.AddRateLimiter(options => {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            // Baseline limiter for everything: authenticated callers get a per-user
            // bucket; anonymous traffic falls back to per-IP so one noisy client
            // can't drown out the rest.
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(http => {
                var userId = http.User.FindFirst("UserId")?.Value;
                if (!string.IsNullOrEmpty(userId)) {
                    return RateLimitPartition.GetFixedWindowLimiter("u:" + userId, _ => new FixedWindowRateLimiterOptions {
                        PermitLimit = 120,
                        Window = TimeSpan.FromMinutes(1),
                        QueueLimit = 0
                    });
                }

                var ip = http.Connection.RemoteIpAddress?.ToString() ?? "unknown";
                return RateLimitPartition.GetFixedWindowLimiter("ip:" + ip, _ => new FixedWindowRateLimiterOptions {
                    PermitLimit = 60,
                    Window = TimeSpan.FromMinutes(1),
                    QueueLimit = 0
                });
            });

            // Tight per-IP cap for unauthenticated endpoints that are easy targets
            // for spam or brute force (login, contact form, password reset).
            options.AddPolicy(PublicStrict, http => {
                var ip = http.Connection.RemoteIpAddress?.ToString() ?? "unknown";
                return RateLimitPartition.GetSlidingWindowLimiter(ip, _ => new SlidingWindowRateLimiterOptions {
                    PermitLimit = 5,
                    Window = TimeSpan.FromMinutes(1),
                    SegmentsPerWindow = 6,
                    QueueLimit = 0
                });
            });
        });

        return services;
    }
}
