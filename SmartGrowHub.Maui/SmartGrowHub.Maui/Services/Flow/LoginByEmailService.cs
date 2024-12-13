using Microsoft.Extensions.Logging;
using SmartGrowHub.Maui.Features.LogIn.ViewModel;
using SmartGrowHub.Maui.Services.Api;
using SmartGrowHub.Maui.Services.App;
using SmartGrowHub.Maui.Services.Infrastructure;

namespace SmartGrowHub.Maui.Services.Flow;

public interface ILoginByEmailService
{
    IO<Unit> Start(CancellationToken cancellationToken);
    Eff<Unit> SendOtpToEmail(string emailAddress, CancellationToken cancellationToken);
    Eff<Unit> CheckOtp(int otpValue, CancellationToken cancellationToken);
}

public sealed partial class LoginByEmailService(
    ILogger<LoginByEmailService> logger,
    ITokensStorage tokensStorage,
    IAuthService authService,
    INavigationService navigationService,
    IDialogService dialogService)
    : ILoginByEmailService
{
    public IO<Unit> Start(CancellationToken cancellationToken) =>
        navigationService.GoToAsync(nameof(LoginByEmailPageModel), cancellationToken);

    public Eff<Unit> SendOtpToEmail(string emailAddress, CancellationToken cancellationToken) =>
        from _ in dialogService.ShowLoadingAsync()
        from __ in authService.SendOtpToEmail(emailAddress, cancellationToken)
            .Bind(_ => dialogService.Pop())
            .IfFailEff(_ => dialogService.Pop())
        select _;

    public Eff<Unit> CheckOtp(int otpValue, CancellationToken cancellationToken) =>
        from response in authService.CheckOtp(otpValue, cancellationToken)
        from _ in tokensStorage.SetAuthTokens(response, cancellationToken)
        from __ in navigationService.SetMainPageAsRoot(cancellationToken: cancellationToken)
        select _;

    private Eff<Unit> LogErrorEff(Error error) =>
        liftEff(() => LogError(logger, error.Message));

    [LoggerMessage(LogLevel.Error, Message = "{message}")]
    static partial void LogError(ILogger logger, string message);
}
