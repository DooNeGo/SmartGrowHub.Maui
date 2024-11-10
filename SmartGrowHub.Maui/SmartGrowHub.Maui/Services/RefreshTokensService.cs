using SmartGrowHub.Application.RefreshTokens;
using SmartGrowHub.Application.Services;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Errors;
using SmartGrowHub.Maui.Application.Interfaces;

namespace SmartGrowHub.Maui.Services;

internal sealed class RefreshTokensService(
    IUserSessionService sessionService,
    IAuthService authService,
    IAuthorizationErrorHandler authorizationErrorHandler)
    : IRefreshTokensService
{
    public Eff<AuthTokens> RefreshTokens(CancellationToken cancellationToken) =>
        from refreshToken in sessionService.GetRefreshToken(cancellationToken)
        let request = new RefreshTokensRequest(refreshToken)
        from response in authService.RefreshTokens(request, cancellationToken)
                         | @catch(DomainErrors.SessionNotFoundError, error => authorizationErrorHandler
                             .Handle(cancellationToken)
                             .Bind(_ => FailEff<RefreshTokensResponse>(error)))
        from _ in sessionService.UpdateTokens(response.AuthTokens, cancellationToken)
        select response.AuthTokens;
}
