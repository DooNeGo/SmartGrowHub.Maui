using Serilog;
using SmartGrowHub.Maui.Services.Api;
using SmartGrowHub.Maui.Services.Extensions;

namespace SmartGrowHub.Maui.Services.Flow;

public interface ILoginByEmailService
{
    IO<Unit> SendOtpToEmail(string emailAddress);
    IO<Unit> CheckOtp(string oneTimePassword);
}

public sealed class LoginByEmailService(
    ILogger logger,
    ISecureStorage secureStorage,
    IAuthService authService)
    : ILoginByEmailService
{
    public IO<Unit> SendOtpToEmail(string emailAddress) =>
        authService
            .RequestOtpToEmail(emailAddress)
            .TapOnFail(error => IO.lift(() => logger.Error(error.ToException(), "Failed to send otp to email")));

    public IO<Unit> CheckOtp(string oneTimePassword) => (
        from response in authService.VerifyOtp(oneTimePassword)
        from _ in secureStorage.SetAuthTokens(response)
        select _
    ).TapOnFail(error => IO.lift(() => logger.Error(error.ToException(), "Failed to check otp")));
}