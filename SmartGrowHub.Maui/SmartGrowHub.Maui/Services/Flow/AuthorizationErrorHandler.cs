using SmartGrowHub.Maui.Resources.Localization;
using SmartGrowHub.Maui.Services.App;

namespace SmartGrowHub.Maui.Services.Flow;

public interface IAuthorizationErrorHandler
{
    IO<Unit> Handle(CancellationToken cancellationToken);
}

public sealed class AuthorizationErrorHandler(
    IDialogService dialogService,
    ILogoutService logoutService)
    : IAuthorizationErrorHandler
{
    public IO<Unit> Handle(CancellationToken cancellationToken) =>
        from _2 in DisplayAuthorizationError()
        from _3 in logoutService.LogOut(cancellationToken)
        select unit;

    private IO<Unit> DisplayAuthorizationError() =>
        dialogService.DisplayAlertAsync(
            AppResources.AuthorizationError,
            AppResources.LogInToYourAccountAgain,
            AppResources.Ok);
}
