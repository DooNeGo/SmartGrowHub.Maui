using SmartGrowHub.Maui.Resources.Localization;
using SmartGrowHub.Maui.Services.App;

namespace SmartGrowHub.Maui.Services.Flow;

public interface IAuthorizationErrorHandler
{
    Eff<Unit> Handle(CancellationToken cancellationToken);
}

public sealed class AuthorizationErrorHandler(
    IDialogService dialogService,
    ILogoutService logoutService)
    : IAuthorizationErrorHandler
{
    public Eff<Unit> Handle(CancellationToken cancellationToken) =>
        from _1 in dialogService.PopAllAsync()
        from _2 in DisplayAuthorizationError()
        from _3 in logoutService.LogOut(cancellationToken)
        select unit;

    private IO<Unit> DisplayAuthorizationError() =>
        dialogService.DisplayAlertAsync(
            AppResources.AuthorizationError,
            AppResources.LogInToYourAccountAgain,
            AppResources.Ok);
}
