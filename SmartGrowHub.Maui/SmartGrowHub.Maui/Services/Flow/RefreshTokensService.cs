using Microsoft.Extensions.Logging;
using SmartGrowHub.Maui.Services.Api;
using SmartGrowHub.Maui.Services.Extensions;
using SmartGrowHub.Maui.Services.Infrastructure;
using SmartGrowHub.Shared.Tokens;

namespace SmartGrowHub.Maui.Services.Flow;

public interface IRefreshTokensService
{
    Eff<AuthTokensDto> RefreshTokens(CancellationToken cancellationToken);
}

public sealed class RefreshTokensService(
    ITokensStorage tokensStorage,
    IAuthService authService,
    ILogger<RefreshTokensService> logger,
    IAuthorizationErrorHandler authorizationErrorHandler)
    : IRefreshTokensService
{
    public Eff<AuthTokensDto> RefreshTokens(CancellationToken cancellationToken) =>
        from refreshToken in tokensStorage.GetRefreshToken(cancellationToken)
        from response in authService.RefreshTokens(refreshToken, cancellationToken)
                         | @catch(error => error != Errors.Cancelled, error =>
                             OnError<AuthTokensDto>(error, cancellationToken))
        from _ in tokensStorage.SetAuthTokens(response, cancellationToken)
        select response;
    
    private Eff<T> OnError<T>(Error error, CancellationToken cancellationToken) =>
        from _1 in logger.LogErrorEff(error)
        from _2 in authorizationErrorHandler.Handle(cancellationToken)
        from result in FailEff<T>(error)
        select result;
}