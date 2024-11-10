using CommunityToolkit.Mvvm.Messaging;
using SmartGrowHub.Application.LogOut;
using SmartGrowHub.Application.Services;
using SmartGrowHub.Maui.Application.Interfaces;
using SmartGrowHub.Maui.Application.Messages;
using SmartGrowHub.Maui.Services.Extensions;

namespace SmartGrowHub.Maui.Services;

internal sealed class LogOutService(
    IUserSessionService sessionService,
    IAuthService authService,
    INavigationService navigationService,
    IMessenger messenger)
    : ILogOutService
{
    public Eff<Unit> LogOut(CancellationToken cancellationToken) =>
        from sessionId in sessionService.GetUserSessionId(cancellationToken)
        let request = new LogOutRequest(sessionId)
        from response in authService.LogOut(request, cancellationToken)
        from _1 in sessionService.RemoveSession()
        from _2 in navigationService.SetLogInAsRoot(cancellationToken: cancellationToken)
        from _3 in messenger.SendIO(LoggedOutMessage.Default)
        select unit;
}
