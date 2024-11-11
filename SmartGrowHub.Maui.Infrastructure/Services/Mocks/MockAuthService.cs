using SmartGrowHub.Application.LogIn;
using SmartGrowHub.Application.LogOut;
using SmartGrowHub.Application.RefreshTokens;
using SmartGrowHub.Application.Register;
using SmartGrowHub.Application.Services;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.Maui.Infrastructure.Services.Mocks;

internal sealed class MockAuthService : IAuthService
{
    public Eff<LogInResponse> LogIn(LogInRequest request, CancellationToken cancellationToken)
    {
        return Pure(new LogInResponse(
            new UserSession(
                new Id<UserSession>(Ulid.Empty),
                new Id<User>(Ulid.Empty),
                new AuthTokens(
                    AccessToken.From("asdasdsadasdas").ThrowIfFail(),
                    new RefreshToken(Ulid.Empty, DateTime.UtcNow.AddDays(2))))));
    }

    public Eff<LogOutResponse> LogOut(LogOutRequest request, CancellationToken cancellationToken)
    {
        return Pure(new LogOutResponse());
    }

    public Eff<RefreshTokensResponse> RefreshTokens(RefreshTokensRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Eff<RegisterResponse> Register(RegisterRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}