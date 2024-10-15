using SmartGrowHub.Maui.Services.Extensions;

namespace SmartGrowHub.Maui;

public sealed partial class AppShell
{
    public AppShell() => InitializeComponent();

    public IO<Unit> SetMainAsRoot() =>
        SetRootPage(MainTabBar);

    public IO<Unit> SetLogInAsRoot() =>
        SetRootPage(StartPage);

    private IO<Unit> SetRootPage(ShellItem item) =>
        lift(() => Dispatcher
            .InvokeOnUiThreadIfNeeded(() =>
            {
                CurrentItem = item;
                return unit;
            }));
}
