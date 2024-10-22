using SmartGrowHub.Domain.Extensions;

namespace SmartGrowHub.Maui;

public sealed partial class AppShell
{
    public AppShell() => InitializeComponent();

    public IO<Unit> SetMainAsRoot(bool animate = true) =>
        GoToAsync("//MainTabBar", animate).ToIO();

    public IO<Unit> SetLogInAsRoot(bool animate = true) =>
        GoToAsync("//StartPage", animate).ToIO();
}
