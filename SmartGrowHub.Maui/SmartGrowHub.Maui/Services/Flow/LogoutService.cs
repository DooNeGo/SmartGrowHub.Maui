using SmartGrowHub.Maui.Services.Api;
using SmartGrowHub.Maui.Services.App;
using SmartGrowHub.Maui.Services.Extensions;

namespace SmartGrowHub.Maui.Services.Flow;

public interface ILogoutService
{
    IO<Unit> LogOut(CancellationToken cancellationToken);
}

public sealed class LogoutService(
    INavigationService navigationService,
    IAuthService authService,
    ISecureStorage secureStorage)
    : ILogoutService
{
    public IO<Unit> LogOut(CancellationToken cancellationToken) =>
        from _1 in navigationService.NavigateAsync($"//{Routes.StartPage}")
        from _2 in secureStorage.RemoveAllValues()
        from _3 in LogOutFromServer(cancellationToken)
        select _3;

    private IO<Unit> LogOutFromServer(CancellationToken cancellationToken) => (
        from refreshToken in secureStorage.GetRefreshToken()
        from _ in authService.LogOut(refreshToken, cancellationToken)
        select _
    ).IfNone(Unit.Default).As();
}