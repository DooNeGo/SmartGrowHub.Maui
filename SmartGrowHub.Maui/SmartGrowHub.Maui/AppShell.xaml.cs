namespace SmartGrowHub.Maui;

public sealed partial class AppShell
{
    public AppShell() => InitializeComponent();

    public Unit SetUpMainPageAsStartPage()
    {
        CurrentItem = MainTabBar;
        return unit;
    }
}
