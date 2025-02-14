using Polly;
using Polly.Retry;
using Polly.CircuitBreaker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Core.Domain.Settings;

public class RetryPolicies
{
    private readonly ILogger _logger;
    private readonly ResilienceSettings _settings;

    public RetryPolicies(ILogger logger, IOptions<ResilienceSettings> settings)
    {
        _logger = logger;
        _settings = settings.Value;
    }

    public AsyncRetryPolicy<T> CreateDefaultRetryPolicy<T>()
    {
        return Policy<T>
            .Handle<Exception>()
            .WaitAndRetryAsync(
                _settings.Retry.MaxRetryAttempts,
                retryAttempt => TimeSpan.FromMilliseconds(
                    _settings.Retry.BaseDelayMilliseconds * Math.Pow(2, retryAttempt - 1)),
                (exception, timeSpan, retryCount, context) =>
                {
                    _logger.LogWarning(
                        exception,
                        "Tentativa {RetryCount} falhou após {TimeSpan:g}. Erro: {Error}",
                        retryCount,
                        timeSpan,
                        exception.Message);
                }
            );
    }

    public AsyncCircuitBreakerPolicy<T> CreateCircuitBreakerPolicy<T>()
    {
        return Policy<T>
            .Handle<Exception>()
            .CircuitBreakerAsync(
                _settings.CircuitBreaker.ExceptionsAllowedBeforeBreaking,
                TimeSpan.FromSeconds(_settings.CircuitBreaker.DurationOfBreakInSeconds),
                (exception, duration) =>
                {
                    _logger.LogError(
                        exception,
                        "Circuit breaker aberto por {DurationSeconds} segundos após {ExceptionsCount} exceções",
                        _settings.CircuitBreaker.DurationOfBreakInSeconds,
                        _settings.CircuitBreaker.ExceptionsAllowedBeforeBreaking);
                },
                () =>
                {
                    _logger.LogInformation("Circuit breaker resetado");
                }
            );
    }
} 