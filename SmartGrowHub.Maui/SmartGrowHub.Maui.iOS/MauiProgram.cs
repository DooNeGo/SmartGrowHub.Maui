using Microsoft.Maui.Controls.Handlers.Items2;
using Microsoft.Maui.Handlers;
using UIKit;

namespace SmartGrowHub.Maui.iOS;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        MauiAppBuilder builder = MauiApp.CreateBuilder();

        EntryHandler.Mapper.AppendToMapping("SetUpEntry", (handler, _) =>
            handler.PlatformView.BorderStyle = UITextBorderStyle.None);

        builder
            .UseSharedMauiApp()
            .ConfigureMauiHandlers(collection =>
                collection.AddHandler<CollectionView, CollectionViewHandler2>());

        return builder.Build();
    }
}
