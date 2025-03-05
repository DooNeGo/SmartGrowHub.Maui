using SmartGrowHub.Maui.Services.Extensions;
using SmartGrowHub.Maui.Services.Infrastructure;
using SmartGrowHub.Shared.Auth;
using SmartGrowHub.Shared.Results;
using SmartGrowHub.Shared.Tokens;

namespace SmartGrowHub.Maui.Services.Api;

public interface IAuthService
{
    OptionT<IO, AuthTokensDto> CheckOtp(string otp, CancellationToken cancellationToken);
    OptionT<IO, AuthTokensDto> RefreshTokens(string refreshToken, CancellationToken cancellationToken);
    IO<Unit> SendOtpToEmail(string emailAddress, CancellationToken cancellationToken);
    IO<Unit> LogOut(string refreshToken, CancellationToken cancellationToken);
}

public sealed class AuthService : IAuthService
{
    private readonly HttpService _httpService;

    public AuthService(HttpService httpService)
    {
        _httpService = httpService;
        _httpService.ClientName = nameof(IAuthService);
    }

    public IO<Unit> SendOtpToEmail(string emailAddress, CancellationToken cancellationToken) => (
        from response in _httpService.PostAsJsonAsync<Result, LogInByEmailRequest>(
            "/api/auth/login/email", new LogInByEmailRequest(emailAddress), cancellationToken)
        from _ in response.ToIO()
        select _
    ).ToFailIO(Error.Empty);

    public OptionT<IO, AuthTokensDto> CheckOtp(string otp, CancellationToken cancellationToken) =>
        _httpService.PostAsJsonAsync<Result<AuthTokensDto>, CheckOtpRequest>(
            "/api/auth/login/check", new CheckOtpRequest(otp), cancellationToken
        ).Bind(result => result.ToOptionTIO());

    public OptionT<IO, AuthTokensDto> RefreshTokens(string refreshToken, CancellationToken cancellationToken) =>
        _httpService.PostAsJsonAsync<Result<AuthTokensDto>, RefreshTokensRequest>(
            "/api/auth/refresh", new RefreshTokensRequest(refreshToken), cancellationToken
        ).Bind(result => result.ToOptionTIO());

    public IO<Unit> LogOut(string refreshToken, CancellationToken cancellationToken) =>
        _httpService.PostAsJsonAsync<Result, LogoutRequest>(
            "/api/auth/logout", new LogoutRequest(refreshToken), cancellationToken
        ).Bind(result => result.ToIO()).ToFailIO(Error.Empty);
}
