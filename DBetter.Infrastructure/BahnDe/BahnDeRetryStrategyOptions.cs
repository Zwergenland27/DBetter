using System.Net;
using Polly.Retry;

namespace DBetter.Infrastructure.BahnDe;

public class BahnDeRetryStrategyOptions
{
    private static readonly HashSet<HttpStatusCode> TransientStatusCodes =
    [
        HttpStatusCode.RequestTimeout,        // 408
        HttpStatusCode.TooManyRequests,       // 429
        HttpStatusCode.BadGateway,            // 502
        HttpStatusCode.ServiceUnavailable,    // 503
        HttpStatusCode.GatewayTimeout         // 504
    ];
    
    public static ValueTask<bool> IsTransient(RetryPredicateArguments<HttpResponseMessage> args)
    {
        if (args.Outcome.Exception is HttpRequestException) return ValueTask.FromResult(true);
        if (args.Outcome.Result is not { } response) return ValueTask.FromResult(false);
        
        return ValueTask.FromResult(TransientStatusCodes.Contains(response.StatusCode));
    }
}