using CommunityToolkit.Mvvm.Messaging;
using Mediator;
using SmartGrowHub.Application.LogOut;
using SmartGrowHub.Application.Services;
using SmartGrowHub.Domain.Extensions;
using SmartGrowHub.Maui.Application.Commands;
using SmartGrowHub.Maui.Application.Interfaces;
using SmartGrowHub.Maui.Application.Messages;

namespace SmartGrowHub.Maui.MediatorHandlers.Commands;

internal sealed class LogOutHandler(
    IUserSessionProvider sessionProvider,
    INavigationService navigationService,
    IAuthService authService,
    IMessenger messenger)
    : ICommandHandler<LogOutCommand, Unit>
{
    public ValueTask<Unit> Handle(LogOutCommand command, CancellationToken cancellationToken) =>
        LogOut(cancellationToken).RunUnsafeAsync();

    private Eff<Unit> LogOut(CancellationToken cancellationToken) =>
        from sessionId in sessionProvider.GetUserSessionId(cancellationToken)
        let request = new LogOutRequest(sessionId)
        from response in authService.LogOut(request, cancellationToken)
        from _1 in sessionProvider.RemoveSession()
        from _2 in navigationService.SetLogInAsRoot()
        from _3 in liftEff(() => messenger.Send(LoggedOutMessage.Default))
        select unit;
}
