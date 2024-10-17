using Mediator;
using SmartGrowHub.Domain.Extensions;
using SmartGrowHub.Maui.Application.Interfaces;
using SmartGrowHub.Maui.Application.Messages.Commands;
using SmartGrowHub.Maui.Application.Messages.Notifications;

namespace SmartGrowHub.Maui.MediatorHandlers.Notifications;

internal sealed class NoAuthorizeHandler(
    IMediator mediator,
    IDialogService dialogService)
    : INotificationHandler<NoAuthorizeNotification>
{
    public async ValueTask Handle(NoAuthorizeNotification notification, CancellationToken cancellationToken) =>
        await HandleEff(cancellationToken).RunUnsafeAsync();

    private Eff<Unit> HandleEff(CancellationToken cancellationToken) =>
        from _1 in dialogService.PopAsync()
        from _2 in DisplayAuthorizationError(cancellationToken)
        from _3 in mediator.Send(LogOutCommand.Default, cancellationToken).ToEff()
        select unit;

    private IO<Unit> DisplayAuthorizationError(CancellationToken cancellationToken) =>
        dialogService.DisplayAlertAsync(
            Resources.AuthorizationError,
            Resources.LogInToYourAccountAgain,
            Resources.Ok, cancellationToken);
}
