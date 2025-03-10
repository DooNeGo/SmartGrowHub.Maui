using CommunityToolkit.Maui;
using Mopups.Hosting;
using MPowerKit.Navigation;
using MPowerKit.Popups;
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
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseMPowerKitNavigation(mvvmBuilder => mvvmBuilder
                .ConfigureServices(collection => collection.AddFeatures().AddServices())
                .OnAppStart(async (provider, navigation) =>
                {
                    var navigationService = provider.GetRequiredService<INavigationService>();
                    var secureStorage = provider.GetRequiredService<ISecureStorage>();

                    await secureStorage
                        .GetRefreshToken()
                        .Match(
                            Some: _ => $"/NavigationPage/{Routes.LoginByEmailPage}",
                            None: () => $"/NavigationPage/{Routes.LoginByEmailPage}").As()
                        .Bind(route => navigationService.NavigateAsync(route))
                        .RunAsync();
                }))
            .UseMPowerKitPopups()
            .UseMPowerKitRegions()
            .ConfigureMopups()
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