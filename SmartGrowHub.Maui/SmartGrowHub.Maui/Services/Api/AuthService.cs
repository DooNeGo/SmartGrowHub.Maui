using CommunityToolkit.Mvvm.Messaging;
using SmartGrowHub.Domain.Features.LogIn;
using SmartGrowHub.Domain.Features.LogOut;
using SmartGrowHub.Domain.Features.RefreshTokens;
using SmartGrowHub.Domain.Features.Register;
using SmartGrowHub.Maui.Services.Abstractions;
using SmartGrowHub.Shared.Auth.Dto.LogIn;
using SmartGrowHub.Shared.Auth.Dto.LogOut;
using SmartGrowHub.Shared.Auth.Extensions;
using SmartGrowHub.Shared.UserSessions.Dto;
using SmartGrowHub.Shared.UserSessions.Dto.RefreshTokens;

namespace SmartGrowHub.Maui.Services.Api;

public sealed class AuthService(
    IHttpService httpService,
    IUserSessionProvider sessionProvider,
    IMessenger messenger)
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
                .Map(_ => response))
            .Bind(response => liftEff(() => messenger.Send(response)));

    public Eff<LogOutResponse> LogOutAsync(CancellationToken cancellationToken) =>
        sessionProvider.GetUserSessionIdAsync(cancellationToken)
            .Bind(option => option.Match(
                Some: id => httpService
                    .PostAsync<LogOutRequestDto, LogOutResponse>(
                        "auth/logout", new LogOutRequestDto(id), cancellationToken)
                    .Map(optionResponse => optionResponse.IfNone(new LogOutResponse())),
                None: Pure(new LogOutResponse())))
            .Bind(response => sessionProvider
                .Remove()
                .Map(_ => response));

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
