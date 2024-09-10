using SmartGrowHub.Maui.Services;
using SmartGrowHub.Maui.Services.Abstractions;

namespace SmartGrowHub.Maui;

public sealed partial class App
{
    public App(AppShell shell, IAuthService authService, IDialogService dialogService)
    {
        InitializeComponent();

        UserAppTheme = AppTheme.Light;
        MainPage = shell;

        using CancellationTokenSource tokenSource = new(TimeSpan.FromSeconds(10));
        authService.LogInIfRememberAsync(tokenSource.Token)
            .Match(
                Some: _ => shell.SetUpMainPageAsStartPage(),
                None: () => { },
                Fail: exception => dialogService.DisplayAlertAsync(
                    Localization.Resources.Authorization,
                    Localization.Resources.FailedLogInYourAccount,
                    Localization.Resources.Ok,
                    tokenSource.Token))
            .GetAwaiter().GetResult();
    }
}
