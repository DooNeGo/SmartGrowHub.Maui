using SmartGrowHub.Maui.Application.Interfaces;

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

        Task.Run(() => _sessionProvider.GetUserSession(tokenSource.Token)
            .Bind(_ => _shell.SetMainAsRoot())
            .RunAsync()
            .Map(fin => fin.IfFail(error => _dialogService
                .DisplayAlert("Start up error", error.Message, Localization.Resources.Ok))))
            .GetAwaiter().GetResult();

        base.OnStart();
    }

    private Unit OnSessionSet() => _shell
        .SetMainAsRoot().Run();

    private Unit OnSessionRemoved() => _shell
        .SetLogInAsRoot().Run();
}
