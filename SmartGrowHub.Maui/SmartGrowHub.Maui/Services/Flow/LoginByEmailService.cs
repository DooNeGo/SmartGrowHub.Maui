using Microsoft.Extensions.Logging;
using SmartGrowHub.Maui.Features.LogIn.ViewModel;
using SmartGrowHub.Maui.Services.Api;
using SmartGrowHub.Maui.Services.App;
using SmartGrowHub.Maui.Services.Extensions;
using SmartGrowHub.Maui.Services.Infrastructure;
using SmartGrowHub.Shared.Tokens;

namespace SmartGrowHub.Maui.Services.Flow;

public interface ILoginByEmailService
{
    IO<Unit> Start(CancellationToken cancellationToken);
    Eff<Unit> SendOtpToEmail(string emailAddress, CancellationToken cancellationToken);
    Eff<Unit> CheckOtp(int otpValue, CancellationToken cancellationToken);
}

public sealed class LoginByEmailService(
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
        from _1 in dialogService.ShowLoadingAsync()
        from _2 in authService
            .SendOtpToEmail(emailAddress, cancellationToken)
            .IfFailEff(OnError<Unit>)
        from _3 in dialogService.Pop()
        from _4 in navigationService.GoToAsync(
            $"{nameof(CheckCodePageModel)}?{nameof(CheckCodePageModel.SentTo)}={emailAddress}",
            cancellationToken)
        select _4;

    public Eff<Unit> CheckOtp(int otpValue, CancellationToken cancellationToken) =>
        from _1 in dialogService.ShowLoadingAsync()
        from response in authService
            .CheckOtp(otpValue, cancellationToken)
            .IfFailEff(OnError<AuthTokensDto>)
        from _2 in dialogService.Pop()
        from _3 in tokensStorage.SetAuthTokens(response, cancellationToken)
        from _4 in navigationService.SetMainPageAsRoot(cancellationToken: cancellationToken)
        select _4;

    private Eff<T> OnError<T>(Error error) =>
        from _1 in dialogService.Pop()
        from _2 in logger.LogErrorEff(error)
        from result in FailEff<T>(error)
        select result;
}