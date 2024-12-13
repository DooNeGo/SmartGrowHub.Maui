using Android.Content.Res;
using Microsoft.Maui.Handlers;
using Color = Android.Graphics.Color;

namespace SmartGrowHub.Maui.Droid;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        MauiAppBuilder builder = MauiApp.CreateBuilder();

        EntryHandler.Mapper.AppendToMapping("NoUnderLine", (handler, _) =>
            handler.PlatformView.BackgroundTintList = ColorStateList.ValueOf(Color.Transparent));
        
        builder
            .UseSharedMauiApp();

        return builder.Build();
    }
}