using SmartGrowHub.Maui.Services;

namespace SmartGrowHub.Maui;

public sealed partial class App
{
    private readonly AppShell _shell;

    public App(AppShell shell, IUserSessionProvider sessionProvider, IDialogService dialogService)
    {
        InitializeComponent();
        _shell = shell;

        UserAppTheme = AppTheme.Light;
        MainPage = shell;

        sessionProvider.SessionRemoved += OnSessionRemoved;
        sessionProvider.SessionSetted += OnSessionSetted;

        using CancellationTokenSource tokenSource = new(TimeSpan.FromSeconds(5));

        _ = sessionProvider.GetUserSessionAsync(tokenSource.Token)
            .Map(option => option.Match(
                Some: _ =>
                    OnSessionSetted(),
                None: () =>
                    OnSessionRemoved()))
            .Run()
            .IfFail(error => dialogService.DisplayAlert("Start up error", error.Message, "Ok"));
    }

    private Unit OnSessionSetted() => _shell.SetUpMainPageAsStartPage().RunUnsafe();

    private Unit OnSessionRemoved() => _shell.SetUpStartPageAsStartPage().RunUnsafe();
}
