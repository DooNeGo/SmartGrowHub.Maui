using SmartGrowHub.Maui.Services.Extensions;
using SmartGrowHub.Shared.Auth;
using SmartGrowHub.Shared.Results;
using SmartGrowHub.Shared.Tokens;

namespace SmartGrowHub.Maui.Services.Api;

public interface IAuthService
{
    Eff<AuthTokensDto> CheckOtp(int otp, CancellationToken cancellationToken);
    Eff<AuthTokensDto> RefreshTokens(string refreshToken, CancellationToken cancellationToken);
    Eff<Unit> SendOtpToEmail(string emailAddress, CancellationToken cancellationToken);
    Eff<Unit> LogOut(string refreshToken, CancellationToken cancellationToken);
}

public sealed class AuthService(HttpClient httpClient) : IAuthService
{
    public Eff<Unit> SendOtpToEmail(string emailAddress, CancellationToken cancellationToken) =>
        httpClient
            .PostJsonAsync<Result, LogInByEmailRequest>("/api/auth/login/email", new LogInByEmailRequest(emailAddress), cancellationToken)
            .Bind(result => result.ToEff());

    public Eff<AuthTokensDto> CheckOtp(int otp, CancellationToken cancellationToken) =>
        httpClient
            .PostJsonAsync<Result<AuthTokensDto>, CheckOtpRequest>("/api/auth/login/check", new CheckOtpRequest(otp), cancellationToken)
            .Bind(result => result.ToEff());

    public Eff<AuthTokensDto> RefreshTokens(string refreshToken, CancellationToken cancellationToken) =>
        httpClient
            .PostJsonAsync<Result<AuthTokensDto>, RefreshTokensRequest>("/api/auth/refresh", new RefreshTokensRequest(refreshToken), cancellationToken)
            .Bind(result => result.ToEff());
    
    public Eff<Unit> LogOut(string refreshToken, CancellationToken cancellationToken) =>
        httpClient
            .PostJsonAsync<Result, LogoutRequest>("/api/auth/logout", new LogoutRequest(refreshToken), cancellationToken)
            .Bind(result => result.ToEff());
}
