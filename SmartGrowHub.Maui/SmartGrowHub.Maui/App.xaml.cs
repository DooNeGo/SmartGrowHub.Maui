using LanguageExt.Async;
using SmartGrowHub.Maui.Application.Interfaces;

namespace SmartGrowHub.Maui;

public sealed partial class App
{
    private readonly AppShell _shell;
    private readonly IUserSessionProvider _sessionProvider;

    public App(AppShell shell, IUserSessionProvider sessionProvider)
    {
        InitializeComponent();

        UserAppTheme = AppTheme.Light;
        MainPage = _shell = shell;
        _sessionProvider = sessionProvider;
    }

    protected override void OnStart()
    {
        //using CancellationTokenSource tokenSource = new(TimeSpan.FromSeconds(3));

        //_ = Async.await(_sessionProvider
        //    .GetUserSession(tokenSource.Token)
        //    .Bind(_ => _shell.SetMainAsRoot(false, tokenSource.Token))
        //    .RunAsync());

        base.OnStart();
    }
}
