using Polly.Extensions.Http;
using Polly.Retry;
using Polly.Timeout;
using Polly;
using System.Net;

namespace Challenge.Api.Configurations.Extensions;

public class PollyExtensions
{
    private static readonly List<HttpStatusCode> retriableStatusCodes = new()
{
        HttpStatusCode.RequestTimeout,       // 408
        HttpStatusCode.TooManyRequests,      // 429
        HttpStatusCode.InternalServerError,  // 500
        HttpStatusCode.BadGateway,           // 502
        HttpStatusCode.ServiceUnavailable,   // 503
        HttpStatusCode.GatewayTimeout        // 504
};

    public static IAsyncPolicy<HttpResponseMessage> TimeoutPolicy()
    {
        return Policy.TimeoutAsync<HttpResponseMessage>(128);
    }

    public static AsyncRetryPolicy<HttpResponseMessage> WaitAndRetry()
    {
        var retry = HttpPolicyExtensions
                .HandleTransientHttpError()
                .Or<TimeoutRejectedException>()
                .OrResult(msg => retriableStatusCodes.Contains(msg.StatusCode))
                .WaitAndRetryAsync(new[]
                {
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(2),
                TimeSpan.FromSeconds(5),
                }, (outcome, timespan, retryCount, context) =>
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"Trying for the {retryCount} time. StatusCode:{outcome?.Result?.StatusCode}");
                    Console.ForegroundColor = ConsoleColor.White;
                });

        return retry;
    }
}
