using SmartGrowHub.Domain.Features.LogIn;
using SmartGrowHub.Domain.Features.RefreshTokens;
using SmartGrowHub.Domain.Features.Register;
using SmartGrowHub.Maui.Services.Abstractions;

namespace SmartGrowHub.Maui.Services.Mock;

public sealed class MockAuthService : IAuthService
{
    public Eff<LogInResponse> LogInAsync(LogInRequest request, bool remember, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Eff<Unit> LogOutAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Eff<RefreshTokensResponse> RefreshTokensAsync(RefreshTokensRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Eff<RegisterResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}