public class ResilienceSettings
{
    public CircuitBreakerSettings CircuitBreaker { get; set; } = new();
    public RetrySettings Retry { get; set; } = new();
}

public class CircuitBreakerSettings
{
    public int ExceptionsAllowedBeforeBreaking { get; set; } = 2;
    public int DurationOfBreakInSeconds { get; set; } = 30;
}

public class RetrySettings
{
    public int MaxRetryAttempts { get; set; } = 3;
    public int BaseDelayMilliseconds { get; set; } = 1000;
} 