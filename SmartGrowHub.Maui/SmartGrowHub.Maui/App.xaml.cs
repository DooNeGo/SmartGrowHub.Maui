namespace SmartGrowHub.Maui;

public sealed partial class App
{
    public App(AppShell shell)
    {
        InitializeComponent();

        UserAppTheme = AppTheme.Light;
        MainPage = shell;
    }
}
