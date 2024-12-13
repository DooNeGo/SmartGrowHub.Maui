using SmartGrowHub.Maui.Services.Api;
using SmartGrowHub.Maui.Services.App;
using SmartGrowHub.Maui.Services.Infrastructure;

namespace SmartGrowHub.Maui.Services.Flow;

public interface ILogoutService
{
    Eff<Unit> LogOut(CancellationToken cancellationToken);
}

public sealed class LogoutService(
    INavigationService navigationService,
    IAuthService authService,
    ITokensStorage tokensStorage) : ILogoutService
{
    public Eff<Unit> LogOut(CancellationToken cancellationToken) =>
        from refreshToken in tokensStorage.GetRefreshToken(cancellationToken)
        from _ in authService.LogOut(refreshToken, cancellationToken)
        from __ in navigationService.SetLogInAsRoot(cancellationToken: cancellationToken)
        select _;
}