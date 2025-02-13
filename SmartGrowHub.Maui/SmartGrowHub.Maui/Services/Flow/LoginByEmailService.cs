using Microsoft.Extensions.Logging;
using SmartGrowHub.Maui.Services.Api;
using SmartGrowHub.Maui.Services.App;
using SmartGrowHub.Maui.Services.Extensions;

namespace SmartGrowHub.Maui.Services.Flow;

public interface ILoginByEmailService
{
    IO<Unit> Start();
    Eff<Unit> SendOtpToEmail(string emailAddress, CancellationToken cancellationToken);
    Eff<Unit> CheckOtp(int oneTimePassword, CancellationToken cancellationToken);
}

public sealed class LoginByEmailService(
    ILogger<LoginByEmailService> logger,
    ISecureStorage secureStorage,
    IAuthService authService,
    INavigationService navigationService,
    IDialogService dialogService)
    : ILoginByEmailService
{
    public IO<Unit> Start() => navigationService.NavigateAsync(Routes.LoginByEmailPage);
    
    public Eff<Unit> SendOtpToEmail(string emailAddress, CancellationToken cancellationToken) =>
        from _1 in dialogService.ShowLoadingAsync()
        from _2 in authService
            .SendOtpToEmail(emailAddress, cancellationToken)
            .IfFailEff(OnError<Unit>)
        from _3 in dialogService.Pop()
        from _4 in navigationService
            .CreateBuilder()
            .SetRoute(Routes.CheckCodePage)
            .AddRouteParameter("SentTo", emailAddress)
            .NavigateAsync()
        select _3;

    public Eff<Unit> CheckOtp(int oneTimePassword, CancellationToken cancellationToken) => (
        from _1 in dialogService.ShowLoadingAsync()
        from response in authService.CheckOtp(oneTimePassword, cancellationToken)
        from _2 in secureStorage.SetAuthTokens(response)
        from _3 in dialogService.Pop()
        from _4 in navigationService.NavigateAsync($"//{Routes.MainPage}")
        select _4
    ).Run().As().IfFailEff(OnError<Option<Unit>>).Map(_ => unit);

    private Eff<T> OnError<T>(Error error) =>
        from _1 in dialogService.Pop()
        from _2 in logger.LogErrorEff(error)
        from result in FailEff<T>(error)
        select result;
}