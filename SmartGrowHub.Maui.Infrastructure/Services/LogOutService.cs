using CommunityToolkit.Mvvm.Messaging;
using SmartGrowHub.Application.LogOut;
using SmartGrowHub.Application.Services;
using SmartGrowHub.Maui.Application.Interfaces;
using SmartGrowHub.Maui.Application.Messages;

namespace SmartGrowHub.Maui.Infrastructure.Services;

internal sealed class LogOutService(
    IUserSessionProvider sessionProvider,
    IAuthService authService,
    INavigationService navigationService,
    IMessenger messenger)
    : ILogOutService
{
    public Eff<Unit> LogOut(CancellationToken cancellationToken) =>
        from sessionId in sessionProvider.GetUserSessionId(cancellationToken)
        let request = new LogOutRequest(sessionId)
        from response in authService.LogOut(request, cancellationToken)
        from _1 in sessionProvider.RemoveSession()
        from _2 in navigationService.SetLogInAsRoot(cancellationToken: cancellationToken)
        from _3 in liftEff(() => messenger.Send(LoggedOutMessage.Default))
        select unit;
}
