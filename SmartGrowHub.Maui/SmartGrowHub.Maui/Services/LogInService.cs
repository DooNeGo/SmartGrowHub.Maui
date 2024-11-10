using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Logging;
using SmartGrowHub.Application.LogIn;
using SmartGrowHub.Application.Services;
using SmartGrowHub.Maui.Application.Interfaces;
using SmartGrowHub.Maui.Application.Messages;
using SmartGrowHub.Maui.Services.Extensions;

namespace SmartGrowHub.Maui.Services;

internal sealed partial class LogInService(
    IAuthService authService,
    IUserSessionService sessionService,
    INavigationService navigationService,
    IMessenger messenger,
    ILogger<LogInService> logger)
    : ILogInService
{
    public Eff<Unit> LogIn(LogInRequest request, bool remember, CancellationToken cancellationToken) =>
        LogInInternal(request, remember, cancellationToken)
            .IfFailEff(LogErrorEff);

    private Eff<Unit> LogInInternal(LogInRequest request, bool remember, CancellationToken cancellationToken) =>
        from response in authService.LogIn(request, cancellationToken)
        from _1 in remember
            ? sessionService.SaveAndSetSession(response.UserSession, cancellationToken)
            : sessionService.SetSession(response.UserSession)
        from _2 in navigationService.SetMainPageAsRoot()
        from _3 in messenger.SendIO(LoggedInMessage.Default)
        select unit;

    private Eff<Unit> LogErrorEff(Error error) =>
        liftEff(() => LogError(logger, error.Message));

    [LoggerMessage(LogLevel.Error, Message = "{message}")]
    static partial void LogError(ILogger logger, string message);
}
