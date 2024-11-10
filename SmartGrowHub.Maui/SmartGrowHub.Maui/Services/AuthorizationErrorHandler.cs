using SmartGrowHub.Maui.Application.Interfaces;

namespace SmartGrowHub.Maui.Services;

internal sealed class AuthorizationErrorHandler(
    IDialogService dialogService,
    ILogOutService logOutService)
    : IAuthorizationErrorHandler
{
    public Eff<Unit> Handle(CancellationToken cancellationToken) =>
        from _1 in dialogService.PopAllAsync()
        from _2 in DisplayAuthorizationError()
        from _3 in logOutService.LogOut(cancellationToken)
        select unit;

    private IO<Unit> DisplayAuthorizationError() =>
        dialogService.DisplayAlertAsync(
            Resources.AuthorizationError,
            Resources.LogInToYourAccountAgain,
            Resources.Ok);
}
