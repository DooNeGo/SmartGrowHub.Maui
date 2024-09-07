using Android.Content.Res;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Handlers;

namespace SmartGrowHub.Maui.Droid;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    { 
        MauiAppBuilder builder = MauiApp.CreateBuilder();
        
        EntryHandler.Mapper.AppendToMapping("NoUnderLine", (handler, _) => 
            handler.PlatformView.BackgroundTintList = ColorStateList.ValueOf(Colors.Transparent.ToAndroid()));

        builder
            .UseSharedMauiApp();

        return builder.Build();
    }
}
