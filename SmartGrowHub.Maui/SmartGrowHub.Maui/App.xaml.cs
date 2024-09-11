using SmartGrowHub.Maui.Services;
using SmartGrowHub.Maui.Services.Abstractions;

namespace SmartGrowHub.Maui;

public sealed partial class App
{
    private readonly AppShell _shell;

    public App(AppShell shell, IAuthService authService, IDialogService dialogService)
    {
        InitializeComponent();
        _shell = shell;

        UserAppTheme = AppTheme.Light;
        MainPage = shell;

        authService.LoggedOut += OnLoggedOut;

        using CancellationTokenSource tokenSource = new(TimeSpan.FromSeconds(5));
        authService.LogInIfRememberAsync(tokenSource.Token)
            .Match(
                Some: _ => OnLoggedIn(),
                None: () => unit,
                Fail: _ => dialogService.DisplayAlert(
                    Localization.Resources.Authorization,
                    Localization.Resources.FailedLogInYourAccount,
                    Localization.Resources.Ok,
                    tokenSource.Token))
            .GetAwaiter().GetResult();
    }

    private Unit OnLoggedIn() => _shell.SetUpMainPageAsStartPage();

    private Unit OnLoggedOut() => _shell.SetUpStartPageAsStartPage();
}
