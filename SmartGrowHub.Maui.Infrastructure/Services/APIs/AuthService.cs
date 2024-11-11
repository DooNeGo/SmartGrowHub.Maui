using SmartGrowHub.Application.LogIn;
using SmartGrowHub.Application.LogOut;
using SmartGrowHub.Application.RefreshTokens;
using SmartGrowHub.Application.Register;
using SmartGrowHub.Application.Services;
using SmartGrowHub.Maui.Infrastructure.Services.Extensions;
using SmartGrowHub.Shared.Auth.Dto.LogIn;
using SmartGrowHub.Shared.Auth.Dto.LogOut;
using SmartGrowHub.Shared.Auth.Dto.Register;
using SmartGrowHub.Shared.Auth.Extensions;
using SmartGrowHub.Shared.Errors;
using SmartGrowHub.Shared.Errors.Extensions;
using SmartGrowHub.Shared.UserSessions.Dto.RefreshTokens;
using SmartGrowHub.Shared.UserSessions.Extensions;

namespace SmartGrowHub.Maui.Infrastructure.Services.APIs;

internal sealed class AuthService(HttpClient httpClient) : IAuthService
{
    public Eff<LogInResponse> LogIn(LogInRequest request, CancellationToken cancellationToken) =>
        httpClient
            .PostAsync<LogInRequestDto, LogInResponseDto, ErrorDto>("auth/login", request.ToDto(), cancellationToken)
            .Bind(either => either.Match(
                Left: errorDto => errorDto.ToDomain(),
                Right: response => response.TryToDomain().ToEff()));

    public Eff<LogOutResponse> LogOut(LogOutRequest request, CancellationToken cancellationToken) =>
        httpClient
            .PostAsync<LogOutRequestDto, LogOutResponse, ErrorDto>("auth/logout", request.ToDto(), cancellationToken)
            .Bind(either => either.Match(
                Left: errorDto => errorDto.ToDomain(),
                Right: SuccessEff));

    public Eff<RefreshTokensResponse> RefreshTokens(RefreshTokensRequest request, CancellationToken cancellationToken) =>
        httpClient
            .PostAsync<RefreshTokensRequestDto, RefreshTokensResponseDto, ErrorDto>("auth/refresh", request.ToDto(), cancellationToken)
            .Bind(either => either.Match(
                Left: errorDto => errorDto.ToDomain(),
                Right: response => response.TryToDomain().ToEff()));

    public Eff<RegisterResponse> Register(RegisterRequest request, CancellationToken cancellationToken) =>
        httpClient
            .PostAsync<RegisterRequestDto, RegisterResponse, ErrorDto>("auth/register", request.ToDto(), cancellationToken)
            .Bind(either => either.Match(
                Left: errorDto => errorDto.ToDomain(),
                Right: SuccessEff));
}
