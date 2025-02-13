using CommunityToolkit.Maui;
using Mopups.Hosting;
using SmartGrowHub.Maui.Features;
using SmartGrowHub.Maui.Services;
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
            .ConfigureMopups()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("Inter-Regular.ttf", "InterRegular");
                fonts.AddFont("Inter-SemiBold.ttf", "InterSemiBold");
                fonts.AddFont("Inter-Medium.ttf", "InterMedium");
            });

        builder.Services
            .AddSingleton<AppShell>()
            .AddServices()
            .AddFeatures();
        
#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder;
    }
}