using epj.RouteGenerator;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;

namespace SmartGrowHub.Maui;

[AutoRoutes("Page")]
[ExtraRoute(nameof(NavigationPage), typeof(NavigationPage))]
public sealed partial class App
{
    public App()
    {
        UserAppTheme = AppTheme.Light;
        On<Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
        InitializeComponent();
    }
}