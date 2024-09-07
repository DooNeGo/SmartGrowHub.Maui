using Microsoft.Maui.Handlers;

namespace SmartGrowHub.Maui.iOS;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        MauiAppBuilder builder = MauiApp.CreateBuilder();

        EntryHandler.Mapper.AppendToMapping("SetUpEntry", (handler, _) =>
            handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None);

        builder
            .UseSharedMauiApp();

        return builder.Build();
    }
}
