using SmartGrowHub.Maui.Services.Extensions;

namespace SmartGrowHub.Maui;

public sealed partial class AppShell
{
    public AppShell() => InitializeComponent();

    public Eff<Unit> SetUpMainPageAsStartPage() =>
        SetUpRootPage(MainTabBar);

    public Eff<Unit> SetUpStartPageAsStartPage() =>
        SetUpRootPage(StartPage);

    private Eff<Unit> SetUpRootPage(ShellItem item) =>
        Pure(Dispatcher.InvokeOnUiThreadIfNeeded(() =>
        {
            CurrentItem = item;
            return unit;
        }));
}
