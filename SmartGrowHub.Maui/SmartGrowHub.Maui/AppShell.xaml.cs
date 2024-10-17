using SmartGrowHub.Domain.Extensions;

namespace SmartGrowHub.Maui;

public sealed partial class AppShell
{
    public AppShell() => InitializeComponent();

    public IO<Unit> SetMainAsRoot() =>
        GoToAsync("///MainTabBar", false).ToIO();

    public IO<Unit> SetLogInAsRoot() =>
        GoToAsync("///StartPage", false).ToIO();
}
