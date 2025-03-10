using CommunityToolkit.Maui;
using MPowerKit.Navigation;
using MPowerKit.Navigation.Popups;
using MPowerKit.Regions;
using SmartGrowHub.Maui.Features;
using SmartGrowHub.Maui.Services;
using SmartGrowHub.Maui.Services.App;
using SmartGrowHub.Maui.Services.Extensions;
#if DEBUG
using Microsoft.Extensions.Logging;
#endif

namespace SmartGrowHub.Maui;

public static class MauiProgramExtensions
{
    public static MauiAppBuilder UseSharedMauiApp(this MauiAppBuilder builder)
    {
#pragma warning disable CA1416
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
#pragma warning restore CA1416
            .UseMPowerKitNavigation(mvvmBuilder => mvvmBuilder
                .ConfigureServices(collection => collection
                    .AddFeatures()
                    .AddServices())
                .UsePopupNavigation()
                .UsePageEventsInRegions()
                .OnAppStart(async (provider, _) =>
                {
                    var navigationService = provider.GetRequiredService<INavigationService>();
                    var secureStorage = provider.GetRequiredService<ISecureStorage>();

                    await secureStorage
                        .GetRefreshToken()
                        .Match(
                            Some: _ => $"{Routes.NavigationPage}/{Routes.StartPage}",
                            None: () => $"{Routes.NavigationPage}/{Routes.StartPage}").As()
                        .Bind(route => navigationService.NavigateAsync(route))
                        .RunAsync();
                }))
            .UseMPowerKitRegions()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("Inter-Regular.ttf", "InterRegular");
                fonts.AddFont("Inter-SemiBold.ttf", "InterSemiBold");
                fonts.AddFont("Inter-Medium.ttf", "InterMedium");
            });
        
#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder;
    }
}