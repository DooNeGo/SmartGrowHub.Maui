using Mediator;
using SmartGrowHub.Application.LogIn;
using SmartGrowHub.Application.Services;
using SmartGrowHub.Maui.Application.Interfaces;
using SmartGrowHub.Maui.Application.Messages.Commands;

namespace SmartGrowHub.Maui.MediatorHandlers.Commands;

internal sealed class LogInHandler(
    IAuthService authService,
    IUserSessionProvider sessionProvider,
    INavigationService navigationService)
    : ICommandHandler<LogInCommand, Unit>
{
    public ValueTask<Unit> Handle(LogInCommand command, CancellationToken cancellationToken) =>
        LogIn(command, cancellationToken).RunUnsafeAsync();

    private Eff<Unit> LogIn(LogInCommand command, CancellationToken cancellationToken) =>
        from request in Pure(new LogInRequest(command.UserName, command.Password))
        from response in authService.LogIn(request, cancellationToken)
        from _1 in command.Remember
            ? sessionProvider.SaveAndSetSession(response.UserSession, cancellationToken)
            : sessionProvider.SetSession(response.UserSession)
        from _2 in navigationService.SetMainPageAsRoot()
        select unit;
}
