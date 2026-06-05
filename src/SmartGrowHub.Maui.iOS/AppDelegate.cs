using Foundation;

namespace SmartGrowHub.Maui.iOS;

[Register(nameof(AppDelegate))]
#pragma warning disable CA1711
public sealed class AppDelegate : MauiUIApplicationDelegate
#pragma warning restore CA1711
{
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}