namespace RealWorld.WebAPI.Logging;

public interface ILoggerAdapter<TType>
{
    void LogInformation(string? message, params object?[] args);
    void LogError(Exception? exception, string? message, params object?[] args);
}

public sealed class LoggerAdapter<TType>(ILogger<TType> logger) : ILoggerAdapter<TType>
{
    public void LogInformation(string? message, params object?[] args)
    {
        logger.LogInformation(message, args);
    }

    public void LogError(Exception? exception, string? message, params object?[] args)
    {
        logger.LogError(exception, message, args);
    }
}
