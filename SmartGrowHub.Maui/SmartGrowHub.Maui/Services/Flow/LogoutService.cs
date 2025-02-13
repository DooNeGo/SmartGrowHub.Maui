using SmartGrowHub.Maui.Services.Api;
using SmartGrowHub.Maui.Services.App;
using SmartGrowHub.Maui.Services.DelegatingHandlers;
using SmartGrowHub.Maui.Services.Extensions;

namespace SmartGrowHub.Maui.Services.Flow;

public interface ILogoutService
{
    Eff<Unit> LogOut(CancellationToken cancellationToken);
}

public sealed class LogoutService(
    INavigationService navigationService,
    IAuthService authService,
    ISecureStorage secureStorage)
    : ILogoutService
{
    public Eff<Unit> LogOut(CancellationToken cancellationToken) =>
        from _1 in navigationService.NavigateAsync($"//{Routes.StartPage}")
        from _2 in secureStorage.RemoveAllValues()
        from _3 in LogOutFromServer(cancellationToken)
        select _3;

    private Eff<Unit> LogOutFromServer(CancellationToken cancellationToken) => (
        from refreshToken in secureStorage.GetRefreshToken().MapT(io => io.ToEff())
        from _ in authService.LogOut(refreshToken, cancellationToken)
        select _
    ).IfNone(unit).As();
}