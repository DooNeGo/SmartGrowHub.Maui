using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.Maui.Application.Interfaces;

public interface IRefreshTokensService
{
    Eff<AuthTokens> RefreshTokens(CancellationToken cancellationToken);
}
