using Microsoft.Extensions.Logging;

namespace TcHaxx.Snappy.TcADS;

internal sealed class LoggerFactory : ILoggerFactory
{
    private readonly ILogger? _logger;

    private LoggerFactory(ILogger logger)
    {
        _logger = logger;
    }

    public static ILoggerFactory? From(ILogger? logger)
    {
        return logger is null ? null : new LoggerFactory(logger);
    }

    public void AddProvider(ILoggerProvider provider)
    {
    }

    public ILogger CreateLogger(string categoryName)
    {
        return _logger ?? LoggerFactoryExtensions.CreateLogger(this, GetType());
    }

    public void Dispose()
    {
    }

}
