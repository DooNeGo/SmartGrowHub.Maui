using Serilog;
using SmartGrowHub.Maui.Services.Api;
using SmartGrowHub.Maui.Services.Extensions;
using SmartGrowHub.Shared.Tokens;

namespace SmartGrowHub.Maui.Services.App;

public interface IAuthService
{
    IO<Unit> RequestOtpToEmail(string emailAddress);
    IO<Unit> VerifyOtp(string oneTimePassword);
    IO<Unit> RefreshTokens();
    IO<Unit> LogOut();
}

internal sealed class AuthService : IAuthService
{
    private readonly IAuthApi _authApi;
    private readonly ISecureStorage _secureStorage;
    private readonly ILogger _logger;

    public AuthService(
        IAuthApi authApi,
        ISecureStorage secureStorage,
        ILogger logger)
    {
        _authApi = authApi;
        _secureStorage = secureStorage;
        _logger = logger;
    }

    public IO<Unit> RequestOtpToEmail(string emailAddress) =>
        _authApi
            .RequestOtpToEmail(emailAddress)
            .TapOnFail(error => IO.lift(() => _logger.Error(error.ToException(), "Failed to send otp to email")));

    public IO<Unit> VerifyOtp(string oneTimePassword) => (
        from response in _authApi.VerifyOtp(oneTimePassword)
        from _ in _secureStorage.SetAuthTokens(response)
        select _
    ).TapOnFail(error => IO.lift(() => _logger.Error(error.ToException(), "Failed to check otp")));
    
    public IO<Unit> RefreshTokens() =>
        from refreshToken in _secureStorage.GetRefreshToken()
            .ToIOOrFail("No refresh token found")
            .TapOnFail(_ => LogOut())
        from authTokens in RefreshTokens(refreshToken)
        from _ in _secureStorage.SetAuthTokens(authTokens)
        select _;

    private IO<AuthTokensDto> RefreshTokens(string refreshToken) =>
        _authApi.RefreshTokens(refreshToken)
            .Retry(Schedule.Once)
            .TapOnFail(error => error.Code is 3 ? LogOut() : IO.pure(Unit.Default));
    
    public IO<Unit> LogOut() =>
        from _1 in LogOutFromServer()
        from _2 in _secureStorage.RemoveAllValues()
        select _2;

    private IO<Unit> LogOutFromServer() => (
        from refreshToken in _secureStorage.GetRefreshToken()
        from _ in _authApi.LogOut(refreshToken)
        select _
    ).IfNone(Unit.Default).As();
}