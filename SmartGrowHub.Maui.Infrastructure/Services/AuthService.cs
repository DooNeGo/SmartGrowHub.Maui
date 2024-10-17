using Mediator;
using SmartGrowHub.Application.LogIn;
using SmartGrowHub.Application.LogOut;
using SmartGrowHub.Application.RefreshTokens;
using SmartGrowHub.Application.Register;
using SmartGrowHub.Application.Services;
using SmartGrowHub.Domain.Errors;
using SmartGrowHub.Domain.Extensions;
using SmartGrowHub.Maui.Application.Messages.Notifications;
using SmartGrowHub.Maui.Infrastructure.Services.Extensions;
using SmartGrowHub.Shared.Auth.Dto.LogIn;
using SmartGrowHub.Shared.Auth.Dto.LogOut;
using SmartGrowHub.Shared.Auth.Dto.Register;
using SmartGrowHub.Shared.Auth.Extensions;
using SmartGrowHub.Shared.Errors;
using SmartGrowHub.Shared.Errors.Extensions;
using SmartGrowHub.Shared.UserSessions.Dto.RefreshTokens;
using SmartGrowHub.Shared.UserSessions.Extensions;

namespace SmartGrowHub.Maui.Infrastructure.Services;

internal sealed class AuthService(HttpClient httpClient, IMediator mediator) : IAuthService
{
    public Eff<LogInResponse> LogIn(LogInRequest request, CancellationToken cancellationToken) =>
        from requestDto in Pure(request.ToDto())
        from result in httpClient
            .PostAsync<LogInRequestDto, LogInResponseDto, ErrorDto>("auth/login", requestDto, cancellationToken)
            .Bind(either => either.Match(
                Left: errorDto => errorDto.ToDomain(),
                Right: response => response.TryToDomain().ToEff()))
        select result;

    public Eff<LogOutResponse> LogOut(LogOutRequest request, CancellationToken cancellationToken) =>
        httpClient
            .PostAsync<LogOutRequestDto, LogOutResponse, ErrorDto>("auth/logout", request.ToDto(), cancellationToken)
            .Bind(either => either.Match(
                Left: errorDto => errorDto.ToDomain(),
                Right: response => SuccessEff(response)));

    public Eff<RefreshTokensResponse> RefreshTokens(RefreshTokensRequest request, CancellationToken cancellationToken) =>
        httpClient
            .PostAsync<RefreshTokensRequestDto, RefreshTokensResponseDto, ErrorDto>("auth/refresh", request.ToDto(), cancellationToken)
            .Bind(either => either.Match(
                Left: errorDto => errorDto.ToDomain(),
                Right: response => response.TryToDomain().ToEff()))
            | @catch(DomainErrors.SessionNotFoundError, error => mediator
                .Publish(NoAuthorizeNotification.Default).ToEff()
                .Bind(_ => FailEff<RefreshTokensResponse>(error)));

    public Eff<RegisterResponse> Register(RegisterRequest request, CancellationToken cancellationToken) =>
        from requestDto in Pure(request.ToDto())
        from result in httpClient
            .PostAsync<RegisterRequestDto, RegisterResponse, ErrorDto>("auth/register", requestDto, cancellationToken)
            .Bind(either => either.Match(
                Left: errorDto => errorDto.ToDomain(),
                Right: response => SuccessEff(response)))
        select result;
}
