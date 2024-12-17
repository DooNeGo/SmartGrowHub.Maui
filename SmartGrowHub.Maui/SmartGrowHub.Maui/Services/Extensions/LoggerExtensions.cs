using Microsoft.Extensions.Logging;

namespace SmartGrowHub.Maui.Services.Extensions;

public static partial class LoggerExtensions
{
    public static Eff<Unit> LogErrorEff(this ILogger logger, Error error) =>
        liftEff(() => LogError(logger, error.Message));
    
    public static Eff<Unit> LogErrorEff<T>(this ILogger<T> logger, Error error) =>
        liftEff(() => LogError(logger, error.Message));

    [LoggerMessage(LogLevel.Error, Message = "{message}")]
    static partial void LogError(ILogger logger, string message);
}