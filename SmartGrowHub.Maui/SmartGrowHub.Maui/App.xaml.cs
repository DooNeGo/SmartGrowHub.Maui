using SmartGrowHub.Maui.Application.Interfaces;

namespace SmartGrowHub.Maui;

public sealed partial class App
{
    private readonly AppShell _shell;

    public App(AppShell shell, IUserSessionProvider sessionProvider)
    {
        InitializeComponent();
        UserAppTheme = AppTheme.Light;

        using CancellationTokenSource tokenSource = new(TimeSpan.FromSeconds(3));

        Task.Run(() => sessionProvider
            .GetUserSession(tokenSource.Token)
            .Bind(_ => shell.SetMainAsRoot())
            .RunAsync(), tokenSource.Token)
            .GetAwaiter().GetResult();

        _shell = shell;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        MainPage = _shell;
        return base.CreateWindow(activationState);
    }
}
