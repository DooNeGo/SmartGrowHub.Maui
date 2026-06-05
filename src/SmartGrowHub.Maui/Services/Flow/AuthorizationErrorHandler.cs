using SmartGrowHub.Maui.Resources.Localization;
using SmartGrowHub.Maui.Services.App;

namespace SmartGrowHub.Maui.Services.Flow;

public interface IAuthorizationErrorHandler
{
    IO<Unit> Handle();
}

public sealed class AuthorizationErrorHandler(
    IDialogService dialogService,
    ILogoutService logoutService)
    : IAuthorizationErrorHandler
{
    public IO<Unit> Handle() =>
        from _1 in DisplayAuthorizationError()
        from _2 in logoutService.LogOut()
        select _2;

    private IO<Unit> DisplayAuthorizationError() =>
        dialogService.DisplayAlert(
            AppResources.AuthorizationError,
            AppResources.LogInToYourAccountAgain,
            AppResources.Ok);
}
