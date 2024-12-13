using SmartGrowHub.Maui.Services.Api;
using SmartGrowHub.Maui.Services.Infrastructure;
using SmartGrowHub.Shared.Tokens;

namespace SmartGrowHub.Maui.Services.Flow;

public interface IRefreshTokensService
{
    Eff<AuthTokensDto> RefreshTokens(CancellationToken cancellationToken);
}

public sealed class RefreshTokensService(
    ITokensStorage tokensStorage,
    IAuthService authService) : IRefreshTokensService
{
    public Eff<AuthTokensDto> RefreshTokens(CancellationToken cancellationToken) =>
        from refreshToken in tokensStorage.GetRefreshToken(cancellationToken)
        from response in authService.RefreshTokens(refreshToken, cancellationToken)
        from _ in tokensStorage.SetAuthTokens(response, cancellationToken)
        select response;
}