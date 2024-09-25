namespace SmartGrowHub.Maui;

public sealed partial class AppShell
{
    public AppShell() => InitializeComponent();

    public Eff<Unit> SetUpMainPageAsStartPage() =>
        SetUpRootPage(MainTabBar);

    public Eff<Unit> SetUpStartPageAsStartPage() =>
        SetUpRootPage(StartPage);

    private Eff<Unit> SetUpRootPage(ShellItem item) =>
        Pure(Application.Current!.Dispatcher
            .Dispatch(() => CurrentItem = item))
            .Map(_ => unit);
}
