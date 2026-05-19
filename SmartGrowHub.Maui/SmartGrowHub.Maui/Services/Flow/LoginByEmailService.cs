using Serilog;
using SmartGrowHub.Maui.Services.Api;
using SmartGrowHub.Maui.Services.Extensions;

namespace SmartGrowHub.Maui.Services.Flow;

public interface ILoginByEmailService
{
    IO<Unit> SendOtpToEmail(string emailAddress, CancellationToken cancellationToken);
    IO<Unit> CheckOtp(string oneTimePassword, CancellationToken cancellationToken);
}

public sealed class LoginByEmailService(
    ILogger logger,
    ISecureStorage secureStorage,
    IAuthService authService)
    : ILoginByEmailService
{
    public IO<Unit> SendOtpToEmail(string emailAddress, CancellationToken cancellationToken) =>
        authService
            .SendOtpToEmail(emailAddress, cancellationToken)
            .TapOnFail(error => IO.lift(() => logger.Error(error.ToException(), "Failed to send otp to email")));

    public IO<Unit> CheckOtp(string oneTimePassword, CancellationToken cancellationToken) => (
        from response in authService.CheckOtp(oneTimePassword, cancellationToken)
        from _ in secureStorage.SetAuthTokens(response)
        select _
    ).Run().As().TapOnFail(error => IO.lift(() => logger.Error(error.ToException(), "Failed to check otp"))).ToUnit();
}