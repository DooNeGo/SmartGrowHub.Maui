namespace SmartGrowHub.Maui;

public sealed partial class App
{
    private readonly AppShell _shell;
    
    public App(AppShell shell)
    {
        _shell = shell;
        UserAppTheme = AppTheme.Light;
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState) => new(_shell);
}