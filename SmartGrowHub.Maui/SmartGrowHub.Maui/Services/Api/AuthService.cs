using SmartGrowHub.Maui.Services.Extensions;
using SmartGrowHub.Maui.Services.Infrastructure;
using SmartGrowHub.Shared.Auth;
using SmartGrowHub.Shared.Results;
using SmartGrowHub.Shared.Tokens;

namespace SmartGrowHub.Maui.Services.Api;

public interface IAuthService
{
    OptionT<Eff, AuthTokensDto> CheckOtp(int otp, CancellationToken cancellationToken);
    OptionT<Eff, AuthTokensDto> RefreshTokens(string refreshToken, CancellationToken cancellationToken);
    Eff<Unit> SendOtpToEmail(string emailAddress, CancellationToken cancellationToken);
    Eff<Unit> LogOut(string refreshToken, CancellationToken cancellationToken);
}

public sealed class AuthService : IAuthService
{
    private readonly HttpService _httpService;

    public AuthService(HttpService httpService)
    {
        _httpService = httpService;
        _httpService.ClientName = nameof(IAuthService);
    }

    public Eff<Unit> SendOtpToEmail(string emailAddress, CancellationToken cancellationToken) =>
        _httpService.PostJsonAsync<Result, LogInByEmailRequest>(
            "/api/auth/login/email", new LogInByEmailRequest(emailAddress), cancellationToken
        ).Run().As().Bind(option => option.Match(
            Some: result => result.ToEff(),
            None: () => Pure(unit)));

    public OptionT<Eff, AuthTokensDto> CheckOtp(int otp, CancellationToken cancellationToken) =>
        _httpService.PostJsonAsync<Result<AuthTokensDto>, CheckOtpRequest>(
            "/api/auth/login/check", new CheckOtpRequest(otp), cancellationToken
        ).Bind(result => OptionT.lift(result.ToEff()));

    public OptionT<Eff, AuthTokensDto> RefreshTokens(string refreshToken, CancellationToken cancellationToken) =>
        _httpService.PostJsonAsync<Result<AuthTokensDto>, RefreshTokensRequest>(
            "/api/auth/refresh", new RefreshTokensRequest(refreshToken), cancellationToken
        ).Bind(result => OptionT.lift(result.ToEff()));

    public Eff<Unit> LogOut(string refreshToken, CancellationToken cancellationToken) =>
        _httpService.PostJsonAsync<Result, LogoutRequest>(
            "/api/auth/logout", new LogoutRequest(refreshToken), cancellationToken
        ).Run().As().Bind(option => option.Match(
            Some: result => result.ToEff(),
            None: () => Pure(unit)));
}
