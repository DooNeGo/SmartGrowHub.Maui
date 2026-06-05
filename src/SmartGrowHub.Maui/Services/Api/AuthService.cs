using SmartGrowHub.Maui.Services.Extensions;
using SmartGrowHub.Maui.Services.Infrastructure;
using SmartGrowHub.Shared.Auth;
using SmartGrowHub.Shared.Results;
using SmartGrowHub.Shared.Tokens;

namespace SmartGrowHub.Maui.Services.Api;

public interface IAuthService
{
    IO<AuthTokensDto> VerifyOtp(string otp);
    IO<AuthTokensDto> RefreshTokens(string refreshToken);
    IO<Unit> RequestOtpToEmail(string emailAddress);
    IO<Unit> LogOut(string refreshToken);
}

public sealed class AuthService : IAuthService
{
    private readonly IHttpService _httpService;

    public AuthService(IHttpService httpService)
    {
        _httpService = httpService;
        _httpService.ClientName = nameof(IAuthService);
    }

    public IO<Unit> RequestOtpToEmail(string emailAddress) => (
        from response in _httpService.PostAsJsonAsync<Result, RequestOtpToEmailRequest>(
            "/api/auth/otp/email", new RequestOtpToEmailRequest(emailAddress))
        from _ in response.ToIO()
        select _
    ).ToIOOrFail("Response was null");

    public IO<AuthTokensDto> VerifyOtp(string otp) =>
        _httpService.PostAsJsonAsync<Result<AuthTokensDto>, VerifyOtpRequest>(
            "/api/auth/otp/verify", new VerifyOtpRequest(otp)
        ).ToIOOrFail("Response was null").Bind(result => result.ToIO());

    public IO<AuthTokensDto> RefreshTokens(string refreshToken) =>
        _httpService.PostAsJsonAsync<Result<AuthTokensDto>, RefreshTokensRequest>(
            "/api/auth/tokens/refresh", new RefreshTokensRequest(refreshToken)
        ).ToIOOrFail("Response was null").Bind(result => result.ToIO());

    public IO<Unit> LogOut(string refreshToken) =>
        _httpService.PostAsJsonAsync<Result, LogoutRequest>(
            "/api/auth/logout", new LogoutRequest(refreshToken)
        ).ToIOOrFail("Response was null").Bind(result => result.ToIO());
}
