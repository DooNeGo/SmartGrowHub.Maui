using epj.RouteGenerator;

namespace SmartGrowHub.Maui;

[AutoRoutes("Page")]
public sealed partial class App
{
    public App()
    {
        UserAppTheme = AppTheme.Light;
        InitializeComponent();
    }

    //protected override void OnStart() =>
      //  _navigationService.InitializeRootPage().RunAsync().SafeFireAndForget();
}