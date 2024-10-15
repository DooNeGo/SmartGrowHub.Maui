using CommunityToolkit.Mvvm.Messaging;
using SmartGrowHub.Application.LogIn;
using SmartGrowHub.Application.LogOut;
using SmartGrowHub.Application.RefreshTokens;
using SmartGrowHub.Application.Register;
using SmartGrowHub.Application.Services;
using SmartGrowHub.Maui.Application.Interfaces;
using SmartGrowHub.Shared.Auth.Dto.LogIn;
using SmartGrowHub.Shared.Auth.Dto.LogOut;
using SmartGrowHub.Shared.Auth.Dto.Register;
using SmartGrowHub.Shared.Auth.Extensions;
using SmartGrowHub.Shared.HttpErrors;
using SmartGrowHub.Shared.UserSessions.Dto.RefreshTokens;
using SmartGrowHub.Shared.UserSessions.Extensions;

namespace SmartGrowHub.Maui.Infrastructure.Services;

internal sealed class AuthService(
    IHttpService httpService,
    IUserSessionProvider sessionProvider,
    IMessenger messenger)
    : IAuthService
{
    public Eff<LogInResponse> LogIn(LogInRequest request, CancellationToken cancellationToken) =>
        from requestDto in request.TryToDto().ToEff()
        from result in httpService
            .PostAsync<LogInRequestDto, LogInResponseDto, HttpError>("auth/login", requestDto, cancellationToken)
            .Bind(either => either.Match(
                Left: httpError => httpError.ToError(),
                Right: response => response.TryToDomain().ToEff()))
        from _ in sessionProvider.SaveAndSetSession(result.UserSession, cancellationToken)
        select result;

    public Eff<LogOutResponse> LogOut(LogOutRequest request, CancellationToken cancellationToken) =>
        httpService
            .PostAsync<LogOutRequestDto, LogOutResponse, string>("auth/logout", request.ToDto(), cancellationToken)
            .Bind(either => either.Match(
                Left: error => Error.New(error),
                Right: response => SuccessEff(response)))
            .Bind(response => sessionProvider
                .RemoveSession()
                .Map(_ => response));

    public Eff<RefreshTokensResponse> RefreshTokens(RefreshTokensRequest request, CancellationToken cancellationToken) =>
        httpService
            .PostAsync<RefreshTokensRequestDto, RefreshTokensResponseDto, HttpError>("auth/refresh", request.ToDto(), cancellationToken)
            .Bind(either => either.Match(
                Left: httpError => httpError.ToError(),
                Right: response => response.TryToDomain().ToEff()))
            .Bind(response => sessionProvider
                .UpdateTokens(response.AuthTokens, cancellationToken)
                .Map(_ => response));

    public Eff<RegisterResponse> Register(RegisterRequest request, CancellationToken cancellationToken) =>
        from requestDto in request.TryToDto().ToEff()
        from result in httpService
            .PostAsync<RegisterRequestDto, RegisterResponse, HttpError>("auth/register", requestDto, cancellationToken)
            .Bind(either => either.Match(
                Left: httpError => httpError.ToError(),
                Right: response => SuccessEff(response)))
        select result;
}
