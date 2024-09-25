using SmartGrowHub.Domain.Features.LogIn;
using SmartGrowHub.Domain.Features.RefreshTokens;
using SmartGrowHub.Domain.Features.Register;
using SmartGrowHub.Maui.Services.Abstractions;
using SmartGrowHub.Shared.Auth.Dto.LogIn;
using SmartGrowHub.Shared.Auth.Extensions;
using SmartGrowHub.Shared.UserSessions.Dto;
using SmartGrowHub.Shared.UserSessions.Dto.RefreshTokens;

namespace SmartGrowHub.Maui.Services.Api;

public sealed class AuthService(
    IHttpService httpService,
    IUserSessionProvider sessionProvider)
    : IAuthService
{
    public Eff<LogInResponse> LogInAsync(LogInRequest request, bool remember, CancellationToken cancellationToken)
        => httpService
            .PostAsync<LogInRequestDto, LogInResponseDto>("auth/login", request.ToDto(), cancellationToken)
            .Bind(option => option.Match(
                Some: response => response.TryToDomain().ToEff(),
                None: () => Error.New("Log in response was empty")))
            .Bind(response => (remember
                ? sessionProvider.SaveAndSetSessionAsync(response.UserSession, cancellationToken)
                : sessionProvider.SetSession(response.UserSession))
                .Map(_ => response));

    public Eff<Unit> LogOutAsync(CancellationToken cancellationToken) => sessionProvider.Remove();

    public Eff<RefreshTokensResponse> RefreshTokensAsync(RefreshTokensRequest request, CancellationToken cancellationToken)
        => httpService
            .PostAsync<RefreshTokensRequestDto, RefreshTokensResponseDto>("auth/refresh", request.ToDto(), cancellationToken)
            .Bind(option => option.Match(
                Some: response => response.TryToDomain().ToEff(),
                None: () => Error.New("Refresh tokens response was empty")))
            .Bind(response => sessionProvider
                .UpdateTokensAsync(response.AuthTokens, cancellationToken)
                .Map(_ => response));

    public Eff<RegisterResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
