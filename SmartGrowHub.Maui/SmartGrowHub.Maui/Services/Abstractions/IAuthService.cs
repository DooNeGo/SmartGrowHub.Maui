using SmartGrowHub.Domain.Features.LogIn;
using SmartGrowHub.Domain.Features.LogOut;
using SmartGrowHub.Domain.Features.RefreshTokens;
using SmartGrowHub.Domain.Features.Register;

namespace SmartGrowHub.Maui.Services.Abstractions;

public interface IAuthService
{
    Eff<LogInResponse> LogInAsync(LogInRequest request, bool remember, CancellationToken cancellationToken);
    Eff<RegisterResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken);
    Eff<RefreshTokensResponse> RefreshTokensAsync(RefreshTokensRequest request, CancellationToken cancellationToken);
    Eff<LogOutResponse> LogOutAsync(CancellationToken cancellationToken);
}
