using AsyncAwaitBestPractices;
using epj.RouteGenerator;
using SmartGrowHub.Maui.Services.App;

namespace SmartGrowHub.Maui;

[AutoRoutes("Page")]
public sealed partial class App
{
    private readonly INavigationService _navigationService;
    private readonly AppShell _appShell;

    public App(INavigationService navigationService, AppShell appShell)
    {
        _navigationService = navigationService;
        _appShell = appShell;
        UserAppTheme = AppTheme.Light;
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState) => new(_appShell);

    protected override void OnStart() =>
        _navigationService.InitializeRootPage().RunAsync().SafeFireAndForget();
}