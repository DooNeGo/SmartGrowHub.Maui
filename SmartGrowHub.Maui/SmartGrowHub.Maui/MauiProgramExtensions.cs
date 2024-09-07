using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace SmartGrowHub.Maui;

public static class MauiProgramExtensions
{
    public static MauiAppBuilder UseSharedMauiApp(this MauiAppBuilder builder)
    {
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("Inter.ttf", "Inter");
                fonts.AddFont("Inter-18pt-Medium.ttf", "Inter18Medium");
                fonts.AddFont("Inter-18pt-SemiBold.ttf", "Inter18SemiBold");
            });

        builder.Services
            .AddUi()
            .AddServices();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder;
    }
}
