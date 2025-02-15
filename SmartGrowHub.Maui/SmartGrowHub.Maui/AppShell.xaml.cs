using epj.RouteGenerator;

namespace SmartGrowHub.Maui;

[ExtraRoute("GrowHubComponentsTabbedPage", typeof(TabBar))]
public sealed partial class AppShell
{
    public AppShell() => InitializeComponent();
}
