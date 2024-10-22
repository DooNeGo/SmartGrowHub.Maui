using Mediator;
using SmartGrowHub.Application.RefreshTokens;
using SmartGrowHub.Application.Services;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Maui.Application.Commands;
using SmartGrowHub.Maui.Application.Interfaces;

namespace SmartGrowHub.Maui.MediatorHandlers.Commands;

internal sealed class RefreshTokensHandler(
    IAuthService authService,
    IUserSessionProvider sessionProvider)
    : ICommandHandler<RefreshTokensCommand, AuthTokens>
{
    public ValueTask<AuthTokens> Handle(RefreshTokensCommand command, CancellationToken cancellationToken) =>
        RefreshTokens(cancellationToken).RunUnsafeAsync();

    private Eff<AuthTokens> RefreshTokens(CancellationToken cancellationToken) =>
        from refreshToken in sessionProvider.GetRefreshToken(cancellationToken)
        from request in Pure(new RefreshTokensRequest(refreshToken))
        from response in authService.RefreshTokens(request, cancellationToken)
        from _ in sessionProvider.UpdateTokens(response.AuthTokens, cancellationToken)
        select response.AuthTokens;
}
