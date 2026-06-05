using epj.RouteGenerator;

namespace SmartGrowHub.Maui;

[AutoRoutes("Page")]
[ExtraRoute(nameof(NavigationPage), typeof(NavigationPage))]
public sealed partial class App
{
    public App()
    {
        UserAppTheme = AppTheme.Light;
        InitializeComponent();
    }
}