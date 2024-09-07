namespace SmartGrowHub.Maui;

public sealed partial class App
{
    public App(Shell shell)
    {
        InitializeComponent();

        UserAppTheme = AppTheme.Light;
        MainPage = shell;
    }
}
