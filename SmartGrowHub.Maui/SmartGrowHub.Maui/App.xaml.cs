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
        sessionProvider.SessionSet += OnSessionSet;

        using CancellationTokenSource tokenSource = new(TimeSpan.FromSeconds(6));

        _ = sessionProvider.GetUserSessionAsync(tokenSource.Token)
            .Bind(option => option.Match(
                Some: _ => _shell.SetUpMainPageAsStartPage(),
                None: () => _shell.SetUpStartPageAsStartPage()))
            .Run()
            .IfFail(error => dialogService.DisplayAlert("Start up error", error.Message, "Ok"));
    }

    private Unit OnSessionSet() => _shell.SetUpMainPageAsStartPage().RunUnsafe();

    private Unit OnSessionRemoved() => _shell.SetUpStartPageAsStartPage().RunUnsafe();
}
