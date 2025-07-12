using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace Hertmans.Shared.Auth.Services;

internal static class ResilienceHandler
{
    public static IHttpClientBuilder WithDefaultResilience<T>(this IHttpClientBuilder httpClientBuilder)
    {
        httpClientBuilder.AddResilienceHandler($"{typeof(T).Name}-pipeline", (builder, context) =>
        {
            var logFactory = context.ServiceProvider.GetRequiredService<ILoggerFactory>();
            var logger = logFactory.CreateLogger(nameof(ResilienceHandler));
            builder.AddRetry(GetRetryOptions(logger));
        });

        return httpClientBuilder;
    }
    
    private static RetryStrategyOptions<HttpResponseMessage> GetRetryOptions(ILogger logger) =>
        new()
        {
            ShouldHandle = args => HandleTransientHttpError(args.Outcome),
            MaxRetryAttempts = 3,
            BackoffType = DelayBackoffType.Exponential,
            Delay = TimeSpan.FromSeconds(2),
            OnRetry = args =>
            {
                var attempt = args.AttemptNumber;
                var exception = args.Outcome.Exception;
                var requestUri = args.Outcome.Result?.RequestMessage?.RequestUri;
                
                logger.LogWarning(exception, "Exception occurred while calling {Url} (attempt {AttemptNumber})",
                    requestUri, attempt);
                return ValueTask.CompletedTask;
            }
        };

    private static ValueTask<bool> HandleTransientHttpError(Outcome<HttpResponseMessage> outcome) => outcome switch
    {
        { Exception: HttpRequestException } => PredicateResult.True(),
        { Result.StatusCode: HttpStatusCode.RequestTimeout } => PredicateResult.True(),
        { Result.StatusCode: >= HttpStatusCode.InternalServerError } => PredicateResult.True(),
        _ => PredicateResult.False()
    };
}