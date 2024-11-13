namespace SmartGrowHub.Maui;

public sealed partial class App
{
    private readonly AppShell _shell;

    public App(AppShell shell)
    {
        InitializeComponent();

        UserAppTheme = AppTheme.Light;
        _shell = shell;
    }

    protected override Window CreateWindow(IActivationState? activationState) => new(_shell);
}
