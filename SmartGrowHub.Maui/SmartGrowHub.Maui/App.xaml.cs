using SmartGrowHub.Maui.Services;

namespace SmartGrowHub.Maui;

public sealed partial class App
{
    private readonly AppShell _shell;
    private readonly IUserSessionProvider _sessionProvider;
    private readonly IDialogService _dialogService;

    public App(AppShell shell, IUserSessionProvider sessionProvider, IDialogService dialogService)
    {
        _shell = shell;
        _sessionProvider = sessionProvider;
        _dialogService = dialogService;

        InitializeComponent();
        UserAppTheme = AppTheme.Light;
        MainPage = shell;

        sessionProvider.SessionRemoved += OnSessionRemoved;
        sessionProvider.SessionSet += OnSessionSet;
    }

    protected override void OnStart()
    {
        using CancellationTokenSource tokenSource = new(TimeSpan.FromSeconds(6));

        _ = Task.Run(() => _sessionProvider.GetUserSessionAsync(tokenSource.Token)
            .Bind(option => option.Match(
                Some: _ => _shell.SetUpMainPageAsStartPage(),
                None: () => _shell.SetUpStartPageAsStartPage()))
            .RunAsync())
            .GetAwaiter().GetResult()
            .IfFail(error => _dialogService.DisplayAlert("Start up error", error.Message, "Ok"));

        base.OnStart();
    }

    private Unit OnSessionSet() => _shell.SetUpMainPageAsStartPage().RunUnsafe();

    private Unit OnSessionRemoved() => _shell.SetUpStartPageAsStartPage().RunUnsafe();
}
