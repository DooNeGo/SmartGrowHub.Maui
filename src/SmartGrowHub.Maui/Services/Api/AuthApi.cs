using SmartGrowHub.Maui.Services.Extensions;
using SmartGrowHub.Maui.Services.Infrastructure;
using SmartGrowHub.Shared.Auth;
using SmartGrowHub.Shared.Results;
using SmartGrowHub.Shared.Tokens;

namespace SmartGrowHub.Maui.Services.Api;

public interface IAuthApi
{
    IO<AuthTokensDto> VerifyOtp(string otp);
    IO<AuthTokensDto> RefreshTokens(string refreshToken);
    IO<Unit> RequestOtpToEmail(string emailAddress);
    IO<Unit> LogOut(string refreshToken);
}

public sealed class AuthApi : IAuthApi
{
    private readonly IHttpService _httpService;

    public AuthApi(IHttpService httpService)
    {
        _httpService = httpService;
        _httpService.ClientName = nameof(IAuthApi);
    }

    public IO<Unit> RequestOtpToEmail(string emailAddress) => (
        from response in _httpService.PostAsJson<Result, RequestOtpToEmailRequest>(
            "/api/auth/otp/email", new RequestOtpToEmailRequest(emailAddress))
        from _ in response.ToIO()
        select _
    ).ToIOOrFail("Response was null");

    public IO<AuthTokensDto> VerifyOtp(string otp) =>
        _httpService.PostAsJson<Result<AuthTokensDto>, VerifyOtpRequest>(
            "/api/auth/otp/verify", new VerifyOtpRequest(otp)
        ).ToIOOrFail("Response was null").Bind(result => result.ToIO());

    public IO<AuthTokensDto> RefreshTokens(string refreshToken) =>
        _httpService.PostAsJson<Result<AuthTokensDto>, RefreshTokensRequest>(
            "/api/auth/tokens/refresh", new RefreshTokensRequest(refreshToken)
        ).ToIOOrFail("Response was null").Bind(result => result.ToIO());

    public IO<Unit> LogOut(string refreshToken) =>
        _httpService.PostAsJson<Result, LogoutRequest>(
            "/api/auth/logout", new LogoutRequest(refreshToken)
        ).ToIOOrFail("Response was null").Bind(result => result.ToIO());
}
