namespace SmartGrowHub.Maui;

public sealed partial class AppShell
{
    public AppShell() => InitializeComponent();

    public Unit SetUpMainPageAsStartPage()
    {
        CurrentItem = MainTabBar;
        return unit;
    }

    public Unit SetUpStartPageAsStartPage()
    {
        CurrentItem = StartPage;
        return unit;
    }
}
