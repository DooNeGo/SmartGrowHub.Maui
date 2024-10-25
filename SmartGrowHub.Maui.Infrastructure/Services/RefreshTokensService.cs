using SmartGrowHub.Application.RefreshTokens;
using SmartGrowHub.Application.Services;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Errors;
using SmartGrowHub.Maui.Application.Interfaces;

namespace SmartGrowHub.Maui.Infrastructure.Services;

internal sealed class RefreshTokensService(
    IUserSessionProvider sessionProvider,
    IAuthService authService,
    INoAuthorizeService noAuthorizeService)
    : IRefreshTokensService
{
    public Eff<AuthTokens> RefreshTokens(CancellationToken cancellationToken) =>
        from refreshToken in sessionProvider.GetRefreshToken(cancellationToken)
        let request = new RefreshTokensRequest(refreshToken)
        from response in authService.RefreshTokens(request, cancellationToken)
        | @catch(DomainErrors.SessionNotFoundError, error => noAuthorizeService
            .Handle(cancellationToken)
            .Bind(_ => FailEff<RefreshTokensResponse>(error)))
        from _ in sessionProvider.UpdateTokens(response.AuthTokens, cancellationToken)
        select response.AuthTokens;
}
