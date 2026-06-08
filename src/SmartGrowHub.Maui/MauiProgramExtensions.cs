using CommunityToolkit.Maui;
using MPowerKit.Navigation;
using MPowerKit.Navigation.Popups;
using MPowerKit.Regions;
using Serilog;
using SmartGrowHub.Maui.Features;
using SmartGrowHub.Maui.Services;
using SmartGrowHub.Maui.Services.App;
using SmartGrowHub.Maui.Services.Extensions;

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
                .ConfigureServices(ConfigureServices)
                .UsePopupNavigation()
                .UsePageEventsInRegions()
                .OnAppStart(OnAppStarted))
            .UseMPowerKitRegions()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("Inter-Regular.ttf", "InterRegular");
                fonts.AddFont("Inter-SemiBold.ttf", "InterSemiBold");
                fonts.AddFont("Inter-Medium.ttf", "InterMedium");
            });

        AppDomain.CurrentDomain.UnhandledException += (_, e) =>
        {
            var message = e.ExceptionObject.ToString();

            if (e.ExceptionObject is Exception exception)
            {
                Log.Fatal(exception, "Unhandled exception");
            }
            else
            {
                Log.Fatal("Unhandled exception: {message}", message);
            }

            Clipboard.SetTextAsync(message).GetAwaiter().GetResult();
        };

        return builder;
    }

    private static void ConfigureServices(IServiceCollection collection)
    {
        collection
            .AddFeatures()
            .AddServices()
            .AddSerilog(configuration =>
            {
#if DEBUG
                configuration.WriteTo.Debug();
#endif
            });
    }

    private static async ValueTask OnAppStarted(IServiceProvider provider, MPowerKit.Navigation.Interfaces.INavigationService _1)
    {
        var navigationService = provider.GetRequiredService<INavigationService>();
        var secureStorage = provider.GetRequiredService<ISecureStorage>();

        _ = await secureStorage.GetRefreshToken()
            .Match(
                Some: _ => $"{Routes.NavigationPage}/{Routes.MainPage}",
                None: () => $"{Routes.NavigationPage}/{Routes.StartPage}")
            .Bind(route => navigationService.NavigateAsync(route))
            .RunAsync();
    }
}