using Microsoft.Extensions.Logging;

namespace SmartGrowHub.Maui.Services.Extensions;

public static partial class LoggerExtensions
{
    public static IO<Unit> LogErrorIO<T>(this ILogger<T> logger, Error error) =>
        IO.lift(() => LogError(logger, error.Message));

    [LoggerMessage(LogLevel.Error, Message = "{message}")]
    static partial void LogError(ILogger logger, string message);
}