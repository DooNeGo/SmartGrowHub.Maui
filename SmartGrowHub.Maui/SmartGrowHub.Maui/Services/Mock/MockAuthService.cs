using SmartGrowHub.Domain.Features.LogIn;
using SmartGrowHub.Domain.Features.LogOut;
using SmartGrowHub.Domain.Features.RefreshTokens;
using SmartGrowHub.Domain.Features.Register;
using SmartGrowHub.Maui.Services.Abstractions;
using SmartGrowHub.Shared.Auth.Dto.LogIn;
using SmartGrowHub.Shared.Auth.Extensions;
using SmartGrowHub.Shared.UserSessions.Dto;

namespace SmartGrowHub.Maui.Services.Mock;

public sealed class MockAuthService(IUserSessionProvider sessionProvider) : IAuthService
{
    public Eff<LogInResponse> LogInAsync(LogInRequest request, bool remember, CancellationToken cancellationToken)
    {
        return new LogInResponseDto(new UserSessionDto(
            Ulid.NewUlid(),
            Ulid.NewUlid(),
            "asdasdasdasdsadasdadasdas",
            "asdasdsadadadasdasdada"))
            .TryToDomain().ToEff()
            .Bind(response => IO.yield(1000).Map(_ => response))
            .Bind(response => sessionProvider
                .SetSession(response.UserSession)
                .Map(_ => response));
    }

    public Eff<LogOutResponse> LogOutAsync(CancellationToken cancellationToken)
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